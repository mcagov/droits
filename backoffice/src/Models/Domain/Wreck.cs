namespace Droits.Models
{
    public class Wreck
    {
        public Guid Id{get;set;}

        public WreckStatus Status{get;set;} = WreckStatus.Active;
        public string Name{get;set;} = string.Empty;
        public DateTime DateOfLoss{get;set;}

        public bool IsWarWreck{get;set;} = false;
        public bool IsAnAircraft{get;set;} = false;
        public string? Latitude{get;set;}
        public string? Longitude{get;set;}

        public bool IsProtectedSite{get;set;} = false;
        public string? ProtectionLegislation{get;set;}

        public DateTime Created{get;set;}
        public DateTime Modified{get;set;}


    }

}
