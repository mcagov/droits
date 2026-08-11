describe('Smoke test - healthcheck', () => {
  it('returns OK from the web app healthcheck endpoint', () => {
    cy.request('/health').then((response) => {
      expect(response.status).to.eq(200)
      expect(response.body).to.eq('OK')
    })
  })
})
