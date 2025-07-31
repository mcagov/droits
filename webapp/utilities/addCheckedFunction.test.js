import { addCheckedFunction } from './addCheckedFunction';

describe('addCheckedFunction', () => {
  let mockEnv;
  let checkedFunction;
  // create test environment
  beforeEach(() => {
    mockEnv = {
      addGlobal: jest.fn((name, fn) => {
        if (name === 'checked') {
          checkedFunction = fn;
        }
      })
    };
    
    addCheckedFunction(mockEnv);
  });

  it('registers the checked function', () => {
    expect(mockEnv.addGlobal).toHaveBeenCalledWith('checked', expect.any(Function));
  });

  it('returns empty string when no data', () => {
    const context = { ctx: {} };
    expect(checkedFunction.call(context, 'test', 'value')).toBe('');
  });

  it('returns checked for exact matches', () => {
    const context = {
      ctx: { data: { name: 'John' } }
    };
    
    expect(checkedFunction.call(context, 'name', 'John')).toBe('checked');
    expect(checkedFunction.call(context, 'name', 'Jane')).toBe('');
  });

  it('works with checkboxes arrays', () => {
    const context = {
      ctx: { data: { hobbies: ['reading', 'cycling'] } }
    };
    
    expect(checkedFunction.call(context, 'hobbies', 'reading')).toBe('checked');
    expect(checkedFunction.call(context, 'hobbies', 'swimming')).toBe('');
  });

  it('handles bracket notation', () => {
    const context = {
      ctx: { data: { personal: { age: 25 } } }
    };
    
    expect(checkedFunction.call(context, "['personal']['age']", 25)).toBe('checked');
  });

  it('handles nested brackets', () => {
    const context = {
      ctx: { 
        data: { 
          address: { 
            uk: { postcode: 'BB5 555' } 
          } 
        } 
      }
    };
    
    expect(checkedFunction.call(context, "['address']['uk']['postcode']", 'BB5 555')).toBe('checked');
  });

  it('returns empty for missing fields', () => {
    const context = {
      ctx: { data: { name: 'John' } }
    };
    
    expect(checkedFunction.call(context, 'missing', 'value')).toBe('');
  });

  it('handles different types', () => {
    const context = {
      ctx: { 
        data: { 
          count: 5,
          active: true,
          empty: null
        } 
      }
    };
    
    expect(checkedFunction.call(context, 'count', 5)).toBe('checked');
    expect(checkedFunction.call(context, 'active', true)).toBe('checked');
    expect(checkedFunction.call(context, 'empty', null)).toBe('checked');
  });
});
