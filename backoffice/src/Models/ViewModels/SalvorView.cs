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
            Created = salvor.Created;
            LastModified = salvor.LastModified;
            
        }
        
        public Guid Id { get; }
        public string Email { get; }
        public string Name { get; } = string.Empty;
    
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; }
    
        [DisplayName("Last Modified")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastModified { get; }
}

