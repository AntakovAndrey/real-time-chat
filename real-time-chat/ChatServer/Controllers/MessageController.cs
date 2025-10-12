using ChatServer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatServer.Controllers;

[ApiController]
[Route("api/message")]
public class MessageController : Controller
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }
    
    [HttpGet("getMessagesByChatId/{chatId}")]
    public async Task<IActionResult> GetMessagesByChatId(Guid chatId, CancellationToken cancellationToken)
    {
        var foundMessages = await _messageService.GetMessagesByChatId(chatId, cancellationToken);
        return Ok(foundMessages);
    }
}
