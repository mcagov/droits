import { localAuthCredentials } from '../../../utilities/localAuth';

const signInError = {
  text: 'Enter the local email address and password',
  href: '#email',
};

export const showSignIn = (req, res) => {
  res.render('portal/local-sign-in', { values: {} });
};

export const signIn = (users) => (req, res, next) => {
  const { email, password } = localAuthCredentials();

  if (req.body.email !== email || req.body.password !== password) {
    return res.status(401).render('portal/local-sign-in', {
      values: { email: req.body.email },
      errors: { email: signInError },
      errorSummary: [signInError],
    });
  }

  const user = { oid: `local-${email}`, emails: [email], displayName: 'Dev User' };
  if (!users.some((existing) => existing.oid === user.oid)) {
    users.push(user);
  }

  req.login(user, (err) => {
    if (err) {
      return next(err);
    }

    req.session.user = user;
    req.session.data.email = email;
    res.redirect('/portal/dashboard');
  });
};

export default function (app, users) {
  app.get('/login', showSignIn);
  app.post('/login', signIn(users));
}
