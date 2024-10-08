import sessionDataDefaults from "./session-data-defaults"

// recursively check whether two arguments share any references
// note: will infinite loop if a has any circular references
const haveAnySharedReferences = (a, b) => {
  // ignore null values, the typeof these are objects
  // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/typeof#typeof_null
  if (a === null) {
    return false
  } else if (typeof a !== "object") {
    return a === b
  } else {
    return Object.keys(a).every((property) => {
      haveAnySharedReferences(a[property], b[property])
    })
  }
}

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

  it("shouldn't return information saved to the defaults object to a later call", () => {
    const defaults1 = sessionDataDefaults()
    defaults1.personal["full-name"] = "Alice"
    const defaults2 = sessionDataDefaults()
    expect(defaults2.personal["full-name"]).toBe("")
  })

  it("should return a different object with different nested objects each time it is called", () => {
    const defaults1 = sessionDataDefaults()
    const defaults2 = sessionDataDefaults()

    expect(defaults1 === defaults2).toBeFalsy()
    
    expect(haveAnySharedReferences(defaults1, defaults2)).toBeFalsy()
  })
})
