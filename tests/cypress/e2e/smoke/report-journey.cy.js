describe('Smoke test - report a wreck journey', () => {
  const submittableEnvironments = ['development', 'staging']
  const canSubmit = submittableEnvironments.includes(Cypress.env('ENVIRONMENT'))

  it('completes a report through to the declaration', () => {
    cy.visit('/report/start')
    cy.contains('.govuk-button', 'Start now').click()

    cy.get('#removed-property').check() // Yes
    cy.clickContinue()

    cy.get('#wreck-find-date-day').clear().type('15')
    cy.get('#wreck-find-date-month').clear().type('08')
    cy.get('#wreck-find-date-year').clear().type('2025')
    cy.clickContinue()

    const stamp = new Date().toISOString().slice(0, 10)
    cy.get('#full-name').clear().type('ROW Smoke Test')
    cy.get('#email').clear().type(`row.smoke.test+${stamp}@example.com`)
    cy.get('#address-line-1').clear().type('48 Street')
    cy.get('#address-town').clear().type('London')
    cy.get('#address-county').clear().type('Surrey')
    cy.get('#address-postcode').clear().type('E17 2MD')
    cy.clickContinue()

    cy.get('#known-wreck-2').check() // No
    cy.clickContinue()

    // Select "Sea shore" as salvage location
    cy.get('#removed-from-4').check()
    cy.clickContinue()

    // Select location method: Decimal degrees
    cy.get('#location-type').check()
    // Enter coordinates
    cy.get('#location-latitude-decimal').type('50.8225')
    cy.get('#location-longitude-decimal').type('-0.1372')
    cy.clickContinue()

    cy.contains('Add your first item').click()

    cy.get('#property\\.i0\\.description').clear().type('Ship timber')
    cy.get('#property\\.i0\\.quantity').clear().type('1')
    cy.get('#value-known-2').check()
    cy.clickContinue()

    cy.get('#property-image').click()
    cy.get('input[type="file"]').selectFile('cypress/fixtures/test.jpeg', { force: true })
    cy.get('.photo-upload__button').click()
    cy.get('.govuk-button--continue').click()

    //  where the wreck is stored
    cy.get('#property-i0-storage-address').check()
    cy.clickContinue()

    // shows declaration page and asks if I wish to claim award
    cy.get('#propertyDeclaration').check()
    cy.get('.form > .govuk-button').click()

    cy.get('#claim-salvage-2').check()
    cy.get('.govuk-button').click()

    cy.url().should('include', '/report/check-your-answers')
    cy.contains('.govuk-button', 'Accept and send').should('be.visible')

    if (!canSubmit) {
      return
    }

    cy.get('#property-declaration').check()
    cy.contains('.govuk-button', 'Accept and send').click()

    cy.contains('.govuk-panel__title', 'Report submitted', { timeout: 60000 }).should('be.visible')
    cy.get('.govuk-panel__body strong').invoke('text').should('not.be.empty')
  })
})
