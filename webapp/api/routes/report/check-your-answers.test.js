import request from 'supertest';
import app from '../../../server';

describe('GET /report/check-your-answers', () => {
  it('redirects to /report/start when report-date is empty', async () => {
    const agent = request.agent(app);
    const res = await agent.get('/report/check-your-answers');
    expect(res.status).toBe(302);
    expect(res.headers.location).toBe('/report/start');
  });

  it('does not redirect to /report/start when report-date is populated', async () => {
    const agent = request.agent(app);
    // POST to removed-property-check-answer sets report-date in session
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

    // Add a property item to session that has no image
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
