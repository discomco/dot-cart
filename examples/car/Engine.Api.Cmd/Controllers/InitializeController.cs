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
    private readonly ISequenceT<Initialize.Payload> _sequence;

    public InitializeController(ISequenceBuilderT<Initialize.Payload> sequenceBuilder,
        Hope2Cmd<Initialize.Payload, EventMeta> hope2Cmd)
    {
        _sequence = sequenceBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<Initialize.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            Guard.Against.Null(hope);
            feedback = await _sequence.ExecuteAsync(hope);
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