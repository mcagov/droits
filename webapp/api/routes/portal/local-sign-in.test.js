import { showSignIn, signIn } from './local-sign-in';

const response = () => {
  const res = {};
  res.status = jest.fn(() => res);
  res.render = jest.fn(() => res);
  res.redirect = jest.fn(() => res);
  return res;
};

const request = (body) => ({
  body,
  session: { data: {} },
  login: jest.fn((user, done) => done()),
});

afterEach(() => {
  delete process.env.LOCAL_AUTH_EMAIL;
  delete process.env.LOCAL_AUTH_PASSWORD;
});

describe('showSignIn', () => {
  it('renders the local sign in page', () => {
    const res = response();

    showSignIn({}, res);

    expect(res.render).toHaveBeenCalledWith('portal/local-sign-in', { values: {} });
  });
});

describe('signIn', () => {
  it('signs in the local user and goes to the dashboard', () => {
    const users = [];
    const req = request({ email: 'dev@droits.local', password: 'password' });
    const res = response();

    signIn(users)(req, res, jest.fn());

    const user = { oid: 'local-dev@droits.local', emails: ['dev@droits.local'], displayName: 'Dev User' };
    expect(req.login).toHaveBeenCalledWith(user, expect.any(Function));
    expect(users).toEqual([user]);
    expect(req.session.user).toEqual(user);
    expect(req.session.data.email).toBe('dev@droits.local');
    expect(res.redirect).toHaveBeenCalledWith('/portal/dashboard');
  });

  it('does not add the user twice', () => {
    const users = [];

    signIn(users)(request({ email: 'dev@droits.local', password: 'password' }), response(), jest.fn());
    signIn(users)(request({ email: 'dev@droits.local', password: 'password' }), response(), jest.fn());

    expect(users).toHaveLength(1);
  });

  it('uses the configured email and password', () => {
    process.env.LOCAL_AUTH_EMAIL = 'someone@droits.local';
    process.env.LOCAL_AUTH_PASSWORD = 'secret';
    const res = response();

    signIn([])(request({ email: 'someone@droits.local', password: 'secret' }), res, jest.fn());

    expect(res.redirect).toHaveBeenCalledWith('/portal/dashboard');
  });

  it.each([
    [{ email: 'dev@droits.local', password: 'wrong' }],
    [{ email: 'someone@else.local', password: 'password' }],
    [{}],
  ])('rejects %p', (body) => {
    const users = [];
    const req = request(body);
    const res = response();

    signIn(users)(req, res, jest.fn());

    expect(res.status).toHaveBeenCalledWith(401);
    expect(res.render).toHaveBeenCalledWith(
      'portal/local-sign-in',
      expect.objectContaining({ values: { email: body.email } }),
    );
    expect(req.login).not.toHaveBeenCalled();
    expect(users).toEqual([]);
  });

  it('passes sign in errors on', () => {
    const error = new Error('session store down');
    const req = request({ email: 'dev@droits.local', password: 'password' });
    req.login = jest.fn((user, done) => done(error));
    const next = jest.fn();
    const res = response();

    signIn([])(req, res, next);

    expect(next).toHaveBeenCalledWith(error);
    expect(res.redirect).not.toHaveBeenCalled();
  });
});
