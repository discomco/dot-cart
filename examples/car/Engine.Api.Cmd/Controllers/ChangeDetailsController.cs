using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using Engine.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Engine.Api.Cmd.Controllers;

[ApiController]
[Route("/api/engine/[controller]")]
public class ChangeDetailsController : ControllerBase
{
    private readonly Hope2Cmd<ChangeDetails.Payload, EventMeta> _hope2Cmd;
    private readonly ISequenceT<ChangeDetails.Payload> _sequence;

    public ChangeDetailsController(
        ISequenceBuilderT<ChangeDetails.Payload> sequenceBuilder,
        Hope2Cmd<ChangeDetails.Payload, EventMeta> hope2Cmd)
    {
        _sequence = sequenceBuilder.Build();
        _hope2Cmd = hope2Cmd;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<ChangeDetails.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            feedback = await _sequence.ExecuteAsync(hope);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return BadRequest(feedback);
    }
}