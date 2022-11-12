using DotCart.Contract;
using DotCart.Drivers.InMem;
using Engine.Contract.Initialize;

namespace Engine.Context.Initialize;

public static class Drivers
{
    public class ResponderDriver : MemResponderDriver<Hope>
    {
        public ResponderDriver(GenerateHope<Hope> generateHope) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
    
}