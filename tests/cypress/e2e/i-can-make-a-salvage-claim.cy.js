describe('I can start a claim', () => {
    it('shows the first question after I click on start now', () => {
        cy.visit('/report/start')
        cy.get('.govuk-button').click()
    })
})

