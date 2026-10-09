import { signedOutRedirect } from '../../../utilities/localAuth';

export default function (app) {

  app.get('/error', function (req, res) {
    req.session.destroy(function (err) {
      req.logOut();

      console.log('An Azure auth service error occurred');
      return res.redirect(signedOutRedirect('/service-error'));
    });
  });

  app.get('/account-error', function (req, res) {
    req.session.destroy(function (err) {
      console.log('An Azure auth account error occurred');
      req.logOut();
      return res.redirect(signedOutRedirect('/account-notification'));
    });
  });
}
