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
  it('register new user', function () {
    cy.visit('http://localhost:3000/portal/start');
    cy.contains('a', 'create an account').click();
    
  });
});

it('shows error when registering with an existing email address', function () {
  const existingEmail = 'denise.turkova@madetech.com';

  cy.visit('http://localhost:3000/portal/start');
  cy.contains('a', 'create an account').click();

  cy.origin('https://testb2cmcga.b2clogin.com', { args: { existingEmail } }, ({ existingEmail }) => {
    cy.get('#email').should('be.visible').type(existingEmail);
    cy.get('button[type="submit"]').click();
    cy.get('#newPassword').should('be.visible').type('TestPass123!');
    cy.get('#confirmPassword').should('be.visible').type('TestPass123!');
    cy.get('button[type="submit"]').click();
    cy.contains('user with the specified ID already exists', { timeout: 10000 }).should('be.visible');
  });
});



  it('shows error when registering without reporting a shipwreck', function () {
  cy.visit('http://localhost:3000/portal/start');
  cy.contains('a', 'create an account').click();
  cy.get('input[name="email"]').type('denise.turkova@gmail.com');
  cy.get('form').submit();
  cy.contains('Claim not found').should('be.visible');
});
