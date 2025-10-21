
import "../../../jest.config.js";
import request from "supertest";
require('dotenv-json')();
import app from '../../../server.js';
import path from "path";
import fs from 'fs';


const testFilePath = path.join(__dirname, 'test-upload.csv');
describe('POST /bulk-upload-file', () => {
    
    beforeAll(() => {
        fs.writeFileSync(testFilePath, 'Description,Quantity,Total value,Storage address line 1,Town,County,Postcode\nTest Item,1,£10,Test Address,Test Town,Test County,BB10 2AA');
    });

    afterAll(() => {
        fs.unlinkSync(testFilePath);
        if (fs.existsSync(testFilePath)) {
            fs.unlinkSync(testFilePath);
        }
    });
    
    it('should upload a file, respond with 200, and then delete the file', async () => {
        const res = await request(app)
            .post('/report/property-bulk')
            .attach('bulk-upload-file', testFilePath);

        expect(res.body.status).toBe(200);
        await new Promise(resolve => setTimeout(resolve, 100));
        
        const filesInUploadsDir = fs.readdirSync("./uploads");
        expect(filesInUploadsDir.length).toBe(0);
    });
});
