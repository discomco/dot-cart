using DotCart.Abstractions.Contract;
using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Core;
using DotCart.Schema;
using Serilog;

namespace DotCart.Actors;

public class ProviderT<TDriver, TQuery>
    : IProviderT<TQuery>
    where TQuery : IQueryB
    where TDriver : IQueryStoreT<TQuery>
{
    private readonly TDriver _driver;

    public ProviderT(TDriver driver)
    {
        _driver = driver;
    }

    public async Task<IFeedback> GetDataAsync(TQuery query, IFeedback previous = null)
    {
        var feedback = Feedback.New(query.AggId);
        try
        {
            feedback = await _driver.ExecuteAsync(query, previous);
            return feedback;
        }
        catch (Exception e)
        {
            Log.Error(AppErrors.Error(e.InnerAndOuter()));
            feedback.SetError(e.AsError());
        }

        return feedback;
    }
}