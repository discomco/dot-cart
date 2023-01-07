////////////////////////////////////////////////////////////////////////////////////////////////
// MIT License
////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c)2023 DisComCo Sp.z.o.o. (http://discomco.pl)
////////////////////////////////////////////////////////////////////////////////////////////////
// Permission is hereby granted, free of charge,
// to any person obtaining a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
// and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS",
// WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////


using DotCart.Abstractions.Actors;
using DotCart.Abstractions.Behavior;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using Serilog;

namespace DotCart.Context.Actors;

[Name("CmdHandler")]
[Order(int.MaxValue)]
public class CmdHandlerStepT<TPipeInfo, TPayload, TMeta> : StepT<TPipeInfo, TPayload>
    where TPayload : IPayload
    where TMeta : IEventMeta
    where TPipeInfo : IPipeInfoB
{
    private readonly ICmdHandler _cmdHandler;
    private readonly Hope2Cmd<TPayload, TMeta> _hope2Cmd;

    public CmdHandlerStepT(Hope2Cmd<TPayload, TMeta> hope2Cmd, ICmdHandler cmdHandler)
    {
        Level = Importance.Crucial;
        _hope2Cmd = hope2Cmd;
        _cmdHandler = cmdHandler;
    }

    protected override string GetName()
    {
        return NameAtt.StepName(Level, NameAtt.Get(this), FactTopicAtt.Get<TPayload>());
    }

    public override async Task<Feedback> ExecuteAsync(IDto msg, Feedback? previousFeedback,
        CancellationToken cancellationToken = default)
    {
        var feedback = Feedback.New(msg.AggId, previousFeedback, Name);
        try
        {
//            Log.Information($"{AppVerbs.Executing} {Name}");
            var cmd = _hope2Cmd((HopeT<TPayload>)msg);
            feedback = await _cmdHandler.HandleAsync(cmd, previousFeedback, cancellationToken);
//            Log.Information($"{AppFacts.Executed} {Name}");
        }
        catch (Exception e)
        {
            Log.Error($"{AppErrors.Error(e.InnerAndOuter())} ");
            feedback.SetError(e.AsError());
        }

        return feedback;
    }
}