using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.FormModels
{
    public class BaseEntityForm
    {
        protected BaseEntityForm()
        {
        }
        
        protected BaseEntityForm(BaseEntity entity)
        {
         
        }


        protected void ApplyChanges(BaseEntity entity)
        {

        }
    }
}
