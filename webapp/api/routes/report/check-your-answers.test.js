import request from 'supertest';
import fs from 'fs';
import axios from 'axios';
import app from '../../../server';

const apiEndpoint = 'http://api-endpoint';
const expectedBase64ImageData = 'base64encodeddata';

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

afterEach(() => {
  jest.clearAllMocks();
  // Remove contents but leave the uploads/ directory in place — a parallel worker
  // (e.g. property-bulk.test.js) may be relying on the directory existing
  if (fs.existsSync('uploads')) {
    for (const entry of fs.readdirSync('uploads')) {
      fs.rmSync(`uploads/${entry}`, { recursive: true, force: true });
    }
  }
});

const setup = (expectedDroitReference, expectedDroitId, expectedBase64ImageData, apiEndpoint) => {
  jest.clearAllMocks();
  axios.post.mockResolvedValue({
    status: 200,
    data: {
      reference: expectedDroitReference,
      droitId: expectedDroitId
    },
  });
  fs.promises.readFile.mockResolvedValue(expectedBase64ImageData);
  process.env.API_ENDPOINT = apiEndpoint
}

const populateSession = async (agent) => {
  await agent.post('/report/removed-property-check-answer').send({'removed-property': 'yes'});
}

const assertSessionExists = async (agent) => {
  // Check the session exists. report-date is set so /report/check-your-answers should not redirect to the start page
  const after = await agent.get('/report/check-your-answers');
  expect(after.status).not.toBe(302);
}
const assertSessionCleared = async (agent) => {
  // Session was cleared, so report-date is {} and /report/check-your-answers should redirect to the start page
  const afterClear = await agent.get('/report/check-your-answers');
  expect(afterClear.status).toBe(302);
  expect(afterClear.headers.location).toBe('/report/start');
}

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
  const expectedDroitReference = 'DRT/2024/001';
  const expectedDroitId = 42;

  beforeEach(() => {
    setup(expectedDroitReference, expectedDroitId, expectedBase64ImageData, apiEndpoint);
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

describe('POST /report/confirmation — image file reading', () => {
  const expectedDroitReference = 'DRT/2024/002';
  const expectedDroitId = 43;

  beforeEach(() => {
    setup(expectedDroitReference, expectedDroitId, expectedBase64ImageData, apiEndpoint);
  });

  it('reads the image file and replaces the filename with base64 data', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    fs.promises.readFile.mockResolvedValue('base64encodeddata');

    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    const wreckMaterial = axios.post.mock.calls[1][1];
    expect(wreckMaterial.image).toEqual({ filename: 'test.jpg', data: 'base64encodeddata' });
  });

  it('coerces an empty value string to null', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    await agent.post('/report/property-form-image/i0').type('form').send({
      'property[i0][description]': 'Anchor',
      'property[i0][quantity]': '1',
      'property[i0][value]': '',
      'value-known': 'no',
    });
    fs.promises.readFile.mockResolvedValue('base64encodeddata');

    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    const wreckMaterial = axios.post.mock.calls[1][1];
    expect(wreckMaterial.value).toBeNull();
  });

  it('still submits the wreck material when the image file cannot be read', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    fs.promises.readFile.mockRejectedValue(new Error('ENOENT: file not found'));

    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    // SubmitWreckMaterial is still called even though readFile failed
    const wreckMaterialCall = axios.post.mock.calls[1];
    expect(wreckMaterialCall).toBeDefined();
    // image is left as the original filename string, not replaced with an object
    expect(typeof wreckMaterialCall[1].image).toBe('string');
  });
});

