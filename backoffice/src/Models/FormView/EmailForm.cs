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

        public Dictionary<string,dynamic> GetPersonalisation()
            => new (){
                    { "reference", "123"},
                    { "custom message", Subject},
                    { "hazardous find", "no"},
                    { "mmo", "no"},
                    { "wreck add info", "no"},
                    { "item add info", "no"},
                    {"find add info", "no"},
                    { "rip", "no"},
                    {"rip no recover", "no"},
                    { "archaeological protocol", "no"},
                    { "more info", "no"}
            };
    }
}
