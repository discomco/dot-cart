using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;

namespace DotCart.Abstractions.Clients;

public abstract class RequesterT<TDriver> : IRequester
    where TDriver : IRequesterDriverB
{
    private readonly TDriver _driver;

    protected RequesterT(TDriver driver)
    {
        _driver = driver;
    }

    public void Dispose()
    {
        _driver.Dispose();
    }

    public Task<Feedback> RequestAsync<THope>(THope hope) where THope : IHope
    {
        return _driver.RequestAsync(hope);
    }
}