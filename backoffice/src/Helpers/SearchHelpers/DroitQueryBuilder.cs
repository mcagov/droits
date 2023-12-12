using System.Linq.Expressions;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Helpers.SearchHelpers;

public static class DroitQueryBuilder
{
    
    private const int MaxLevenshteinDistance = 5;
    
    public static IQueryable<Droit> BuildQuery(DroitSearchForm form, IQueryable<Droit> query,  bool usePsql = true)
    {
            
            if (!string.IsNullOrEmpty(form.Reference))
            {
                query = query.Where(d =>
                 !string.IsNullOrEmpty(d.Reference) &&
                 (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.Reference.ToLower(), d.Reference.ToLower()) : 
                     SearchHelper.GetLevenshteinDistance(form.Reference.ToLower(), d.Reference.ToLower())) < MaxLevenshteinDistance ||
                 d.Reference.ToLower().Contains(form.Reference.ToLower())
                );
            }
            
            if (form.CreatedFrom != null )
            {
                query = query.Where(d => d.Created >= form.CreatedFrom);
            }
            
            if (form.CreatedTo != null)
            {
                query = query.Where(d => d.Created <= form.CreatedTo);
            }
            
            if (form.LastModifiedFrom != null)
            {
                query = query.Where(d => d.LastModified >= form.LastModifiedFrom);
            }
            
            if (form.LastModifiedTo != null)
            {
                query = query.Where(d => d.LastModified <= form.LastModifiedTo);
            }
            
            if (form.ReportedDateFrom != null)
            {
                query = query.Where(d => d.ReportedDate >= form.ReportedDateFrom);
            }
            
            if (form.ReportedDateTo != null)
            {
                query = query.Where(d => d.ReportedDate <= form.ReportedDateTo);
            }
            
            if (form.DateFoundFrom != null )
            {
                query = query.Where(d => d.DateFound >= form.DateFoundFrom);
            }
            
            if (form.DateFoundTo != null)
            {
                query = query.Where(d => d.DateFound <= form.DateFoundTo);
            }
            
            query = query.Where(d =>
                (form.IsHazardousFind == null || d.IsHazardousFind == form.IsHazardousFind) &&
                (form.IsDredge == null || d.IsDredge == form.IsDredge) &&
                (form.IsIsolatedFind == null || d.WreckId == null == form.IsIsolatedFind) &&
                (form.AssignedToUserId == null || d.AssignedToUserId == form.AssignedToUserId)
            );
            query = query.Where(d =>
                ( form.StatusList.IsNullOrEmpty() ||
                  form.StatusList.Contains(d.Status) ));
            
            //Wreck Filters
            
