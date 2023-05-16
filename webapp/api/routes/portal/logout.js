export default function (app) {
  app.get('/logout', function (req, res) {
    req.session.destroy(function (err) {
      req.logOut();
      res.redirect(`${process.env.B2C_BASE_URL}/oauth2/v2.0/logout?p=${process.env.B2C_SIGN_IN_FLOW_NAME}&post_logout_redirect_uri=${process.env.ENV_BASE_URL}/portal/start`);
    });
  });
}
