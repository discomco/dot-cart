using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Defaults.CouchDb;
using MyCouch.Responses;

namespace DotCart.Drivers.CouchDB;

public static partial class Extensions
{
    public static ICouchDocStoreT<TDbInfo, TDoc, TID> AsCouchDocStore<TDbInfo, TDoc, TID>(this IStoreB dbStore)
        where TID : IID
        where TDoc : IState
        where TDbInfo : ICouchDbInfoB
    {
        return (ICouchDocStoreT<TDbInfo, TDoc, TID>)dbStore;
    }

    public static Error AsError(this DatabaseHeaderResponse rsp)
    {
        var res = Error.Empty;
        res.Code = rsp.StatusCode;
        res.Method = rsp.RequestMethod.Method;
        res.Subject = rsp.DbName;
        res.Content = rsp.Reason;
        res.ErrorMessage = rsp.Error;
        return res;
    }
}