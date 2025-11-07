import express from 'express';
import bodyParser from 'body-parser';
import nunjucks from 'nunjucks';
import path from 'path';

import {
  sessionData,
  addCheckedFunction,
  matchRoutes,
  addNunjucksFilters,
  forceHttps
} from './utilities';
import routes from './api/routes';
import config from './app/config.js';
import rateLimitMiddleware from './utilities/rateLimiter.js';

const connect_redis = require("connect-redis");

const cors = require('cors');

require("dotenv-json")();
const app = express();
app.options('*', cors());

// Rate Limiter applied to all routes
app.use(rateLimitMiddleware);

// Global vars
app.locals.serviceName = config.SERVICE_NAME;

// Local vars
const env = process.env.NODE_ENV;

if (env === 'production') {
  app.get('/', function(req, res, next){
    if(req.hostname === 'report-wreck-material.service.gov.uk'){
      res.redirect('https://www.gov.uk/report-wreck-material/reporting-wreck-material');
    }else{
      next();
    }
  });
}

let useHttps = process.env.USE_HTTPS || config.USE_HTTPS;

useHttps = useHttps.toLowerCase();

// Production session data
const session = require('express-session');
const redis = require("redis");
const redisStore = connect_redis(session);
const redisClient = redis.createClient({
    host: process.env.REDIS_HOST,
    port: 6379,
});

const isSecure = env === 'production' && useHttps === 'true';
if (isSecure) {
  app.use(forceHttps);
}

if (config.SERVICE_UNAVAILABLE) {
  nunjucks.configure('./app/static/', {
    autoescape: true,
    express: app
  });
  
  app.all('*', (req, res, next) => {
    console.log('Service Unavailable.');
    res.status(503);
    res.render('service-unavailable.html');
  });
} else {
  // Support for parsing data in POSTs
  app.use(bodyParser.json());
  app.use(
    bodyParser.urlencoded({
      extended: true,
    })
  );

  // Needed for secure cookies when running behind a proxy
  // https://expressjs.com/en/guide/behind-proxies.html
  app.set('trust proxy', 1);

  // Configure nunjucks environment
  const nunjucksAppEnv = nunjucks.configure(
    [
      path.join(__dirname, './node_modules/govuk-frontend/'),
      path.join(__dirname, './app/views/'),
    ],
    {
      autoescape: false,
      express: app,
      watch: env === 'development',
    }
  );
  addCheckedFunction(nunjucksAppEnv);
  addNunjucksFilters(nunjucksAppEnv);

  // Set views engine
  app.set('view engine', 'html');

  app.use(express.static(path.join(__dirname, './dist')));
  app.use('/uploads', express.static('uploads'));
  app.use('/auth', express.static(path.join(__dirname, 'app', 'static', 'auth')));


  app.use(
    '/assets',
    express.static(
      path.join(__dirname, './node_modules/govuk-frontend/govuk/assets')
    )
  );

  // Serve CSS files specifically
  app.use('/assets/css', (req, res, next) => {
    res.set('Cache-Control', 'no-cache, no-store, must-revalidate');
    next();
  }, express.static(path.join(__dirname, './dist/assets/css')));


  // Session uses service name to avoid clashes with other prototypes
  app.use(session({
    secret: process.env.CSRFT_SESSION_SECRET,
    store: new redisStore({
        client: redisClient,
        ttl: 3600
    }),
    sameSite: 'none',
    saveUninitialized: false,
    resave: false,
    cookie: { httpOnly: true , secure: isSecure},
    unset: 'destroy'
  }));

  // Manage session data. Assigns default values to data
  app.use(sessionData);


  app.get('/', function(req, res){
    res.render('index');
  })

  // Load API routes
  app.use('/', routes());

  // Disables caching when user clicks back button on confirmation page
  app.use('/report/check-your-answers', function (req, res, next) {
    res.set(
      'Cache-Control',
      'no-cache, private, no-store, must-revalidate, max-stale=0, post-check=0, pre-check=0'
    );
    next();
  });

  app.get(/^([^.]+)$/, function (req, res, next) {
      matchRoutes(req, res, next);
    }
  )

  // Redirect all POSTs to GETs
  app.post(/^\/([^.]+)$/, function (req, res) {
    res.redirect('/' + req.params[0]);
  });

  // Catch 404 and forward to error handler
  app.use(function (req, res, next) {
      const err = new Error(`Page not found: ${req.path}`);
      err.status = 404;

    next(err);
  });

  // Display error
  app.use(function (err, req, res, next) {

    if (err.message.indexOf('not found') > 0) {
      res.status(404).render('404');
    } else {
      res.status(err.status || 500);
      res.render('service-unavailable.html');
    }
  });
}

export default app;
