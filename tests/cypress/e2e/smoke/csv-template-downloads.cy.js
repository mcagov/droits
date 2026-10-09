describe('Smoke test - bulk upload CSV template', () => {
  const templatePath = '/assets/downloads/ROW-bulk-upload-template.csv'

  it('links to the CSV template from the report start page', () => {
    cy.visit('/report/start')
    cy.get(`a[href="${templatePath}"]`)
      .should('be.visible')
      .and('contain.text', 'required format')
  })

  it('serves the CSV template for download', () => {
    cy.request(templatePath).then((response) => {
      expect(response.status).to.eq(200)
      expect(response.headers['content-type']).to.include('text/csv')
      expect(response.body).to.not.be.empty
    })
  })
})
