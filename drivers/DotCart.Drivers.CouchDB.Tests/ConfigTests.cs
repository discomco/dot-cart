using DotCart.Core;
using DotCart.TestKit.Mocks;

namespace DotCart.Drivers.CouchDB.Tests;

public class ConfigTests
{
    [Fact]
    public void ShouldSetDbExists()
    {
        // GIVEN
        var dbExists = false;
        // WHEN
        Config.SetDbExists(DbNameAtt.Get<TheContext.ICouchDocDbInfo>());
        var exists = Convert.ToBoolean(DotEnv.Get($"{DbNameAtt.Get<TheContext.ICouchDocDbInfo>()}_EXISTS"));
        // THEN
        Assert.True(exists);
    }

    [Fact]
    public void ShouldGetDbExists()
    {
        // GIVEN
        var dbExists = false;
        // WHEN
        DotEnv.Set($"{DbNameAtt.Get<TheContext.ICouchDocDbInfo>()}_EXISTS", true);
        var exists = Config.GetDbExists(DbNameAtt.Get<TheContext.ICouchDocDbInfo>());
        // THEN
        Assert.True(exists);
        DotEnv.Set($"{DbNameAtt.Get<TheContext.ICouchDocDbInfo>()}_EXISTS", false);
        exists = Config.GetDbExists(DbNameAtt.Get<TheContext.ICouchDocDbInfo>());
        // THEN
        Assert.False(exists);
    }
}