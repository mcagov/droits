const deployedEnvironmentMarkers = [
  'ECS_CONTAINER_METADATA_URI_V4',
  'ECS_CONTAINER_METADATA_URI',
  'ECS_ENABLE_CONTAINER_METADATA',
];

export const isLocalAuthRequested = (env = process.env) =>
  (env.DROITS_LOCAL_AUTH || '').toLowerCase() === 'true';

export const isLocalAuthAllowed = (env = process.env) =>
  env.NODE_ENV === 'development' &&
  deployedEnvironmentMarkers.every((marker) => !env[marker]);

export const isLocalAuth = (env = process.env) =>
  isLocalAuthRequested(env) && isLocalAuthAllowed(env);

export const localAuthCredentials = (env = process.env) => ({
  email: env.LOCAL_AUTH_EMAIL || 'dev@droits.local',
  password: env.LOCAL_AUTH_PASSWORD || 'password',
});

export const signedOutRedirect = (path, env = process.env) =>
  isLocalAuth(env)
    ? path
    : `${env.B2C_BASE_URL}/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=${env.ENV_BASE_URL}${path}`;