            if (!string.IsNullOrEmpty(form.WreckName))
            {
                query = query.Where(d => 
                    d.Wreck != null && 
                    (!string.IsNullOrEmpty(d.Wreck.Name) &&
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.WreckName.ToLower(), d.Wreck.Name.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.WreckName.ToLower(), d.Wreck.Name.ToLower())) < MaxLevenshteinDistance ||
                     d.Wreck.Name.ToLower().Contains(form.WreckName.ToLower()))
                );
            }
            
            query = query.Where(d =>
                !form.WreckName.HasValue() ||
                d.Wreck != null &&
                d.Wreck.Name.HasValue()
            );
            
            //Salvor Filters
            
            if (!string.IsNullOrEmpty(form.SalvorName))
            {
                query = query.Where(d => 
                    d.Salvor != null && 
                    (!string.IsNullOrEmpty(d.Salvor.Name) &&
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.SalvorName.ToLower(), d.Salvor.Name.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.SalvorName.ToLower(), d.Salvor.Name.ToLower())) < MaxLevenshteinDistance ||
                     d.Salvor.Name.ToLower().Contains(form.SalvorName.ToLower()))
                );
            }
            
            query = query.Where(d =>
                !form.SalvorName.HasValue() ||
                ( d.Salvor != null &&
                  d.Salvor.Name.HasValue() ));
            
            //Location Filters
            
            if (form.LatitudeFrom != null )
            {
                query = query.Where(d =>
                    d.Latitude >= form.LatitudeFrom
                );
            }
            if (form.LatitudeTo != null )
            {
                query = query.Where(d =>
                    d.Latitude <= form.LatitudeTo
                );
            }
            if (form.LongitudeFrom != null )
            {
                query = query.Where(d =>
                    d.Longitude >= form.LongitudeFrom
                );
            }
            if (form.LongitudeTo != null )
            {
                query = query.Where(d =>
                    d.Longitude <= form.LongitudeTo
                );
            }
            if (form.DepthFrom != null )
            {
                query = query.Where(d =>
                    d.Depth >= form.DepthFrom
                );
            }
            if (form.DepthTo != null )
            {
                query = query.Where(d =>
                    d.Depth <= form.DepthTo
                );
            }
            
            if (!string.IsNullOrEmpty(form.LocationDescription))
            {
               query = query.Where(d => 
                    d.LocationDescription != null && 
                    (!string.IsNullOrEmpty(d.LocationDescription) &&
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.LocationDescription.ToLower(), d.LocationDescription.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.LocationDescription.ToLower(), d.LocationDescription.ToLower())) < MaxLevenshteinDistance ||
                     d.LocationDescription.ToLower().Contains(form.LocationDescription.ToLower()))
                );
            }


            query = query.Where(
                d => // long and lat to use location radius in calculation (method on droit data)
                    ( form.InUkWaters == null || d.InUkWaters == form.InUkWaters ) &&
                    ( form.RecoveredFromList.IsNullOrEmpty() ||
                      ( d.RecoveredFrom.HasValue &&
                        form.RecoveredFromList.Contains(d.RecoveredFrom.Value) ) )
            );

            //Wreck Material Filters
            query = query.Where(d =>
                form.IgnoreWreckMaterialSearch ||
                d.WreckMaterials.Any(wm =>
                    form.WreckMaterial != null &&
                    ( !string.IsNullOrEmpty(wm.Description) &&
                      ( usePsql
                          ? EF.Functions.FuzzyStringMatchLevenshtein(
                              form.WreckMaterial.ToLower(), wm.Description.ToLower())
                          : SearchHelper.GetLevenshteinDistance(form.WreckMaterial.ToLower(),
                              wm.Description.ToLower()) ) < MaxLevenshteinDistance ||
                      wm.Description.ToLower().Contains(form.WreckMaterial.ToLower()) )) &&
                d.WreckMaterials.Any(wm =>
                    form.WreckMaterialOwner != null &&
                    ( !string.IsNullOrEmpty(wm.WreckMaterialOwner) &&
                      ( usePsql
                          ? EF.Functions.FuzzyStringMatchLevenshtein(
                              form.WreckMaterialOwner.ToLower(),
                              wm.WreckMaterialOwner.ToLower())
                          : SearchHelper.GetLevenshteinDistance(
                              form.WreckMaterialOwner.ToLower(),
                              wm.WreckMaterialOwner.ToLower()) ) < MaxLevenshteinDistance ||
                      wm.WreckMaterialOwner.ToLower()
                          .Contains(form.WreckMaterialOwner.ToLower()) )) &&
                d.WreckMaterials.Any(wm =>
                    form.ValueConfirmed == null || wm.ValueConfirmed == form.ValueConfirmed));
            // the "in between" values here may need to be pull put to an if statement using the first conditional

            if ( form.QuantityFrom != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.Quantity >= form.QuantityFrom));
            }
            
            if ( form.QuantityTo != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.Quantity <= form.QuantityTo));
            }
            
            if ( form.ValueFrom != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.Value >= form.ValueFrom));
            }
            
            if ( form.ValueTo != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.Value <= form.ValueTo));
            }
           
            if ( form.ReceiverValuationFrom != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.ReceiverValuation >= form.ReceiverValuationFrom));
            }
            
            if ( form.ReceiverValuationTo != null )
            {
                query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        wm.ReceiverValuation <= form.ReceiverValuationTo));
            }
            //Salvage Filters
            
            if (!string.IsNullOrEmpty(form.ServicesDescription))
            {
                query = query.Where(d =>
                    d.Salvor != null && d.ServicesDescription != null && 
                    (!string.IsNullOrEmpty(d.ServicesDescription) &&
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.ServicesDescription.ToLower(), d.ServicesDescription.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.ServicesDescription.ToLower(), d.ServicesDescription.ToLower())) < MaxLevenshteinDistance ||
                     d.ServicesDescription.ToLower().Contains(form.ServicesDescription.ToLower()))
                );
            }
            
            if (!string.IsNullOrEmpty(form.ServicesDuration))
            {
                query = query.Where(d =>
                    d.Salvor != null && d.ServicesDuration != null && 
                    (!string.IsNullOrEmpty(d.ServicesDuration) &&
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.ServicesDuration.ToLower(), d.ServicesDuration.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.ServicesDuration.ToLower(), d.ServicesDuration.ToLower())) < MaxLevenshteinDistance ||
                     d.ServicesDuration.ToLower().Contains(form.ServicesDuration.ToLower()))
                );
            }
            
            if (form.ServicesEstimatedCostFrom != null )
            {
                query = query.Where(d =>
                    d.ServicesEstimatedCost >= form.ServicesEstimatedCostFrom
                );
            }
            
            if (form.ServicesEstimatedCostTo != null )
            {
                query = query.Where(d =>
                    d.ServicesEstimatedCost <= form.ServicesEstimatedCostTo
                );
            }
            
            if (form.SalvageClaimAwardedFrom != null )
            {
                query = query.Where(d =>
                    d.SalvageClaimAwarded >= form.SalvageClaimAwardedFrom
                );
            }
            
            if (form.SalvageClaimAwardedTo != null )
            {
                query = query.Where(d =>
                    d.SalvageClaimAwarded <= form.SalvageClaimAwardedTo
                );
            }
            
            query = query.Where(d =>
                ( form.SalvageAwardClaimed == null ||
                  d.SalvageAwardClaimed == form.SalvageAwardClaimed ) &&
                ( form.MMOLicenceRequired == null ||
                  d.MMOLicenceRequired == form.MMOLicenceRequired ) &&
                ( form.MMOLicenceProvided == null ||
                  d.MMOLicenceProvided == form.MMOLicenceProvided )
            );
            //Legacy Filters
            
            if (!string.IsNullOrEmpty(form.District))
            {
                query = query.Where(d =>
                    d.Salvor != null && !string.IsNullOrEmpty(d.District) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.District.ToLower(), d.District.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.District.ToLower(), d.District.ToLower())) < MaxLevenshteinDistance ||
                    d.District.ToLower().Contains(form.District.ToLower())
                );
            }
            
            if (!string.IsNullOrEmpty(form.LegacyFileReference))
            {
                query = query.Where(d =>
                    d.Salvor != null && !string.IsNullOrEmpty(d.LegacyFileReference) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.LegacyFileReference.ToLower(), d.LegacyFileReference.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.LegacyFileReference.ToLower(), d.LegacyFileReference.ToLower())) < MaxLevenshteinDistance ||
                    d.LegacyFileReference.ToLower().Contains(form.LegacyFileReference.ToLower())
                );
            }
            
            if (!string.IsNullOrEmpty(form.GoodsDischargedBy))
            {
                query = query.Where(d =>
                   d.Salvor != null && !string.IsNullOrEmpty(d.GoodsDischargedBy) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.GoodsDischargedBy.ToLower(), d.GoodsDischargedBy.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.GoodsDischargedBy.ToLower(), d.GoodsDischargedBy.ToLower())) < MaxLevenshteinDistance ||
                    d.GoodsDischargedBy.ToLower().Contains(form.GoodsDischargedBy.ToLower())
                );
            }
            
            if (!string.IsNullOrEmpty(form.DateDelivered))
            {
                query = query.Where(d =>
                    d.Salvor != null && !string.IsNullOrEmpty(d.DateDelivered) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.DateDelivered.ToLower(), d.DateDelivered.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.DateDelivered.ToLower(), d.DateDelivered.ToLower())) < MaxLevenshteinDistance ||
                    d.DateDelivered.ToLower().Contains(form.DateDelivered.ToLower())
                );
            }
            
            if (!string.IsNullOrEmpty(form.Agent))
            {
                query = query.Where(d =>
                    d.Salvor != null && !string.IsNullOrEmpty(d.Agent) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.Agent.ToLower(), d.Agent.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.Agent.ToLower(), d.Agent.ToLower())) < MaxLevenshteinDistance ||
                    d.Agent.ToLower().Contains(form.Agent.ToLower())
                );
            }
            
            if (!string.IsNullOrEmpty(form.RecoveredFromLegacy))
            {
                query = query.Where(d =>
                    d.Salvor != null && !string.IsNullOrEmpty(d.RecoveredFromLegacy) &&
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.RecoveredFromLegacy.ToLower(), d.RecoveredFromLegacy.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.RecoveredFromLegacy.ToLower(), d.RecoveredFromLegacy.ToLower())) < MaxLevenshteinDistance ||
                    d.RecoveredFromLegacy.ToLower().Contains(form.RecoveredFromLegacy.ToLower())
                );
            }
            
            query = query.Where(d =>
                (form.ImportedFromLegacy == null || d.ImportedFromLegacy == form.ImportedFromLegacy) 
            );
            
        return query;
    }

}