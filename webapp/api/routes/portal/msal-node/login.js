require("dotenv-json")();

import {msalConfig} from "../../../auth/authConfig";

const AuthProvider = require('../../../auth/authProvider');

const bunyan = require('bunyan');
const log = bunyan.createLogger({
    name: 'Microsoft OIDC Example Web Application',
    level: 'info',
});

export const authProvider = new AuthProvider(msalConfig);

export default function (app) {
    app.get(
        '/login',
        function (req, res, next) {
            authProvider.login({
                scopes: ['openid', 'profile', process.env.B2C_CLIENT_ID],
                redirectUri: process.env.ENV_BASE_URL + process.env.B2C_REDIRECT_URL,
            })(req, res, next);
        },
        function (req, res) {
            if (!req.session.isAuthenticated || !req.session.account) {
                const logoutUrl = `${process.env.B2C_BASE_URL}/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=${process.env.ENV_BASE_URL}/error`;
                return res.redirect(logoutUrl);
            }
            const profile = req.session.account.idTokenClaims;
            
            log.info('We received a return from AzureAD.');
            res.render('portal/dashboard', { user: profile });
        }
    );
};



