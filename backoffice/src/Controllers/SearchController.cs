﻿#region

using Droits.Models.DTOs;
using Droits.Models.DTOs.Exports;
using Droits.Services;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Droits.Controllers;

public class SearchController : BaseController
{
    private readonly ILogger<SearchController> _logger;
    private readonly IDroitService _droitService;


    public SearchController(ILogger<SearchController> logger, IDroitService droitService)
    {
        _logger = logger;
        _droitService = droitService;
    }


    [HttpGet]
    public async Task<IActionResult> SearchDroits(string query)
    {
        try
        {
            List<DroitExportDto> searchResults = await _droitService.SearchDroitsAsync(query);
            
            return Json(searchResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while searching.");
            return StatusCode(500, "An error occurred while searching.");
        }
    }
    
}