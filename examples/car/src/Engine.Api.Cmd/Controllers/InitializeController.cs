using Ardalis.GuardClauses;
using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Engine.Context;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class InitializeController : ControllerBase
{
    private readonly IPipeT<Initialize.IHopePipe, Contract.Initialize.Payload> _pipe;

    public InitializeController(
        IPipeBuilderT<Initialize.IHopePipe, Contract.Initialize.Payload> pipeBuilder
    )
    {
        _pipe = pipeBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Contract.Initialize.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            Guard.Against.Null(hope);
            feedback = await _pipe.ExecuteAsync(hope);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
            Log.Fatal($"{AppErrors.Error(e.InnerAndOuter())}");
        }

        return BadRequest(feedback);
    }
}