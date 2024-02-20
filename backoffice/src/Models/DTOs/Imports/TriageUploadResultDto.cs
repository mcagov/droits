namespace Droits.Models;

public class TriageUploadResultDto
{
    public Dictionary<string, string?> InvalidDroitReferences = new();
    public Dictionary<string, string?> InvalidTriageNumberValues = new();
    public Dictionary<string, string?> SuccessfulTriageUpdates = new();


    public bool HasError() => InvalidDroitReferences.Any() || InvalidTriageNumberValues.Any();
   
    public string GetErrorMessage()
    {
        var errorMessage = "";
        if ( InvalidDroitReferences.Count > 0 )
        {
            errorMessage +=
                $"Invalid Droit references: {string.Join(",", InvalidDroitReferences.Keys)} \n";
        }

        if ( InvalidTriageNumberValues.Count > 0 )
        {
            errorMessage +=
                $"Invalid Triage numbers for droits: {string.Join(",", InvalidTriageNumberValues.Keys)}";
        }

        return errorMessage;
    }
    
     public string GetSuccessMessage()
     {
         return $"Successfully updated {SuccessfulTriageUpdates.Count} Droits : {string.Join("\n" , SuccessfulTriageUpdates.Keys)}";
     }
}