export default function (app) {

  app.get('/error', function (req, res) {
    const error = req.session.errorMessage || 'An error occurred';
    delete req.session.errorMessage;

    req.session.destroy(function (err) {
      req.logOut();

      console.log(error);
      res.render('service-error', { error });

      // return res.redirect(`${process.env.B2C_BASE_URL}/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=${process.env.ENV_BASE_URL}/service-error`);
    });
  });

  app.get('/account-error', function (req, res) {
    req.session.destroy(function (err) {
      console.dir(err);
      req.logOut();
      return res.redirect(`${process.env.B2C_BASE_URL}/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=${process.env.ENV_BASE_URL}/account-notification`);
    });
  });
}
