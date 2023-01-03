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
public class StartController : ControllerBase
{
    private readonly ICmdHandler _handler;
    private readonly Hope2Cmd<Start.Payload, EventMeta> _hope2Cmd;

    public StartController(ICmdHandler handler, Hope2Cmd<Start.Payload, EventMeta> hope2Cmd)
    {
        _handler = handler;
        _hope2Cmd = hope2Cmd;
    }


    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Start.Payload> hope)
    {
        if (hope == null) return BadRequest("Body cannot be null.");
        var fbk = Feedback.New(hope.AggId);
        try
        {
            var cmd = _hope2Cmd(hope);
            fbk = await _handler.HandleAsync(cmd);
            return Ok(fbk);
        }
        catch (Exception e)
        {
            Log.Debug(e.InnerAndOuter());
            fbk.SetError(e.AsError());
        }

        return BadRequest(fbk);
    }
}