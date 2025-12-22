/**
 * Configuration object to be passed to MSAL instance on creation.
 * For a full list of MSAL Node configuration parameters, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-node/docs/configuration.md
 */

const authorityUrl = process.env.B2C_BASE_URL + '/B2C_1_login';

export const msalConfig = {
    auth: {
        clientId: process.env.B2C_CLIENT_ID,
        authority: authorityUrl,
        clientSecret: process.env.B2C_CLIENT_SECRET,
        knownAuthorities: [new URL(authorityUrl).host],
        protocolMode: 'OIDC',
    },
    system: {
        loggerOptions: {
            piiLoggingEnabled: false,
            logLevel: 2,
            loggerCallback(loglevel, message, containsPii) {
                if (!containsPii) log.info(message);
            },
        },
    },
};