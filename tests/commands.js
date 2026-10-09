Cypress.Commands.add('clickContinue', () => {
    cy.get('.govuk-button').click()
})

Cypress.Commands.add('signIn', (email, password) => {
    cy.session(email, () => {
        cy.request({ url: '/login/?p=B2C_1_login', followRedirect: false }).then((response) => {
            const b2cOrigin = new URL(response.headers.location).origin

            cy.intercept('GET', '/portal/dashboard').as('dashboard')
            cy.visit('/portal/start')
            cy.contains('h1', 'Check the status of wreck material you have reported').should('be.visible')
            cy.get('#signIn').click()

            cy.origin(b2cOrigin, { args: { email, password } }, ({ email, password }) => {
                cy.get('#signInName, #email').type(email, { log: false })
                cy.get('#password').type(password, { log: false })
                cy.get('#next').click()
            })
        })

        cy.wait('@dashboard', { timeout: 30000 })
        cy.url().should('include', '/portal/dashboard')
    }, {
        validate() {
            cy.request({ url: '/portal/dashboard', followRedirect: false }).its('status').should('eq', 200)
        }
    })
})
