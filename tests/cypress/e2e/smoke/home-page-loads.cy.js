describe('Smoke test - home page', () => {
  const devOrStaging = ['development', 'staging'].includes(Cypress.env('ENVIRONMENT'))

  it('loads the home page and links through to the report start page', function () {
    if (!devOrStaging) {
      this.skip()
    }

    cy.visit('/')
    cy.contains('h1', 'Report Wreck Material').should('be.visible')

    cy.contains('a', 'Report wreck material').click()

    cy.url().should('include', '/report/start')
    cy.contains('.govuk-button', 'Start now').should('be.visible')
  })
})
