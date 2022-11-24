using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Behavior;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class InitializeController : ControllerBase
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<Initialize.Cmd, Contract.Initialize.Hope> _hope2Cmd;

    public InitializeController(ICmdHandler cmdHandler, Hope2Cmd<Initialize.Cmd, Contract.Initialize.Hope> hope2Cmd)
    {
        _cmdHandler = cmdHandler;
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] Contract.Initialize.Hope hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            if (hope == null)
                throw new ArgumentException("request cannot be null", nameof(hope));
            feedback = await _cmdHandler.HandleAsync(_hope2Cmd(hope));
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
            Log.Fatal($":: EXCEPTION :: [{GetType()}] => {e.InnerAndOuter()}");
        }

        return BadRequest(feedback);
    }
}