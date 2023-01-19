using DotCart.TestKit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace DotCart.Drivers.CouchDB.Tests
{
    public class CouchInfoTests
        : IoCTests
    {

 
    
    
    
        public CouchInfoTests(ITestOutputHelper output, IoCTestContainer testEnv) : base(output, testEnv)
        {
        }

        protected override void Initialize()
        {
        
        }

        protected override void SetEnVars()
        {
        
        }

        protected override void InjectDependencies(IServiceCollection services)
        {
        }
    }
}

