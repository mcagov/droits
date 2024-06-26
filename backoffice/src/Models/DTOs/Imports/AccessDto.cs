using System.Globalization;
using CsvHelper.Configuration.Attributes;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;

namespace Droits.Models.DTOs.Imports;

public class AccessDto
{
    [Name("Droit Number")]
    public string? DroitNumber { get; set; }

    public string? District { get; set; }

    public string? Aircraft { get; set; }

    [Name("UK Waters")]
    public string? UkWaters { get; set; }

    [Name("Historic/Modern")]
    public string? WreckAge { get; set; }

    [Name("War Wreck")]
    public string? WarWreck { get; set; }

    [Name("Fishing Nets")]
    public string? FishingNets { get; set; }

    [Name("Logged within 48hrs?")]
    public string? Logged48Hours { get; set; }

    [Name("Date Found")]
    public string? DateFound { get; set; }

    [Name("Date Reported")]
    public string? DateReported { get; set; }

    [Name("Date Delivered")]
    public string? DateDelivered { get; set; }

    [Name("Exact Position Found")]
    public string? ExactPositionFound { get; set; }

    public string? Depth { get; set; }

    [Name("Lat (a)")]
    public string? LatitudeA { get; set; }

    [Name("Lat (b)")]
    public string? LatitudeB { get; set; }

    [Name("Lat (c)")]
    public string? LatitudeC { get; set; }

    [Name("Long (a)")]
    public string? LongitudeA { get; set; }

    [Name("Long (b)")]
    public string? LongitudeB { get; set; }

    [Name("Long (c)")]
    public string? LongitudeC { get; set; }

    [Name("Afloat/Ashore/Bumping")]
    public string? RecoveredFrom { get; set; }

    [Name("Where Secured")]
    public string? WhereSecured { get; set; }

    [Name("New Salvor")]
    public string? NewSalvor { get; set; }

    [Name("Salvor Name")]
    public string? SalvorName { get; set; }

    public string? Address { get; set; }

    [Name("Post Code")]
    public string? PostCode { get; set; }

    [Name("Salvage Award Claimed")]
    public string? SalvageAwardClaimed { get; set; }

    [Name("Salvage No")]
    public string? SalvageNo { get; set; }

    public string? Description { get; set; }

    [Name("Description Contd")]
    public string? DescriptionContinued { get; set; }

    [Name("Name of Wreck")]
    public string? WreckName { get; set; }

    [Name("Date of Loss")]
    public string? DateOfLoss { get; set; }

    [Name("Year of Loss")]
    public string? YearOfLoss { get; set; }

    public string? Value { get; set; }

    public string? Owner { get; set; }

    public string? Agent { get; set; }

    [Name("Wreck Construction Details")]
    public string? WreckConstructionDetails { get; set; }

    [Name("Nature of Services")]
    public string? NatureOfServices { get; set; }

    public string? Duration { get; set; }

    [Name("Estimated Cost of Services")]
    public string? EstimatedCostOfServices { get; set; }

    public string? Remarks { get; set; }

    public string? Outcome { get; set; }

    [Name("Salvage Award")]
    public string? SalvageAward { get; set; }

    [Name("Receipts from Owner")]
    public string? ReceiptsFromOwner { get; set; }

    [Name("Date received")]
    public string? DateReceived { get; set; }

    [Name("Receipts from Purchaser")]
    public string? ReceiptsFromPurchaser { get; set; }

    [Name("Date recieved")]
    public string? DateRecieved { get; set; }

    public string? Purchaser { get; set; }

    public string? Museum { get; set; }

    [Name("Discharge of Goods")]
    public string? DischargeOfGoods { get; set; }

    [Name("Closure of Droits")]
    public string? ClosureOfDroits { get; set; }

    [Name("File ref")]
    public string? FileRef { get; set; }


    public string? GetLocationDescription()
    {
        var locationDescription = ExactPositionFound.ValueOrEmpty();

        var coordinates = new[]
        {
            ( "Longitude A", LongitudeA ),
            ( "Longitude B", LongitudeB ),
            ( "Longitude C", LongitudeC ),
            ( "Latitude A", LatitudeA ),
            ( "Latitude B", LatitudeB ),
            ( "Latitude C", LatitudeC )
        };

        foreach ( var (label, value) in coordinates )
        {
            if ( !string.IsNullOrEmpty(value) )
            {
                locationDescription += $"\n{label}: {value}";
            }
        }

        return locationDescription;
    }

    public Address? GetAddress()
    {

        if ( string.IsNullOrEmpty(Address) )
        {
            return new Address();
        }
        
        var address = Address.AsAddress();

        if ( address != null && !string.IsNullOrEmpty(PostCode))
        {
            address.Postcode = PostCode;
        }
       
        return address;
    }
    

    public Address? GetStorageAddress()
    {
        if ( string.IsNullOrEmpty(WhereSecured) )
        {
            return new Address();
        }

        var storageAddress = WhereSecured.AsAddress();
        
        return storageAddress;
    }
    
 
    
    public int? GetDepth()
    {
        if ( string.IsNullOrEmpty(Depth) )
        {
            return null;
        }

        var input = Depth.ToLower();

        input = input.Replace("to", "-").Replace("c.", "");

        var excludeDepthsContaining = new List<string>
            { "fathom", "ft", "feet", "inches", "in", "&", "MSW", "BCD", "MHWS" };

        if ( excludeDepthsContaining.Any(exclude => input.Contains(exclude.ToLower())) )
        {
            return null;
        }

        var cleanedDepth =
            new string(input.Where(c => char.IsDigit(c) || c == '-' || c == '.').ToArray());

        if ( string.IsNullOrEmpty(cleanedDepth) )
        {
            return null;
        }
        
        if ( !cleanedDepth.Contains('-') )
        {
            double.TryParse(cleanedDepth, out var depth);
            return depth > 0d ? ( int )Math.Round(depth) : null;
        }

        if ( !cleanedDepth.Contains('-') || cleanedDepth.StartsWith('-') )
        {
            cleanedDepth = cleanedDepth.Replace("-", "");
            double.TryParse(cleanedDepth, out var depth);
            return depth > 0d ? ( int )Math.Round(depth) : null;
        }

        var depthArray = cleanedDepth.Split('-');
        try
        {
            var depths = Array.ConvertAll(depthArray, double.Parse);
            var averageDepth = depths.Average();
            return ( int )Math.Round(averageDepth);
        }
        catch ( Exception )
        {
            return null;
        }
    }
}