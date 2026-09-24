import {
  isLocalAuth,
  isLocalAuthRequested,
  localAuthCredentials,
  signedOutRedirect,
} from './localAuth';

const localEnv = { DROITS_LOCAL_AUTH: 'true', NODE_ENV: 'development' };

describe('isLocalAuth', () => {
  it('is on when requested in development', () => {
    expect(isLocalAuth(localEnv)).toBe(true);
  });

  it.each([undefined, '', 'false', 'yes'])('is off when DROITS_LOCAL_AUTH is %p', (flag) => {
    expect(isLocalAuth({ ...localEnv, DROITS_LOCAL_AUTH: flag })).toBe(false);
  });

  it.each(['production', 'test', undefined])('is off when NODE_ENV is %p', (nodeEnv) => {
    const env = { ...localEnv, NODE_ENV: nodeEnv };

    expect(isLocalAuthRequested(env)).toBe(true);
    expect(isLocalAuth(env)).toBe(false);
  });

  it.each([
    'ECS_CONTAINER_METADATA_URI_V4',
    'ECS_CONTAINER_METADATA_URI',
    'ECS_ENABLE_CONTAINER_METADATA',
  ])('is off when %s is set', (marker) => {
    expect(isLocalAuth({ ...localEnv, [marker]: 'true' })).toBe(false);
  });
});

describe('localAuthCredentials', () => {
  it('defaults to dev@droits.local and password', () => {
    expect(localAuthCredentials({})).toEqual({ email: 'dev@droits.local', password: 'password' });
  });

  it('uses the configured email and password', () => {
    const env = { LOCAL_AUTH_EMAIL: 'someone@droits.local', LOCAL_AUTH_PASSWORD: 'secret' };

    expect(localAuthCredentials(env)).toEqual({ email: 'someone@droits.local', password: 'secret' });
  });
});

describe('signedOutRedirect', () => {
  const b2cEnv = {
    B2C_BASE_URL: 'https://example.b2clogin.com/example.onmicrosoft.com',
    ENV_BASE_URL: 'https://droits.example',
  };

  it.each(['/portal/start', '/service-error', '/account-notification'])(
    'signs out of Azure AD B2C and returns to %s',
    (path) => {
      expect(signedOutRedirect(path, b2cEnv)).toBe(
        `https://example.b2clogin.com/example.onmicrosoft.com/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=https://droits.example${path}`,
      );
    },
  );

  it('ignores the flag in production and still signs out of Azure AD B2C', () => {
    const env = { ...b2cEnv, DROITS_LOCAL_AUTH: 'true', NODE_ENV: 'production' };

    expect(signedOutRedirect('/portal/start', env)).toMatch(/^https:\/\/example\.b2clogin\.com\//);
  });

  it('goes straight to the page with local auth', () => {
    expect(signedOutRedirect('/portal/start', { ...b2cEnv, ...localEnv })).toBe('/portal/start');
  });
});
