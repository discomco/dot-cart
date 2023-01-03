using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class StopController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<Stop.Payload, EventMeta> _hope2Cmd;

    public StopController(
        ISequenceBuilder sequenceBuilder,
        Hope2Cmd<Stop.Payload, EventMeta> hope2Cmd)
    {
        _cmdHandler = sequenceBuilder.Build();
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Stop.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            var cmd = _hope2Cmd(hope);
            feedback = await _cmdHandler.HandleAsync(cmd);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return BadRequest(feedback);
    }
}