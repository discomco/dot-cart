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
    private readonly IPipeT<Context.ChangeDetails.IHopePipe, ChangeDetails.Payload> _pipe;

    public ChangeDetailsController(
        IPipeBuilderT<Context.ChangeDetails.IHopePipe, ChangeDetails.Payload> pipeBuilder
        )
    {
        _pipe = pipeBuilder.Build();
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> Post([FromBody] HopeT<ChangeDetails.Payload> hope)
    {
        var feedback = Feedback.New(hope.AggId);
        try
        {
            feedback = await _pipe.ExecuteAsync(hope);
            return Ok(feedback);
        }
        catch (Exception e)
        {
            feedback.SetError(e.AsError());
        }

        return BadRequest(feedback);
    }
}