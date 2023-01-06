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
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using static System.Threading.Tasks.Task;

namespace DotCart.Context.Actors;

public abstract class ActorT<TSpoke> : ActorB, IActorT<TSpoke>
{
    protected ActorT(IExchange exchange) : base(exchange)
    {
    }
}

public abstract class ActorB : ActiveComponent, IActor
{
    protected readonly IExchange _exchange;
    private ISpokeB _spoke;
    protected IDriverB Driver;

    protected ActorB(IExchange exchange)
    {
        _exchange = exchange;
    }

    public abstract Task HandleCast(IMsg msg, CancellationToken cancellationToken = default);
    public abstract Task<IMsg> HandleCall(IMsg msg, CancellationToken cancellationToken = default);

    protected override Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        return Run(async () =>
        {
            if (Driver != null)
                Driver.SetActor(this);
            while (_exchange.Status != ComponentStatus.Active)
            {
                _exchange.Activate(cancellationToken);
                await Delay(10, cancellationToken);
            }

            return CompletedTask;
        }, cancellationToken);
    }

    public void SetSpoke(ISpokeB spoke)
    {
        _spoke = spoke;
    }
}