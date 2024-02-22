namespace Droits.Models.DTOs.Imports;

public class TriageUploadResultDto
{
    public List<KeyValuePair<string, string?>> InvalidDroitReferences = new();
    public List<KeyValuePair<string, string?>> InvalidTriageNumberValues = new();
    public List<KeyValuePair<string, string?>> SuccessfulTriageUpdates = new();


    public bool HasError() => InvalidDroitReferences.Any() || InvalidTriageNumberValues.Any();
   
    public string GetErrorMessage()
    {
        var errorMessage = "";
        if ( InvalidDroitReferences.Count > 0 )
        {
            errorMessage +=
                $"Invalid Droit references: {string.Join(",", InvalidDroitReferences.Select((k,v) => k))} \n";
        }

        if ( InvalidTriageNumberValues.Count > 0 )
        {
            errorMessage +=
                $"Invalid Triage numbers for droits: {string.Join(",", InvalidTriageNumberValues.Select((k,v) => k))}";
        }

        return errorMessage;
    }
    
     public string GetSuccessMessage()
     {
         return $"Successfully updated {SuccessfulTriageUpdates.Count} Droits : {string.Join("\n" , SuccessfulTriageUpdates.Select((k,v) => k))}";
     }
}