const AuthProvider = require('./authProvider');
const msal = require('@azure/msal-node');
const axios = require('axios');


jest.mock('@azure/msal-node');
jest.mock('axios');
jest.unmock('./authProvider');

describe('AuthProvider Unit Tests', () => {
    let authProvider;
    let mockReq, mockRes, mockNext;
    let mockMsalInstance;

    const mockConfig = {
        auth: {
            clientId: 'test-client-id',
            authority: 'https://login.microsoftonline.com/test-tenant',
            clientSecret: 'secret',
            knownAuthorities: 'https://login.microsoftonline.com'
        }
    };

    beforeEach(() => {
        jest.clearAllMocks();
        
        mockReq = {
            session: {},
            body: {},
            query: {}
        };
        mockRes = {
            redirect: jest.fn(),
            status: jest.fn().mockReturnThis(),
            send: jest.fn()
        };
        mockNext = jest.fn();
        
        mockMsalInstance = {
            getAuthCodeUrl: jest.fn(),
            acquireTokenByCode: jest.fn(),
            acquireTokenSilent: jest.fn(),
            getTokenCache: jest.fn().mockReturnValue({
                serialize: jest.fn().mockReturnValue('mock-serialized-cache'),
                deserialize: jest.fn()
            })
        };
        
        msal.ConfidentialClientApplication.mockImplementation(() => mockMsalInstance);

        // Use real CryptoProvider for the test logic (it's logic-heavy)
        const RealCrypto = jest.requireActual('@azure/msal-node').CryptoProvider;
        msal.CryptoProvider = RealCrypto;

        authProvider = new AuthProvider(mockConfig);
    });

    describe('login()', () => {
        it('should fetch metadata if missing, set PKCE in session, and redirect', async () => {
    
            axios.get.mockResolvedValueOnce({ data: { tenant_discovery_endpoint: 'mock-tenant' } });
            axios.get.mockResolvedValueOnce({ data: { authorization_endpoint: 'mock-authorization' } });

      
            const mockAuthUrl = 'https://login.microsoftonline.com/common/oauth2/v2.0/authorize?state=...';
            mockMsalInstance.getAuthCodeUrl.mockResolvedValue(mockAuthUrl);

            const middleware = authProvider.login({
                redirectUri: 'http://localhost/dashboard',
                scopes: ['User.Read']
            });
            await middleware(mockReq, mockRes, mockNext);
            
            expect(axios.get).toHaveBeenCalledTimes(2);
            expect(mockReq.session.pkceCodes).toBeDefined();
            expect(mockReq.session.pkceCodes.verifier).toBeDefined();
            expect(mockRes.redirect).toHaveBeenCalledWith(mockAuthUrl);
        });
    });

    describe('handleRedirect()', () => {
        it('should exchange code for token and update session', async () => {
          
            mockReq.session.pkceCodes = { verifier: 'mock-verifier' };
            mockReq.session.authCodeRequest = { scopes: ['User.Read'] };

          
            const stateJson = JSON.stringify({ successRedirect: '/dashboard' });
            const stateBase64 = new msal.CryptoProvider().base64Encode(stateJson);

            mockReq.body = {
                code: 'mock-auth-code',
                state: stateBase64
            };

         
            mockMsalInstance.acquireTokenByCode.mockResolvedValue({
                idToken: 'id-token',
                account: { username: 'testuser' }
            });

            const middleware = authProvider.handleRedirect();
            await middleware(mockReq, mockRes, mockNext);


            expect(mockMsalInstance.acquireTokenByCode).toHaveBeenCalledWith(
                expect.objectContaining({ code: 'mock-auth-code'}),
                expect.anything()
            );

      
            expect(mockReq.session.isAuthenticated).toBe(true);
            expect(mockReq.session.account.username).toBe('testuser');
            expect(mockReq.session.tokenCache).toBe('mock-serialized-cache');
            expect(mockRes.redirect).toHaveBeenCalledWith('/dashboard');
        });
    });

    describe('acquireToken()', () => {
        it('should return cached token if available', async () => {
            mockReq.session.tokenCache = 'existing-cache-data';
            mockReq.session.account = { homeAccountId: '1' };

            mockMsalInstance.acquireTokenSilent.mockResolvedValue({
                accessToken: 'new-access-token',
                idToken: 'new-id-token',
                account: { homeAccountId: '1' }
            });

            const middleware = authProvider.acquireToken();
            await middleware(mockReq, mockRes, mockNext);
            
            expect(mockMsalInstance.getTokenCache().deserialize).toHaveBeenCalledWith('existing-cache-data');
            expect(mockMsalInstance.acquireTokenSilent).toHaveBeenCalled();
            expect(mockRes.redirect).toHaveBeenCalled(); 
        });

        it('should trigger login if silent acquisition fails (InteractionRequired)', async () => {

            axios.get.mockResolvedValueOnce({ data: { tenant_discovery_endpoint: 'mock-tenant' } });
            axios.get.mockResolvedValueOnce({ data: { authorization_endpoint: 'mock-authorization' } });
            
            const interactionError = new msal.InteractionRequiredAuthError('Login needed');
            mockMsalInstance.acquireTokenSilent.mockRejectedValue(interactionError);
            
            const loginSpy = jest.spyOn(authProvider, 'login');

            const middleware = authProvider.acquireToken({ redirectUri: '/dashboard' });
            await middleware(mockReq, mockRes, mockNext);

            expect(loginSpy).toHaveBeenCalled();

            expect(mockRes.redirect).toHaveBeenCalled();
        });
    });

    describe('logout()', () => {
        it('should destroy session and redirect to Azure logout', () => {
            mockReq.session.destroy = jest.fn((next) => next());

            const middleware = authProvider.logout({ postLogoutRedirectUri: 'http://localhost' });
            middleware(mockReq, mockRes, mockNext);

            expect(mockReq.session.destroy).toHaveBeenCalled();
            expect(mockRes.redirect).toHaveBeenCalledWith(
                expect.stringContaining('logout?post_logout_redirect_uri=http://localhost')
            );
        });
    });
});