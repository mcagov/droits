describe('I can start a claim', () => {
    it('shows the first question after I click on start now', () => {
        cy.visit('/report/start')
        cy.get('.govuk-button').click()
    })

    it('shows Yes and No options for property removal', () => {
        cy.visit('/report/removed-property-check')

        cy.get('#removed-property').should('exist') // Yes
        cy.get('#removed-property-2').should('exist') // No
    })

    it('allows selecting only one property removal option at a time', () => {
        cy.visit('/report/removed-property-check')

        cy.get('#removed-property').check()
        cy.get('#removed-property').should('be.checked')
        
        cy.get('#removed-property-2').check()
        cy.get('#removed-property-2').should('be.checked')
        cy.get('#removed-property').should('not.be.checked')
    })

    it('shows a message for no further action needed when "No" is selected', () => {
        cy.visit('/report/removed-property-check')
        
        cy.get('#removed-property-2').check()
        cy.get('.govuk-button').click()
        
        cy.get('h1.govuk-heading-xl')
            .should('contain.text', 'You do not need to report this wreck material')
    })
    
    it('lets me enter a valid wreck find date', () => {
        cy.visit('/report/find-date')

        cy.get('#wreck-find-date-day').clear()
        cy.get('#wreck-find-date-day').type('15')

        cy.get('#wreck-find-date-month').clear()
        cy.get('#wreck-find-date-month').type('08')

        cy.get('#wreck-find-date-year').clear()
        cy.get('#wreck-find-date-year').type('2025')

        cy.get('.govuk-button').click()
    })

    it('lets me enter personal and address details', () => {
        cy.visit('/report/personal')

        cy.get('#full-name').clear()
        cy.get('#full-name').type('Macek Test')

        cy.get('#email').clear()
        cy.get('#email').type('test@example.com')

        cy.get('#address-line-1').clear()
        cy.get('#address-line-1').type('48 Street')

        cy.get('#address-town').clear()
        cy.get('#address-town').type('London')

        cy.get('#address-county').clear()
        cy.get('#address-county').type('Surrey')

        cy.get('#address-postcode').clear()
        cy.get('#address-postcode').type('E17 2MD')

        cy.get('.govuk-button').click()
        
    })

    it('lets me answer known wreck as Yes and continue through vessel information', () => {
        cy.visit('/report/known-wreck')
        
        cy.get('#known-wreck').check()
        cy.get('.govuk-button').click()


        cy.url().should('include', '/report/vessel-information')
        
        cy.get('.govuk-button').click()

        cy.contains('Where was the wreck material found?').should('exist')
    })

    it('lets me answer known wreck as No and skip vessel information', () => {
        cy.visit('/report/known-wreck')

        cy.get('#known-wreck-2').check()
        cy.get('.govuk-button').click()

        cy.contains('Where was the wreck material found?').should('exist')
    })

    it('lets me choose where the wreck material was found and enter a text location', () => {
        cy.visit('/report/salvaged-from')

        // Select "Sea shore"
        cy.get('#removed-from-4').check()

        cy.get('.govuk-button').click()

        // Select location method: "Text description"
        cy.get('#location-type-6').check()

        // Enter a simple location description
        cy.get('#text-location').type('Found near Brighton beach')

        cy.get('.govuk-button').click()
        
    })

    it('lets me enter location using decimal latitude/longitude', () => {
        cy.visit('/report/salvaged-from')

        // Select "Sea shore" as salvage location
        cy.get('#removed-from-4').check()
        cy.get('.govuk-button').click()

        // Select location method: Decimal degrees
        cy.get('#location-type').check()

        // Enter coordinates
        cy.get('#location-latitude-decimal').type('50.8225')
        cy.get('#location-longitude-decimal').type('-0.1372')

        cy.get('.govuk-button').click()
        
    })
    it('lets me click to add a new recovered wreck item', () => {
        cy.visit('/report/property-summary')
        
        cy.contains('Please add recovered wreck material to this report.').should('exist')

        cy.contains('Add your first item').click()

        cy.url().should('include', '/report/property-form/new')
        cy.contains('Describe the item').should('exist')
    })

    it('lets me fill out the wreck item form and submit it', () => {
        cy.visit('/report/property-form/new')

        cy.get('textarea[name="description"]').type('some seashells.')

        cy.get('input[name="quantity"]').type('1')

        cy.get('#known-value').check()
        cy.get('input[name="value"]').type('100')

        cy.get('.govuk-button').click()

        cy.url().should('include', '/report/property-summary')

    })




})
