describe('Smoke test - sign in and sign out', { retries: 0 }, () => {
  const email = Cypress.env('TEST_USER_EMAIL')
  const password = Cypress.env('TEST_USER_PASSWORD')
  const canSignIn = Cypress.env('ENVIRONMENT') === 'development' && email && password

  it('signs in from the check report status page, then signs out', function () {
    if (!canSignIn) {
      this.skip()
    }

    cy.signIn(email, password)

    cy.visit('/portal/dashboard')
    cy.contains('h1', 'Your reports of wreck material').should('be.visible')

    cy.intercept('GET', '/portal/start').as('signedOut')
    cy.contains('a', 'Logout').click()

    cy.wait('@signedOut', { timeout: 30000 })
    cy.url().should('include', '/portal/start')
    cy.get('#signIn').should('be.visible')

    cy.request({ url: '/portal/dashboard', followRedirect: false }).then((response) => {
      expect(response.status).to.eq(302)
      expect(response.headers.location).to.eq('/error')
    })
  })
})