describe('POST /report/confirmation — date formatting', () => {
  const expectedDroitReference = 'DRT/2024/003';
  const expectedDroitId = 44;

  beforeEach(() => {
    setup(expectedDroitReference, expectedDroitId, expectedBase64ImageData, apiEndpoint);
  });

  const postWithFindDate = async (agent, { day, month, year }) => {
    await uploadTestImage(agent);
    await agent.post('/report/find-date-answer').type('form').send({
      'wreck-find-date-day': day,
      'wreck-find-date-month': month,
      'wreck-find-date-year': year,
    });
    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });
    return axios.post.mock.calls[0][1];
  };

  it('zero-pads single-digit month and day in wreck-find-date', async () => {
    const agent = request.agent(app);
    const data = await postWithFindDate(agent, { day: '5', month: '3', year: '2024' });
    expect(data['wreck-find-date']).toBe('2024-03-05');
  });

  it('does not modify double-digit month and day in wreck-find-date', async () => {
    const agent = request.agent(app);
    const data = await postWithFindDate(agent, { day: '15', month: '11', year: '2024' });
    expect(data['wreck-find-date']).toBe('2024-11-15');
  });

  it('assembles report-date as a YYYY-MM-DD string from session values', async () => {
    const agent = request.agent(app);
    // report-date in the session will be set to today
    await populateSession(agent);
    const data = await postWithFindDate(agent, { day: '5', month: '3', year: '2024' });
    const now = new Date();
    const expected = now.toISOString().slice(0, 10);
    expect(data['report-date']).toBe(expected);
  });
});

