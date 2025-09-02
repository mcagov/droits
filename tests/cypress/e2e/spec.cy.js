describe('template spec', () => {
  it('passes', () => {
    cy.visit('/')
  })
})

describe('Homepage', () => {
  it('should display the main heading', () => {
    cy.visit('/')
    cy.contains('h1', 'Report Wreck Material').should('be.visible')
  })
})