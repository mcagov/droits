using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class SalvorView
{
    public SalvorView()
        {
        }
    
        public SalvorView(Salvor salvor)
        {
            Id = salvor.Id;
            Email = salvor.Email;
            Name = salvor.Name;
            TelephoneNumber = salvor.TelephoneNumber;
            DateOfBirth = salvor.DateOfBirth;
            Created = salvor.Created;
            LastModified = salvor.LastModified;
            AddressPostcode = salvor.Address.Postcode;
            
        }
        
        public Guid Id { get; }
        public string Email { get; }
        public string Name { get; } = string.Empty;
        public string TelephoneNumber { get; } = string.Empty;
        // public AddressDetails Address { get; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; }
    
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; }
    
        [DisplayName("Last Modified")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastModified { get; }
        [DisplayName("Postcode")]
        public string AddressPostcode { get; }
}

