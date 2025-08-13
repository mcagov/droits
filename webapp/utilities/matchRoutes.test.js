import request from "supertest";
import app from "../server";

describe('User can visit the page', () => {
    it('should display the correct page with HTML content', async () => {
            const response = await request(app).get('/portal/start');
            expect(response.statusCode).toBe(200);
            
    });
});
            
