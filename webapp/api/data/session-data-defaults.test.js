import sessionDataDefaults from "./session-data-defaults"

describe("session-data-defaults", () => {
  it("should return an object with expected defaults", () => {
    const defaults = sessionDataDefaults()

    // Check expected keys are present, uses deep-equality for expected objects
    expect(defaults).toHaveProperty("report-date", {})
    expect(defaults).toHaveProperty("wreck-find-date", {
      day: '',
      month: '',
      year: ''
    })
    expect(defaults).toHaveProperty("personal", {
      'full-name': '',
      email: '',
      'telephone-number': '',
      'address-line-1': '',
      'address-line-2': '',
      'address-town': '',
      'address-county': '',
      'address-postcode': ''
    })
    expect(defaults).toHaveProperty("location", {
      'location-standard': {},
      'location-given': {},
      'text-location': '',
      'location-description': ''
    })
    expect(defaults).toHaveProperty("property", {})
    expect(defaults).toHaveProperty("vessel-information", {
      'vessel-name': '',
      'vessel-construction-year': '',
      'vessel-sunk-year': ''
    })
    expect(defaults).toHaveProperty("vessel-depth", null)
    expect(defaults).toHaveProperty("wreck-description", "")
    expect(defaults).toHaveProperty("salvage-services", "")
    expect(defaults).toHaveProperty("submittedFiles", [])
  })
})