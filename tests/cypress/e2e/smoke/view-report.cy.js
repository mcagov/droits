describe('Smoke test - view a report', { retries: 0 }, () => {
  const email = Cypress.env('TEST_USER_EMAIL')
  const password = Cypress.env('TEST_USER_PASSWORD')
  const canSignIn = Cypress.env('ENVIRONMENT') === 'development' && email && password

  it('opens a report from the dashboard and shows its overview', function () {
    if (!canSignIn) {
      this.skip()
    }

    cy.signIn(email, password)

    cy.visit('/portal/dashboard')
    cy.contains('h1', 'Your reports of wreck material').should('be.visible')

    cy.get('[data-js="report-listings-row"]').first().within(() => {
      cy.get('td').first().invoke('text').then((text) => {
        const reference = text.replace('Reference:', '').trim()
        expect(reference).to.not.be.empty
        cy.wrap(reference).as('reference')
      })
      cy.contains('a', 'View report').click()
    })

    cy.url().should('include', '/portal/report/')
    cy.contains('h1', 'Report overview').should('be.visible')

    cy.get('@reference').then((reference) => {
      cy.contains('.govuk-summary-list__row', 'Reference').should('contain.text', reference)
    })
    cy.contains('.govuk-summary-list__row', 'Report status').find('.govuk-tag').should('not.be.empty')
    cy.contains('h2', 'Details of find').should('be.visible')
    cy.get('body').should('not.contain.text', 'Invalid Date')
  })
})
