require("dotenv-json")();

const passport = require('passport');
const OIDCStrategy = require('passport-azure-ad').OIDCStrategy;
const bunyan = require('bunyan');
const log = bunyan.createLogger({
    name: 'Microsoft OIDC Example Web Application',
});

// array to hold logged-in users
export const users = [];

export const findByOid = function (oid, fn) {
    for (let i = 0, len = users.length; i < len; i++) {
        const user = users[i];
        if (user.oid === oid) {
            log.info('User logged in.');
            return fn(null, user);
        }
    }
    return fn(null, null);
};

export const serializeUser = function (user, done) {
    done(null, user.oid);
};

export const deserializeUser = function (oid, done) {
    findByOid(oid, function (err, user) {
        done(err, user);
    });
};

export const verifyStrategy = function (iss, sub, profile, accessToken, refreshToken, params, done) {
    if (!profile.oid) {
        return done(new Error('No oid found'), null);
    }

    // Asynchronous verification
    process.nextTick(function () {
        findByOid(profile.oid, function (err, user) {
            if (err) {
                return done(err);
            }
            if (!user) {
                // "Auto-registration"
                users.push(profile);
                return done(null, profile);
            }
            return done(null, user);
        });
    });
};

export default function (app) {
    app.use(passport.initialize());
    app.use(passport.session());

    passport.serializeUser(serializeUser);
    passport.deserializeUser(deserializeUser);
    
    passport.use(
    new OIDCStrategy(
      {
        identityMetadata: process.env.B2C_BASE_URL + process.env.B2C_IDENTITY_METADATA,
        clientID: process.env.B2C_CLIENT_ID,
        responseType: 'code id_token',
        responseMode: 'form_post',
        redirectUrl: process.env.ENV_BASE_URL + process.env.B2C_REDIRECT_URL,
        allowHttpForRedirectUrl: true,
        clientSecret: process.env.B2C_CLIENT_SECRET,
        validateIssuer: false,
        isB2C: true,
        issuer: null,
        passReqToCallback: false,
        useCookieInsteadOfSession: false,
        cookieSameSite: false,
        loggingLevel: 'info',
        loggingNoPII: true,
        scope: process.env.B2C_CLIENT_ID,
      }, verifyStrategy
    )
  );

  app.get(
    '/login',
    function (req, res, next) {
      passport.authenticate('azuread-openidconnect', {
        response: res, // required
        failureRedirect: `${process.env.B2C_BASE_URL}/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=${process.env.ENV_BASE_URL}/error`,
      })(req, res, next);
    },
    function (req, res) {
      log.info('We received a return from AzureAD.');
      res.render('portal/dashboard', { user: req.user });
    }
  );
}
