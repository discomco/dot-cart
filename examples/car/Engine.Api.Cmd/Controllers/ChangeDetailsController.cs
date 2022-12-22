using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using Engine.Behavior;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ChangeDetailsController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<ChangeDetails.Cmd, Contract.ChangeDetails.Hope> _hope2Cmd;

    public ChangeDetailsController(
        ICmdHandler cmdHandler,
        Hope2Cmd<ChangeDetails.Cmd, Contract.ChangeDetails.Hope> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] Contract.ChangeDetails.Hope hope)
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