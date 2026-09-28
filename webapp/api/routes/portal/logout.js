import { signedOutRedirect } from '../../../utilities/localAuth';

export default function (app) {
  app.get('/logout', function (req, res) {
    req.session.destroy(function (err) {
      req.logOut();
      res.redirect(signedOutRedirect('/portal/start'));
    });
  });
}
