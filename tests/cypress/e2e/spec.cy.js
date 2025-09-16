describe('template spec', () => {
  it('passes', () => {
    cy.visit('/')
  });
});
  
describe('Homepage', () => {
  it('should display the main heading', () => {
    cy.visit('/')
    cy.contains('h1', 'Report Wreck Material').should('be.visible')
  });
});

describe( "after shipwreck report, I can register", () => {
  it('register a new user', function () {
    cy.visit('http://localhost:3000/portal/start');
    cy.contains('a', 'create an account').click();
  });
});

