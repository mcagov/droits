
#region

using Droits.Models.Entities;

#endregion

namespace Droits.Models.FormModels
{
    public class BaseEntityForm
    {
        protected BaseEntityForm()
        {
        }
        

        protected BaseEntityForm(BaseEntity entity)
        {
            Id = entity.Id;

        }

        public Guid Id { get; init; }

        protected void ApplyChanges(BaseEntity entity)
        {
            entity.Id = Id;
        }
    }
}
