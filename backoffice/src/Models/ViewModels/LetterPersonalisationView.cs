#region

using System.Text.RegularExpressions;
using Droits.Helpers;
using Droits.Models.Entities;

#endregion

namespace Droits.Models.ViewModels;

public class LetterPersonalisationView
{
    public LetterPersonalisationView(Droit droit)
    {
        Pluralised = droit.WreckMaterials.Count > 1;
        Reference = droit.Reference;
        Date = droit.ReportedDate.ToString("dd/MM/yyyy");
        Wreck = droit.Wreck?.Name ?? "";
        ItemPluralised = Pluralised ? "items" : "item";
        HasPluralised = Pluralised ? "have" : "has";
        ThisPluralised  = Pluralised ? "these" : "this";
        IsPluralised = Pluralised ? "are" : "is";
        PiecePluralised = Pluralised ? "pieces" : "piece";
        Items = String.Join('\n', droit.WreckMaterials
            .Select(material => String.Concat("* ", material.Name)).ToList()) ?? "";
        FullName = droit.Salvor?.Name ?? "";
    }

    private bool Pluralised { get; }
    private string Reference { get; }
    private string Date { get; }
    private string Wreck { get; }
    private string ItemPluralised { get; }
    private string HasPluralised  { get; }
    private string ThisPluralised { get; }
    private string IsPluralised { get; }
    private string PiecePluralised { get; }
    private string Items { get; }
    private string FullName { get; }

    public Dictionary<string, dynamic> GetAsPersonalisation()
    {
        return new Dictionary<string, dynamic>
        {
            { "reference", Reference },
            { "wreck", Wreck },
            { "date", Date },
            { "item pluralised", ItemPluralised },
            { "has pluralised", HasPluralised },
            { "this pluralised", ThisPluralised },
            { "is pluralised", IsPluralised },
            { "piece pluralised", PiecePluralised },
            { "items", Items },
            { "full name", FullName }
        };
    }


    public string SubstituteContent(string content)
    {
        foreach ( var param in GetAsPersonalisation() )
        {
            // Capitalise pluralised word if it's at the start of a sentence.
            if ( param.Key.Contains("pluralised") )
            {
                content = Regex.Replace(content, $@"(?<=[\.\:\?\!]\s|[\.\:\?\!]\)\s|^)\(\({param.Key}\)\)(?=.*[\.\:\?\!])", StringHelper.CapitalizeFirstLetter(param.Value.ToString()), RegexOptions.Multiline);
            }

            content = Regex.Replace(content, $@"\(\({param.Key}\)\)", param.Value.ToString(), RegexOptions.IgnoreCase);
        }

        return content;
    }
}