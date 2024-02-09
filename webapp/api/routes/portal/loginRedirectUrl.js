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
            // Save the error message in the session
            req.session.errorMessage = err.message;
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

        console.log("logged in user...:")
        console.dir(req.user);
        const currentUserEmail = req.user.emails[0];

        req.session.user = req.user;
        req.session.data.email = currentUserEmail;

        res.redirect('/portal/dashboard');
      }
    );
}
