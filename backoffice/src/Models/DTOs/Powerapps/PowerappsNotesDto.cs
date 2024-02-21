using System.Text.Json.Serialization;
using Droits.Models.Enums;


//https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2/annotations?$select=annotationid,createdon,objecttypecode,_objectid_value,notetext,isdocument,filesize,filename,mimetype,documentbody
namespace Droits.Models.DTOs.Powerapps
{
    public class PowerappsNotesDto
    {
        [JsonPropertyName("value")]
        public List<PowerappsNoteDto>? Value { get; set; }
    }

    public class PowerappsNoteDto
    {
        [JsonPropertyName("annotationid")]
        public string? PowerappsAnnotationId { get; set; }

        [JsonPropertyName("modifiedby")]
        public PowerappsUserDto? ModifiedBy { get; set; }
        
        [JsonPropertyName("createdon")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("objecttypecode")]
        public string? LinkedEntityType { get; set; } // crf99_mcawreckreport, 
        
        [JsonPropertyName("_objectid_value")]
        public string? LinkedEntityPowerappsId { get; set; } // crf99_mcawreckreportid etc...
        
        [JsonPropertyName("notetext")]
        public string? NoteText { get; set; }
        
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }


        [JsonPropertyName("isdocument")]
        public bool? IsDocument { get; set; } 

        
        [JsonPropertyName("filesize")]
        public int? FileSize { get; set; }
        
        [JsonPropertyName("filename")]
        public string? FileName { get; set; }

        [JsonPropertyName("mimetype")]
        public string? MimeType { get; set; }

        [JsonPropertyName("documentbody")]
        public string? DocumentBody { get; set; }

        

       
    }

}
