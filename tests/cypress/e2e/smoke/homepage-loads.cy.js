describe('Smoke test', () => {
  it('loads the report start page', () => {
    cy.visit('/report/start')
    cy.get('.govuk-button').should('be.visible')
  })

  it('loads the first report question page', () => {
    cy.visit('/report/removed-property-check')
    cy.contains('h1', 'Have you removed the wreck material').should('be.visible')
    cy.get('#removed-property').should('exist')
    cy.get('#removed-property-2').should('exist')
  })
})
