using System.ComponentModel.DataAnnotations;

namespace Droits.Models
{
    public class EmailForm
    {
        public EmailForm(){}
        public string? EmailAddress{get;set;}
        public string? Subject{get;set;}

        [DataType(DataType.MultilineText)]
        public string? Body{get;set;}
    }
}
