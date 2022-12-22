using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Behavior;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class StopController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<Stop.Cmd, Contract.Stop.Hope> _hope2Cmd;

    public StopController(
        ICmdHandler cmdHandler,
        Hope2Cmd<Stop.Cmd, Contract.Stop.Hope> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] Contract.Stop.Hope hope)
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