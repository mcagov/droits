import app from "../../../server";
import request from "supertest";

describe('Rate limit after exceeding max', () => {
    it('throws an error after 10 tries', async () => {

            for (let i = 0; i < 10; i++) {
                const res = await request(app).get("/auth/openid/return");

            }
            const res = await request(app).get('/auth/openid/return');
            expect(res.status).toBe(429);

    });
});

