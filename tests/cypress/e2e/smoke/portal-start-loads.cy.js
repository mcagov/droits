describe('Smoke test - portal start page', () => {
  it('loads the check report status page with a Sign in button', () => {
    cy.visit('/portal/start')

    cy.contains('h1', 'Check the status of wreck material you have reported').should('be.visible')

    cy.get('#signIn')
      .should('be.visible')
      .and('contain.text', 'Sign in')
  })
})
