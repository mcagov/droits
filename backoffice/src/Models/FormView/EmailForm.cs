using System.ComponentModel.DataAnnotations;

namespace Droits.Models
{
    public class EmailForm
    {
        public EmailForm(){}
        public string EmailAddress{get;set;} = string.Empty;
        public string Subject{get;set;} = string.Empty;

        [DataType(DataType.MultilineText)]
        public string Body{get;set;} = string.Empty;

        public Dictionary<string,dynamic> getPersonalisation()
            => new Dictionary<string,dynamic>(){
                    { "body", Body},
                    { "subject", Subject},
                };
    }
}
