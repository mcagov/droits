
#region

using AutoMapper;
using CsvHelper.Configuration;
using Droits.Data.Mappers.CsvMappers;
using Droits.Exceptions;
using Droits.Helpers;
using Droits.Helpers.Extensions;
using Droits.Helpers.SearchHelpers;
using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Models.FormModels.SearchFormModels;
using Droits.Models.ViewModels;
using Droits.Models.ViewModels.ListViews;
using Droits.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Services;

public interface ISalvorService
{
    Task<SalvorListView> GetSalvorListViewAsync(SearchOptions searchOptions);
    Task<List<Salvor>> GetSalvorsAsync();
    Task<Salvor> SaveSalvorAsync(Salvor salvor);
    Task<Salvor> GetSalvorAsync(Guid id);
    Task<Guid> SaveSalvorFormAsync(SalvorForm form);
    Task<SalvorListView> AdvancedSearchAsync(SalvorSearchForm form);
    Task<Salvor> GetOrCreateAsync(Salvor salvor);
    Task<byte[]> ExportAsync(SalvorSearchForm form);
    Task<Salvor> GetSalvorByEmailAsync(string salvorEmail);
    Task<Salvor> GetSalvorByPowerappsIdAsync(string powerappsId);
    Task<Salvor?> GetSalvorByNameAndAddressAsync(Salvor salvor);

}

public class SalvorService : ISalvorService
{
    private readonly ISalvorRepository _repo;
    private readonly IMapper _mapper;


    public SalvorService(ISalvorRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;

    }


    public async Task<SalvorListView> GetSalvorListViewAsync(SearchOptions searchOptions)
    {
        var query = searchOptions.IncludeAssociations
            ? _repo.GetSalvorsWithAssociations()
            : _repo.GetSalvors();
        var pagedItems = await ServiceHelper.GetPagedResult(
            query.Select(s => new SalvorView(s, searchOptions.IncludeAssociations)), searchOptions);

        return new SalvorListView(pagedItems.Items)
        {
            PageNumber = pagedItems.PageNumber,
            PageSize = pagedItems.PageSize,
            IncludeAssociations = pagedItems.IncludeAssociations,
            TotalCount = pagedItems.TotalCount
        };
    }


    public async Task<List<Salvor>> GetSalvorsAsync()
    {
        return await _repo.GetSalvors().ToListAsync();
    }


    public async Task<Salvor> SaveSalvorAsync(Salvor salvor)
    {
        if ( salvor.Id == default )
        {
            return await AddSalvorAsync(salvor);
        }

        return await UpdateSalvorAsync(salvor);
    }


    private async Task<Salvor> AddSalvorAsync(Salvor salvor)
    {
        var foundSalvor = await _repo.GetSalvorByEmailAddressAsync(salvor.Email.Trim().ToLower());

        if ( foundSalvor != null )
        {
            throw new DuplicateSalvorException("Salvor already exists with supplied email address");
        }
        
        return await _repo.AddAsync(salvor);
    }


    private async Task<Salvor> UpdateSalvorAsync(Salvor salvor)
    {
        return await _repo.UpdateAsync(salvor);
    }


    public async Task<Salvor> GetSalvorAsync(Guid id)
    {
        return await _repo.GetSalvorAsync(id);
    }


    public async Task<Salvor> GetSalvorByEmailAsync(string salvorEmail)
    {
        
        var existingSalvor =
            await _repo.GetSalvorByEmailAddressWithAssociationsAsync(salvorEmail.Trim().ToLower());


        if ( existingSalvor == null )
        {
            throw new SalvorNotFoundException($"No Salvor found with email address {salvorEmail}");
        }

        return existingSalvor;

    }
    public async Task<Guid> SaveSalvorFormAsync(SalvorForm form)
    {
        var salvor = form.ApplyChanges(new Salvor());

        return ( await SaveSalvorAsync(salvor) ).Id;
    }


    public async Task<SalvorListView> AdvancedSearchAsync(SalvorSearchForm form)
    {
        var query = QueryFromForm(form)
            .Select(s => new SalvorView(s, true));
        
        var pagedSalvors =
            await ServiceHelper.GetPagedResult(query, form);

        return new SalvorListView(pagedSalvors.Items)
        {
            PageNumber = pagedSalvors.PageNumber,
            PageSize = pagedSalvors.PageSize,
            IncludeAssociations = pagedSalvors.IncludeAssociations,
            TotalCount = pagedSalvors.TotalCount,
            SearchForm = form
        };
    }


    public async Task<Salvor> GetOrCreateAsync(Salvor salvor)
    {

        if ( !salvor.Email.HasValue() )
        {
            throw new SalvorNotFoundException("No email address supplied for Salvor");
        }
        
        var existingSalvor = await _repo.GetSalvorByEmailAddressAsync(salvor.Email.Trim().ToLower());

        if (existingSalvor != null)
        {
            return existingSalvor;
        }

        salvor.Created = DateTime.UtcNow;
        salvor.LastModified = DateTime.UtcNow;

        salvor = await SaveSalvorAsync(salvor);

        return salvor;
    }


    public async Task<Salvor?> GetSalvorByNameAndAddressAsync(Salvor salvor) => await _repo.GetSalvorByNameAndAddressAsync(salvor.Name, salvor.Address);

    private IQueryable<Salvor> QueryFromForm(SalvorSearchForm form)
    {

        var query = _repo.GetSalvorsWithAssociations();

        return SalvorQueryBuilder.BuildQuery(form,query);
    }


    private List<Salvor> SearchSalvors(IQueryable<Salvor> query)
    {
        return query.ToList();
    }
    
    
    public async Task<byte[]> ExportAsync(SalvorSearchForm form)
    {
        
        var query = QueryFromForm(form);

        var salvors = SearchSalvors(query);
        
       
        
        if (salvors.IsNullOrEmpty())
        {
            throw new Exception("No Salvors to export");
        }

        try
        {
            var salvorsData = salvors.Select(s => new SalvorExportDto(s)).ToList();
            return await ExportHelper.ExportRecordsAsync(salvorsData, new SalvorsCsvMap(form.SalvorExportForm));
        }
        catch ( Exception e )
        {
            Console.WriteLine(e);
            throw;
        }

       
    }
    
    public async Task<Salvor> GetSalvorByPowerappsIdAsync(string powerappsId) =>
        await _repo.GetSalvorByPowerappsIdAsync(powerappsId);
}