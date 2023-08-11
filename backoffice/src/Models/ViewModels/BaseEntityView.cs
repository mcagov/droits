using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels
{
    public class BaseEntityView
    {
        protected BaseEntityView()
        {
        }

        protected BaseEntityView(BaseEntity entity)
        {
            Created = entity.Created;
            LastModified = entity.LastModified;
            LastModifiedBy = entity.LastModifiedByUser?.Name ?? "Unknown";
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; }

        [DisplayName("Last Modified")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastModified { get; }

        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; } = string.Empty;
    }
}