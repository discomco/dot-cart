using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class InitializeController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<Initialize.Payload, EventMeta> _hope2Cmd;

    public InitializeController(ICmdHandler cmdHandler, Hope2Cmd<Initialize.Payload, EventMeta> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Initialize.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            Guard.Against.Null(hope);
            var cmd = _hope2Cmd(hope);
            feedback = await _cmdHandler.HandleAsync(cmd);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
            Log.Fatal($"{AppErrors.Error} {e.InnerAndOuter()}");
        }

        return BadRequest(feedback);
    }
}