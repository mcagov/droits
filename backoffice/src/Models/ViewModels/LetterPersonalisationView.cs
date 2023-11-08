using System.Text.RegularExpressions;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class LetterPersonalisationView
{
    public LetterPersonalisationView(Droit droit)
    {
        Reference = droit.Reference;
        Date = droit.ReportedDate.ToString("dd/MM/yyyy");
        Wreck = droit?.Wreck?.Name ?? "No Wreck";
        
    }


    private string Reference { get; }
    private string Date { get; }
    private string Wreck { get; }

    public Dictionary<string, dynamic> GetAsPersonalisation()
    {
        return new Dictionary<string, dynamic>
        {
            { "reference", Reference },
            { "wreck", Wreck },
            { "date", Date }
        };
    }


    public string SubstituteContent(string content)
    {
        foreach ( var param in GetAsPersonalisation() )
        {
            content = Regex.Replace(content, $@"\(\({param.Key}\)\)", param.Value.ToString(), RegexOptions.IgnoreCase);
        }

        return content;
    }
}

//         Known:
//         "reference"
//             
//         "wreck" //wreck name
//         "date" // reported date
//             
//             
//         Unknown:
//         "custom message" // No longer needed as can edit on the fly. 
//         "item pluralised" (item/items? depending on wreck material count?)
//         "has pluralised"? (is/has depending on wreck material count?)
//         "this pluralised" (this/these depending on wreck material count?)
//         "is pluralised" (is/are ?? depending on wreck material count?)
//         "items" //? 
//         "suggested donate" // bool
//         "wreck known" //bool ?
//         "link_to_file" // copy of report submission? 
//         "late report"//bool ? 
//             