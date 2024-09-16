import {sessionData} from "./sessionData";

describe('sessionData', () => {
    it('should call next()', () => {
        const req = {
            session: {
                data: jest.fn()
            },
            path: ''
        };
        const res = {
            locals: {
                data: {}
            }
        };
        const next = jest.fn();
        
        sessionData(req, res, next);
        
        expect(next).toHaveBeenCalled();
    });
});