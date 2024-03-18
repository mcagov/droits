using System.Linq.Expressions;
using Droits.Helpers.Extensions;
using Droits.Models.Entities;
using Droits.Models.FormModels.SearchFormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Helpers.SearchHelpers;

public static class DroitQueryBuilder
{
    public static IQueryable<Droit> BuildQuery(DroitSearchForm form, IQueryable<Droit> query,  bool usePsql = true)
    {
            
            if (!string.IsNullOrEmpty(form.Reference))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.Reference) &&
                    d.Reference.ToLower().Contains(form.Reference.ToLower())
                );
            }
            
            if (form.CreatedFrom != null )
            {
                query = query.Where(d => d.Created >= form.CreatedFrom.Value.StartOfDay());
            }
            
            if (form.CreatedTo != null)
            {
                query = query.Where(d => d.Created <= form.CreatedTo.Value.EndOfDay());
            }
            
            if (form.LastModifiedFrom != null)
            {
                query = query.Where(d => d.LastModified >= form.LastModifiedFrom.Value.StartOfDay());
            }
            
            if (form.LastModifiedTo != null)
            {
                query = query.Where(d => d.LastModified <= form.LastModifiedTo.Value.EndOfDay());
            }
            
            if (form.ReportedDateFrom != null)
            {
                query = query.Where(d => d.ReportedDate >= form.ReportedDateFrom.Value.StartOfDay());
            }
            
            if (form.ReportedDateTo != null)
            {
                query = query.Where(d => d.ReportedDate <= form.ReportedDateTo.Value.EndOfDay());
            }
            
            if (form.DateFoundFrom != null )
            {
                query = query.Where(d => d.DateFound >= form.DateFoundFrom.Value.StartOfDay());
            }
            
            if (form.DateFoundTo != null)
            {
                query = query.Where(d => d.DateFound <= form.DateFoundTo.Value.EndOfDay());
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
            query = query.Where(d =>
                ( form.TriageNumbers.IsNullOrEmpty() ||
                  (d.TriageNumber!= null && form.TriageNumbers.Contains(d.TriageNumber.Value) )));
            
            //Wreck Filters
            
            if (!string.IsNullOrEmpty(form.WreckName))
            {
                query = query.Where(d => 
                    d.Wreck != null && !string.IsNullOrEmpty(d.Wreck.Name) &&
                    (d.Wreck.Name.ToLower().Contains(form.WreckName.ToLower()) ||
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.WreckName.ToLower(), d.Wreck.Name.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.WreckName.ToLower(), d.Wreck.Name.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.WreckName))
                );
            }
            
            if (!string.IsNullOrEmpty(form.ReportedWreckName))
            {
                query = query.Where(d => 
                    d.ReportedWreckName != null && !string.IsNullOrEmpty(d.ReportedWreckName) &&
                    (d.ReportedWreckName.ToLower().Contains(form.ReportedWreckName.ToLower()) ||
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.ReportedWreckName.ToLower(), d.ReportedWreckName.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.ReportedWreckName.ToLower(), d.ReportedWreckName.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.ReportedWreckName))
                );
            }
            
            if (!string.IsNullOrEmpty(form.OwnerName))
            {
                query = query.Where(d => 
                    d.Wreck != null && !string.IsNullOrEmpty(d.Wreck.OwnerName) &&
                    (d.Wreck.OwnerName.ToLower().Contains(form.OwnerName.ToLower()) ||
                     (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.OwnerName.ToLower(), d.Wreck.OwnerName.ToLower()) :
                         SearchHelper.GetLevenshteinDistance(form.OwnerName.ToLower(), d.Wreck.OwnerName.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.OwnerName))
                );
            }
            
            //Salvor Filters
            
            if (!string.IsNullOrEmpty(form.SalvorName))
            {
                query = query.Where(d => 
                    d.Salvor != null && !string.IsNullOrEmpty(d.Salvor.Name) &&
                     (d.Salvor.Name.ToLower().Contains(form.SalvorName.ToLower()) || 
                      (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.SalvorName.ToLower(), d.Salvor.Name.ToLower()) :
                          SearchHelper.GetLevenshteinDistance(form.SalvorName.ToLower(), d.Salvor.Name.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.SalvorName))
                     
                );
            }
            
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
                   
                    d.LocationDescription != null && !string.IsNullOrEmpty(d.LocationDescription) &&
                    (d.LocationDescription.ToLower().Contains(form.LocationDescription.ToLower()) ||
                     (usePsql? 
                         EF.Functions.FuzzyStringMatchLevenshtein(
                             form.LocationDescription.ToLower(), d.LocationDescription.Substring(0,Math.Min(255, d.LocationDescription.Length)).ToLower())
                         : SearchHelper.GetLevenshteinDistance(form.LocationDescription.ToLower(), d.LocationDescription.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.LocationDescription))
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
            if ( !form.IgnoreWreckMaterialSearch )
            {
              query = query.Where(d =>
                    d.WreckMaterials.Any(wm =>
                        (string.IsNullOrEmpty(form.WreckMaterial)||
                        (!string.IsNullOrEmpty(wm.Description) &&
                        ( wm.Description.ToLower().Contains(form.WreckMaterial.ToLower()) ||
                          ( usePsql
                              ? EF.Functions.FuzzyStringMatchLevenshtein(
                                  form.WreckMaterial.ToLower(), wm.Description.Substring(0,Math.Min(255, wm.Description.Length)).ToLower())
                              : SearchHelper.GetLevenshteinDistance(form.WreckMaterial.ToLower(),
                                  wm.Description.ToLower()) ) <= SearchHelper.GetLevenshteinDistanceThreshold(form.WreckMaterial) )
                        ))
                &&
                        (string.IsNullOrEmpty(form.WreckMaterialOwner) ||
                        (!string.IsNullOrEmpty(wm.WreckMaterialOwner) &&
                        ( wm.WreckMaterialOwner.ToLower()
                              .Contains(form.WreckMaterialOwner.ToLower()) ||
                          ( usePsql
                              ? EF.Functions.FuzzyStringMatchLevenshtein(
                                  form.WreckMaterialOwner.ToLower(),
                                  wm.WreckMaterialOwner.Substring(0,Math.Min(255, wm.WreckMaterialOwner.Length)).ToLower())
                              : SearchHelper.GetLevenshteinDistance(
                                  form.WreckMaterialOwner.ToLower(),
                                  wm.WreckMaterialOwner.ToLower()) ) <= SearchHelper.GetLevenshteinDistanceThreshold(form.WreckMaterialOwner) ))
                    ) &&
                    
                        (form.ValueConfirmed == null || wm.ValueConfirmed == form.ValueConfirmed)
                &&
                    
                            ( form.QuantityFrom == null || wm.Quantity >= form.QuantityFrom ) &&
                            ( form.QuantityTo == null || wm.Quantity <= form.QuantityTo )
                        
                &&
                            ( form.ValueFrom == null || wm.Value >= form.ValueFrom ) &&
                            ( form.ValueTo == null || wm.Value <= form.ValueTo )
                        

                && 
                            ( form.ReceiverValuationFrom == null ||
                              wm.ReceiverValuation >= form.ReceiverValuationFrom ) &&
                            ( form.ReceiverValuationTo == null ||
                              wm.ReceiverValuation <= form.ReceiverValuationTo )
                        
                   ) );
                
            }
            //Salvage Filters
            
            if (!string.IsNullOrEmpty(form.ServicesDescription))
            {
                query = query.Where(d =>
                    d.ServicesDescription != null && 
                    !string.IsNullOrEmpty(d.ServicesDescription) &&
                     d.ServicesDescription.ToLower().Contains(form.ServicesDescription.ToLower()));
            }
            
            if (!string.IsNullOrEmpty(form.ServicesDuration))
            {
                query = query.Where(d =>
                    d.ServicesDuration != null && 
                    !string.IsNullOrEmpty(d.ServicesDuration) &&
                     (d.ServicesDuration.ToLower().Contains(form.ServicesDuration.ToLower()) ||
                      (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.ServicesDuration.ToLower(), d.ServicesDuration.Substring(0,Math.Min(255, d.ServicesDuration.Length)).ToLower()) :
                          SearchHelper.GetLevenshteinDistance(form.ServicesDuration.ToLower(), d.ServicesDuration.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.ServicesDescription))
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
                ( form.MmoLicenceRequired == null ||
                  d.MmoLicenceRequired == form.MmoLicenceRequired ) &&
                ( form.MmoLicenceProvided == null ||
                  d.MmoLicenceProvided == form.MmoLicenceProvided )
            );
            //Legacy Filters
            
            if (!string.IsNullOrEmpty(form.District))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.District) &&
                    (d.District.ToLower().Contains(form.District.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.District.ToLower(), d.District.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.District.ToLower(), d.District.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.District)
                   )
                );
            }
            
            if (!string.IsNullOrEmpty(form.LegacyFileReference))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.LegacyFileReference) &&
                    (d.LegacyFileReference.ToLower().Contains(form.LegacyFileReference.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.LegacyFileReference.ToLower(), d.LegacyFileReference.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.LegacyFileReference.ToLower(), d.LegacyFileReference.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.LegacyFileReference)
                   )
                );
            }
            
            if (!string.IsNullOrEmpty(form.GoodsDischargedBy))
            {
                query = query.Where(d =>
                   !string.IsNullOrEmpty(d.GoodsDischargedBy) &&
                    (d.GoodsDischargedBy.ToLower().Contains(form.GoodsDischargedBy.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.GoodsDischargedBy.ToLower(), d.GoodsDischargedBy.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.GoodsDischargedBy.ToLower(), d.GoodsDischargedBy.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.GoodsDischargedBy)
                   )
                );
            }
            
            if (!string.IsNullOrEmpty(form.DateDelivered))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.DateDelivered) &&
                    (d.DateDelivered.ToLower().Contains(form.DateDelivered.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.DateDelivered.ToLower(), d.DateDelivered.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.DateDelivered.ToLower(), d.DateDelivered.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.DateDelivered)
                   )
                );
            }
            
            if (!string.IsNullOrEmpty(form.Agent))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.Agent) &&
                    (d.Agent.ToLower().Contains(form.Agent.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.Agent.ToLower(), d.Agent.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.Agent.ToLower(), d.Agent.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.Agent)
                   )
                );
            }
            
            if (!string.IsNullOrEmpty(form.RecoveredFromLegacy))
            {
                query = query.Where(d =>
                    !string.IsNullOrEmpty(d.RecoveredFromLegacy) &&
                    (d.RecoveredFromLegacy.ToLower().Contains(form.RecoveredFromLegacy.ToLower()) || 
                    (usePsql? EF.Functions.FuzzyStringMatchLevenshtein(form.RecoveredFromLegacy.ToLower(), d.RecoveredFromLegacy.ToLower()) :
                        SearchHelper.GetLevenshteinDistance(form.RecoveredFromLegacy.ToLower(), d.RecoveredFromLegacy.ToLower())) <= SearchHelper.GetLevenshteinDistanceThreshold(form.RecoveredFromLegacy)
                   )
                );
            }
            
            query = query.Where(d =>
                (form.ImportedFromLegacy == null || d.ImportedFromLegacy == form.ImportedFromLegacy) 
            );
            
        return query;
    }

}