describe('POST /report/confirmation — API submissions', () => {
  const expectedDroitReference = 'DRT/2024/004';
  const expectedDroitId = 45;
  
  beforeEach(() => {
    setup(expectedDroitReference, expectedDroitId, expectedBase64ImageData, apiEndpoint);
  });

  afterEach(() => {
    jest.resetModules()
  });

  it('POSTs the droit to SubmitDroit, each wreck material to SubmitWreckMaterial, then the droitId to SendConfirmationEmail', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    const urls = axios.post.mock.calls.map((c) => c[0]);
    const apiBaseUrl = `${apiEndpoint}/Api`;
    expect(urls[0]).toBe(`${apiBaseUrl}/SubmitDroit`);
    expect(urls[1]).toBe(`${apiBaseUrl}/SubmitWreckMaterial`);
    expect(urls[2]).toBe(`${apiBaseUrl}/SendConfirmationEmail`);
    // SendConfirmationEmail receives the droitId from the SubmitDroit response
    expect(axios.post.mock.calls[2][1]).toBe(expectedDroitId);
  });

  it('names the wreck material as "<reference>-01" and includes droitId and append flag', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    const wreckMaterial = axios.post.mock.calls[1][1];
    expect(wreckMaterial.name).toBe(`${expectedDroitReference}-01`);
    expect(wreckMaterial['droit-id']).toBe(expectedDroitId);
    expect(wreckMaterial['append-to-original-submission']).toBe(true);
  });

  it('sets append-to-original-submission to false when there are more than 5 wreck materials', async () => {
    const agent = request.agent(app);
    for (let i = 0; i < 6; i++) {
      await agent
        .post(`/report/property-form-image-upload/i${i}`)
        .attach('image', Buffer.from('fake image data'), `test${i}.jpg`);
    }

    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    // First SubmitWreckMaterial call (index 1) — all 6 should have the flag false
    const wreckMaterialCalls = axios.post.mock.calls.filter((c) =>
      c[0].includes('/Api/SubmitWreckMaterial')
    );
    expect(wreckMaterialCalls).toHaveLength(6);
    wreckMaterialCalls.forEach((call) => {
      expect(call[1]['append-to-original-submission']).toBe(false);
    });
  });

  it('renders the confirmation page with the reference on success', async () => {
    const agent = request.agent(app);
    await uploadTestImage(agent);
    const res = await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    expect(res.status).toBe(200);
    expect(res.text).toContain(expectedDroitReference);
  });

  it('redirects to /error when SubmitDroit returns a non-200 status', async () => {
    axios.post.mockResolvedValueOnce({ status: 500, data: {} });

    const agent = request.agent(app);
    await uploadTestImage(agent);
    const res = await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    expect(res.status).toBe(302);
    expect(res.headers.location).toBe('/error');
    expect(axios.post).toHaveBeenCalledTimes(1);
  });

  it('clears the session data after a successful submission', async () => {
    const agent = request.agent(app);
    await populateSession(agent);
    await uploadTestImage(agent);

    await assertSessionExists(agent);
    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    // The session is cleared inside the POST /report/confirmation handler, which sets req.session.data = {} after a successful submission.
    await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });
    await assertSessionCleared(agent);
  });

  it('logs an error when SubmitDroit throws a network error', async () => {
    const errorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});

    axios.post.mockRejectedValueOnce({
      code: 'ENOTFOUND',
      config: { method: 'post', url: 'http://backoffice:5000/Api/SubmitDroit' },
    });

    const agent = request.agent(app);
    await uploadTestImage(agent);
    // The route catches the network error and falls through without sending a response,
    // so the request will hang — abort it with a short timeout.
    await agent
      .post('/report/confirmation')
      .send({ 'property-declaration': 'on' })
      .timeout(500)
      .catch(() => {});

    expect(axios.post).toHaveBeenCalledTimes(1);
    expect(errorSpy).toHaveBeenCalledWith(
      expect.stringContaining('Issue submitting droit report: ENOTFOUND')
    );
    errorSpy.mockRestore();
  });

  it('renders the confirmation page and displays an error when SubmitWreckMaterial returns a non-200 status', async () => {
    const errorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});
    axios.post
      .mockResolvedValueOnce({ status: 200, data: { reference: expectedDroitReference, droitId: expectedDroitId } })
      .mockResolvedValueOnce({ status: 500, data: {} });

    const agent = request.agent(app);
    await uploadTestImage(agent);
    const res = await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    expect(res.status).toBe(200);
    expect(res.text).toContain(expectedDroitReference);
    expect(errorSpy).toHaveBeenCalledWith(
      expect.stringContaining(`Posting Wreck Material to API failed for droitId ${expectedDroitId}! - 500`)
    );
    errorSpy.mockRestore();
  });

  it('renders the confirmation page and displays an error when SubmitWreckMaterial throws', async () => {
    const errorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});
    axios.post
      .mockResolvedValueOnce({ status: 200, data: { reference: expectedDroitReference, droitId: expectedDroitId } })
      .mockRejectedValueOnce({ status: 503 });

    const agent = request.agent(app);
    await uploadTestImage(agent);
    const res = await agent.post('/report/confirmation').send({ 'property-declaration': 'on' });

    expect(res.status).toBe(200);
    expect(res.text).toContain(expectedDroitReference);
    expect(errorSpy).toHaveBeenCalledWith(
      expect.stringContaining('Error posting wreck material to API')
    );
    errorSpy.mockRestore();
  });

  it('does not clear the session when SendConfirmationEmail throws', async () => {
    const errorSpy = jest.spyOn(console, 'error').mockImplementation(() => {});
    axios.post
      .mockResolvedValueOnce({ status: 200, data: { reference: expectedDroitReference, droitId: expectedDroitId } })
      .mockResolvedValueOnce({ status: 200 })
      .mockRejectedValueOnce({
        code: 'ENOTFOUND',
        config: { method: 'post', url: 'http://backoffice:5000/Api/SendConfirmationEmail' },
      });

    const agent = request.agent(app);
    await populateSession(agent);
    await uploadTestImage(agent);

    // The handler falls into the outer catch without sending a response, so the request will hang — abort it
    await agent
      .post('/report/confirmation')
      .send({ 'property-declaration': 'on' })
      .timeout(500)
      .catch(() => {});

    expect(errorSpy).toHaveBeenCalledWith(
      expect.stringContaining('Issue submitting droit report: ENOTFOUND')
    );
    await assertSessionExists(agent);

    errorSpy.mockRestore();
  });

});
