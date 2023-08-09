using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.FormModels
{
    public class BaseEntityForm : FormModel
    {
        protected BaseEntityForm()
        {
        }


        protected BaseEntityForm(BaseEntity entity)
        {
            Created = entity.Created;
            LastModified = entity.LastModified;
        }
        
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        protected DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        protected DateTime LastModified { get; set; }


        protected virtual void ApplyChanges(BaseEntity entity)
        {
            entity.Created = Created;
            entity.LastModified = LastModified;
        }
    }
}
