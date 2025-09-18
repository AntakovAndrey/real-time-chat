using System.Security.Claims;
using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController :Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [Authorize]
    [HttpGet("quickSearch/{searchTerm}")]
    public async Task<IActionResult> QuickSearch(string searchTerm, CancellationToken cancellationToken)
    {
        var userClaimId = HttpContext.User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userClaimId))
        {
            return Unauthorized();
        }
        var userId = new Guid(userClaimId);
        var searchResult = await _searchService.QuickSearch(searchTerm, userId, cancellationToken);
        return Ok(searchResult);
    }
    
    [Authorize]
    [HttpGet("chatSearch")]
    public async Task<IActionResult> ChatSearch(string searchTerm, CancellationToken cancellationToken)
    {
        var userClaimId = HttpContext.User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userClaimId))
        {
            return Unauthorized();
        }
        var userId = new Guid(userClaimId);
        var searchResult = await _searchService.ChatSearch(searchTerm, userId, cancellationToken);
        return Ok(searchResult);
    }
    
    [Authorize]
    [HttpGet("messageSearch")]
    public async Task<IActionResult> MessageSearch(string searchTerm, CancellationToken cancellationToken)
    {
        var userClaimId = HttpContext.User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userClaimId))
        {
            return Unauthorized();
        }
        var userId = new Guid(userClaimId);
        var searchResult = await _searchService.MessageSearch(searchTerm, userId, cancellationToken);
        return Ok(searchResult);
    }
    
    [Authorize]
    [HttpGet("userSearch")]
    public async Task<IActionResult> UserSearch(string searchTerm, CancellationToken cancellationToken)
    {
        var userClaimId = HttpContext.User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(userClaimId))
        {
            return Unauthorized();
        }
        var userId = new Guid(userClaimId);
        var searchResult = await _searchService.UserSearch(searchTerm, userId, cancellationToken);
        return Ok(searchResult);
    }
}
