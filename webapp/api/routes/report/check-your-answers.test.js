import request from 'supertest';
import fs from 'fs';
import axios from 'axios';
import app from '../../../server';

jest.mock('axios', () => ({ post: jest.fn() }));
jest.mock('fs', () => {
  const actual = jest.requireActual('fs');
  return {
    ...actual,
    promises: {
      ...actual.promises,
      readFile: jest.fn(),
    },
  };
});

afterAll(() => {
  if (fs.existsSync('uploads')) {
    fs.rmSync('uploads', { recursive: true, force: true });
  }
});

// Uploads a test image so the session has a property item with a string image, which satisfies the allWmContainImages validation check.
const uploadTestImage = async (agent) => {
  await agent
    .post('/report/property-form-image-upload/i0')
    .attach('image', Buffer.from('fake image data'), 'test.jpg');
};

describe('GET /report/check-your-answers', () => {
  it('redirects to /report/start when report-date is empty', async () => {
    const agent = request.agent(app);
    const res = await agent.get('/report/check-your-answers');
    expect(res.status).toBe(302);
    expect(res.headers.location).toBe('/report/start');
  });

  it('does not redirect to /report/start when report-date is populated', async () => {
    const agent = request.agent(app);
    await agent
      .post('/report/removed-property-check-answer')
      .send({ 'removed-property': 'yes' });

    const res = await agent.get('/report/check-your-answers');
    expect(res.headers.location).not.toBe('/report/start');
  });
});

describe('POST /report/confirmation — form validation', () => {
  it('renders errors when property-declaration is missing', async () => {
    const agent = request.agent(app);
    const res = await agent.post('/report/confirmation').send({});

    expect(res.status).toBe(200);
    expect(res.text).toContain('Select to confirm you are happy with the declaration');
  });

  it('renders errors when a wreck material item has no image', async () => {
    const agent = request.agent(app);

    await agent
      .post('/report/property-form-image/i0')
      .type('form')
      .send({
        'property[i0][description]': 'Anchor',
        'property[i0][quantity]': '1',
        'property[i0][value]': '',
        'value-known': 'no',
      });

    const res = await agent
      .post('/report/confirmation')
      .send({ 'property-declaration': 'on' });

    expect(res.status).toBe(200);
    expect(res.text).toContain('An image is required for each wreck material');
  });
});

describe('POST /report/confirmation — location formatting', () => {
  beforeEach(() => {
    jest.clearAllMocks();
    axios.post.mockResolvedValue({
      status: 200,
      data: { reference: 'DRT/2024/001', droitId: 42 },
    });
    fs.promises.readFile.mockResolvedValue('fakebase64data');
  });

  const postWithLocation = async (textLocation, description) => {
    const agent = request.agent(app);
    await uploadTestImage(agent);

    if (textLocation) {
      await agent.post('/report/location-answer').type('form').send({
        'location-type': 'text-location',
        'text-location': textLocation,
        'location-description': description || '',
      });
    } else {
      // coords-decimal sets location-description without touching text-location
      await agent.post('/report/location-answer').type('form').send({
        'location-type': 'coords-decimal',
        'location-latitude-decimal': '50',
        'location-longitude-decimal': '-1',
        'location-description': description || '',
      });
    }

    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });
    return axios.post.mock.calls[0][1];
  };

  it('uses location-description alone when text-location is empty', async () => {
    const data = await postWithLocation('', 'Near the harbour');
    expect(data['location-description']).toBe('Near the harbour');
  });

  it('appends a period to text-location when it does not end with one', async () => {
    const data = await postWithLocation('Near the rocks', 'extra details');
    expect(data['location-description']).toBe('Near the rocks. extra details');
  });

  it('does not add a second period when text-location already ends with one', async () => {
    const data = await postWithLocation('Near the rocks.', 'extra details');
    expect(data['location-description']).toBe('Near the rocks. extra details');
  });

  it('trims trailing whitespace from text-location before formatting', async () => {
    const data = await postWithLocation('Near the rocks  ', 'extra details');
    expect(data['location-description']).toBe('Near the rocks. extra details');
  });
});
