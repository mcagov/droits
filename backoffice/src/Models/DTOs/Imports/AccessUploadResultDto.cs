namespace Droits.Models.DTOs.Imports;

public class AccessUploadResultDto
{

    public AccessUploadResultDto()
    {
        
    }
    
    public AccessUploadResultDto(List<AccessUploadResult> uploadResults)
    {
        UploadResults = uploadResults;
    }

    
    public List<AccessUploadResult> UploadResults { get; set; }= new();

    public List<AccessUploadResult> SuccessfulUploads =>
        UploadResults.Where(r => r.IsSuccess).ToList();
    
    public List<AccessUploadResult> FailedUploads =>
        UploadResults.Where(r => !r.IsSuccess).ToList();
    public bool HasError() => UploadResults.Any(r => !r.IsSuccess);
   
    public string GetErrorMessage()
    {
        return HasError() ? $"{UploadResults.Count(r => r.IsSuccess)} Droits Failed To Upload" : string.Empty;
    }
    
     public string GetSuccessMessage()
     {
         return $"Successfully uploaded {UploadResults.Count(r => r.IsSuccess)} Droits";
     }
}

public class AccessUploadResult
{
    
    public bool IsSuccess { get; set; } = false;
    public bool DuplicateDroitReference { get; set; } = false;
    public string? DroitNumber { get; set; }
    public string? SavedDroitReference { get; set; }
    public Guid? DroitId { get; set; }
    public string? ErrorMessage { get; set; }

}