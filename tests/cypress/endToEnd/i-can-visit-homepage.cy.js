describe('Homepage and Heading', () => {
  it('should display the main heading and the homepage', () => {
    cy.visit('/')
    cy.contains('h1', 'Report Wreck Material').should('be.visible')
  })
})