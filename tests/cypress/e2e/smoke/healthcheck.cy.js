describe('Smoke test - healthcheck', () => {
  it('returns ok from the web app healthcheck endpoint', () => {
    cy.request('/health').then((response) => {
      expect(response.status).to.eq(200)
      expect(response.body).to.have.property('status', 'ok')
      expect(response.body).to.have.property('uptime')
      expect(response.body).to.have.property('timestamp')
    })
  })
})
