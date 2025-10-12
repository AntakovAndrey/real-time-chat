using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    [Authorize]
    [HttpGet("getChatsByUserId")]
    public async Task<IActionResult> GetChatsByUserId(CancellationToken cancellationToken)
    {
        var userClaimId = HttpContext.User.FindFirst("id")?.Value;
        var userId = Guid.Parse(userClaimId);
        var foundChats = await _chatService.GetChatsByUserId(userId, cancellationToken);
        return Ok(foundChats);
    }
}
