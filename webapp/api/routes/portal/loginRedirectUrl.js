const passport = require('passport');

export default function (app) {
  app
    .get(
      '/auth/openid/return',
      function (req, res, next) {
        passport.authenticate('azuread-openidconnect', {
          response: res,
          failureRedirect: '/error',
        })(req, res, function (err) {
          if (err) {
            // Redirect to the error page
            return res.redirect('/error');
          }

          next();
        });
      },
      function (req, res) {
        res.redirect('/portal/dashboard');
      }
    )
    .post(
      '/auth/openid/return',
      function (req, res, next) {
        passport.authenticate('azuread-openidconnect', {
          response: res,
          failureRedirect: '/error',
        })(req, res, next);
      },
      function (req, res) {

        const currentUserEmail = req.user.emails[0];

        req.session.user = req.user;
        req.session.data.email = currentUserEmail;

        res.redirect('/portal/dashboard');
      }
    );
}
