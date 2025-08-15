
import "../../../jest.config.js";
import request from "supertest";
require('dotenv-json')();
import app from '../../../server.js';
import path from "path";
import fs from 'fs';

describe('Uploaded file', () => {
    it('successfully uploads a CSV file', async () => {
        const testFilePath = path.join(__dirname, 'test-upload.csv');

        // csv test object for upload
        const csvContent = 'Description,Quantity,Total value,Storage address line 1,Town,County,Postcode\nTest Item,1,£10,Test Address,Test Town,Test County,BB10 2AA';

        // test file
        fs.writeFileSync(testFilePath, csvContent);

        try {
            const res = await request(app)
                .post('/report/property-bulk')
                .attach('bulk-upload-file', testFilePath);

            expect(res.body.status).toBe(200);
        } finally {
            // Clean up
            if (fs.existsSync(testFilePath)) {
                fs.unlinkSync(testFilePath);
            }
        }
    });
});
