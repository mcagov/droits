
#region

using System.Runtime.CompilerServices;
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
    Task<bool> HasSalvorsToExportAsync(SalvorSearchForm form);
    Task ExportAsync(SalvorSearchForm form, Stream output, CancellationToken cancellationToken = default);
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
        query = OrderSalvors(query, searchOptions);
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
        var query = OrderSalvors(QueryFromForm(form), form)
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


    private static IQueryable<Salvor> OrderSalvors(IQueryable<Salvor> query, SearchOptions searchOptions)
    {
        if (searchOptions.OrderColumn == nameof(SalvorView.DroitsCount))
        {
            return searchOptions.OrderDescending
                ? query.OrderByDescending(s => s.Droits.Count).ThenByDescending(s => s.Created)
                : query.OrderBy(s => s.Droits.Count).ThenByDescending(s => s.Created);
        }

        if (searchOptions.OrderColumn == nameof(SalvorView.Name))
        {
            return searchOptions.OrderDescending
                ? query.OrderByDescending(s => s.Name).ThenByDescending(s => s.Id)
                : query.OrderBy(s => s.Name).ThenBy(s => s.Id);
        }

        return query;
    }


    public async Task<bool> HasSalvorsToExportAsync(SalvorSearchForm form)
    {
        return await SalvorQueryBuilder.BuildQuery(form, _repo.GetSalvorsForExport()).AnyAsync();
    }

    public async Task ExportAsync(SalvorSearchForm form, Stream output, CancellationToken cancellationToken = default)
    {
        var query = SalvorQueryBuilder.BuildQuery(form, _repo.GetSalvorsForExport());
        var salvors = _repo.StreamSalvorsForExport(query);

        await ExportHelper.WriteRecordsAsync(ToExportDtos(salvors, cancellationToken),
            new SalvorsCsvMap(form.SalvorExportForm), output, cancellationToken);
    }

    private static async IAsyncEnumerable<SalvorExportDto> ToExportDtos(IAsyncEnumerable<Salvor> salvors,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach ( var salvor in salvors.WithCancellation(cancellationToken) )
        {
            yield return new SalvorExportDto(salvor);
        }
    }
    
    public async Task<Salvor> GetSalvorByPowerappsIdAsync(string powerappsId) =>
        await _repo.GetSalvorByPowerappsIdAsync(powerappsId);
}