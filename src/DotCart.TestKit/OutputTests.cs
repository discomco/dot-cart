using Xunit.Abstractions;

namespace DotCart.TestKit;

public abstract class OutputTests : IDisposable
{
    private readonly TextWriter _originalOut;
    private readonly TextWriter _textWriter;
    protected readonly ITestOutputHelper Output;

    protected OutputTests(ITestOutputHelper output)
    {
        Output = output;
        _originalOut = Console.Out;
        _textWriter = new StringWriter();
        Console.SetOut(_textWriter);
    }

    public virtual void Dispose()
    {
        Cleanup();
        Output.WriteLine(_textWriter.ToString());
        Console.SetOut(_originalOut);
    }

    protected virtual void Cleanup()
    {
    }
}