using Microsoft.AspNetCore.Mvc;

namespace BlindDateBot.V2.Controllers;

[ApiController]
[Route("[controller]")]
public class TgBotController : Controller
{
    private readonly ILogger<TgBotController> _logger;

    public TgBotController(ILogger<TgBotController> logger)
    {
        _logger = logger;
    }

    [HttpPost, Route("/upd")]
    public IActionResult Handle()
    {
        _logger.LogInformation("Update from TG received");
        return Ok();
    }
}