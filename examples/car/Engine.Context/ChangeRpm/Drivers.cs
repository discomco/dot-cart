using DotCart.Abstractions.Drivers;
using DotCart.Abstractions.Schema;
using DotCart.Drivers.InMem;
using Engine.Contract.ChangeRpm;

namespace Engine.Context.ChangeRpm;

public static class Drivers
{
    
  
    
    
    public interface IMemResponderDriver : IResponderDriverT<Hope>
    {
    }

    public class MemResponderDriver : MemResponderDriver<Hope>, IMemResponderDriver
    {
        public MemResponderDriver(
            GenerateHope<Hope> generateHope
        ) : base(generateHope)
        {
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}