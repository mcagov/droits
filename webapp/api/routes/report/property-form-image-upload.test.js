import request from 'supertest';
import app from '../../../server';
import fs from "fs";

describe('Property Image Upload Routes', () => {
    afterAll(() => {
        if (fs.existsSync('uploads')) {
            fs.rmSync('uploads', {recursive: true, force: true});
        }
    });
    
    const agent = request.agent(app);
    
    describe('POST /report/property-form-image-upload/:prop_id', () => {
        const propId = 'test-property-123';
        const url = `/report/property-form-image-upload/${propId}`;

        it('should 200 and save image details to session on success', async () => {
            const testFile = Buffer.from('this is a test image');
            const response = await agent.post(url)
                .attach('image', testFile, 'test-image.jpg');
            
            expect(response.status).toBe(200);
            expect(response.body.originalFilename).toEqual('test-image.jpg');
            expect(response.body.uploadedFilename).toEqual(expect.any(String));
        });

        it('should return 400 if no file is provided', async () => {
            const response = await agent.post(url).send();
            expect(response.status).toBe(400);
            expect(response.body.error.text).toBe('Image upload failed. No file provided.');
        });

        it('should return error if file is too large (LIMIT_FILE_SIZE)', async () => {
            const largeFile = Buffer.alloc(5000001);
            const response = await agent.post(url)
                .attach('image', largeFile, 'large-file.jpg');
            
            expect(response.status).toBe(400);
            expect(response.body.error.text).toBe('The selected file must be smaller than 5MB');
        });

        it('should return an error for an invalid file type', async () => {
            const response = await agent
                .post(url)
                .attach('image', Buffer.from('this is an invalid image'), 'invalid-image.txt');
            
            expect(response.status).toBe(400);
            expect(response.body.error.text).toEqual("The selected file must be a jpg, jpeg or png");
        })

        it('should return error for req.body.image === "undefined"', async () => {
            const response = await agent.post(url)
                .field('image', 'undefined');
            
            expect(response.status).toBe(400);
            expect(response.body.error.text).toBe('Image upload failed. No file provided.');
        });

        it('should return 403 on prototype pollution attempt', async () => {
            const pollutionUrl = '/report/property-form-image-upload/__proto__';
            const testFile = Buffer.from('this is a test image');
            
            const response = await agent.post(pollutionUrl)
                .attach('image', testFile, 'test-image.jpg');
            
            expect(response.status).toBe(403);
            expect(response.body.error.text).toBe('Image upload failed');
        });
    });

    describe('POST /report/property-bulk-image-upload/:prop_id', () => {
        it('should upload a valid bulk image and return image props', async () => {
            const propId = 'bulk-item-1';
            const testFile = Buffer.from('this is another test image');
            const testFileName = 'bulk-item-1.png';
            const url = `/report/property-bulk-image-upload/${propId}`;

            const response = await agent
                .post(url)
                .field('itemQuantity', '1')
                .attach('image', testFile, testFileName);
            
            expect(response.status).toBe(200);
            expect(response.body.originalFilename).toBe(testFileName);
            expect(response.body.uploadedFilename).toEqual(expect.any(String));
        });
    });
});