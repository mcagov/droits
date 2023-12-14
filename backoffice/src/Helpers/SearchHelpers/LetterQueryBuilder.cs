using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels.SearchFormModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Droits.Helpers.SearchHelpers;

public static class LetterQueryBuilder
{
    
    public static IQueryable<Letter> BuildQuery(LetterSearchForm form, IQueryable<Letter> query)
    {
            
        if ( !string.IsNullOrEmpty(form.Recipient) )
        {
             query = query.Where(l =>
                 !string.IsNullOrEmpty(l.Recipient) &&
                 l.Recipient.ToLower().Contains(form.Recipient.ToLower())
             );
        }
        
        query = query.Where(l =>
                ( form.StatusList.IsNullOrEmpty() ||
                  form.StatusList.Contains(l.Status) ) &&
                ( form.TypeList.IsNullOrEmpty() ||
                  form.TypeList.Contains(l.Type) )
            );

        query = query.OrderBy(l =>
                l.Status == LetterStatus.ReadyForQc ? 0 :
                l.Status == LetterStatus.ActionRequired ? 1 :
                l.Status == LetterStatus.QcApproved ? 2 :
                l.Status == LetterStatus.Draft ? 3 :
                4 // Sent
        ).ThenByDescending(l => l.Created);
        
        return query;
    }
    
}