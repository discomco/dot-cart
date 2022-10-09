using System.Reflection;

namespace DotCart.Tests;

public class AssemblyUtilsTests
{
    [Fact]
    public void GetLoadedModulesShouldNotReturnAnEmptyArray()
    {
        // GIVEN
        // WHEN
        var modules = AssemblyUtils.GetLoadedModules();
        // THEN
        Assert.NotEmpty(modules);
    }

    [Fact]
    public void ShouldGetTheVersionOfTheAssembly()
    {
        // GIVEN
        var assy = Assembly.GetAssembly(GetType());
        // WHEN
        var version = AssemblyUtils.GetVersion(assy);
        // THEN
        Assert.NotEmpty(version);
    }

    [Fact]
    public void ShouldGetTheShortNameofTheAssembly()
    {
        // GIVEN
        var assy = Assembly.GetAssembly(GetType());
        // WHEN
        var shortName = AssemblyUtils.ShortName(assy);
        // THEN
        Assert.NotEmpty(shortName);
        Assert.Equal("DotCart.Tests", shortName);
    }

    [Fact]
    public void ShouldGetAnEmbeddedFileFromTheAssembly()
    {
        // GIVEN
        const string fileName = "files\\invictus.md";
        var assy = Assembly.GetAssembly(GetType());
        // WHEN
        var file = AssemblyUtils.GetEmbeddedFile(assy, fileName);
        // THEN
        Assert.NotNull(file);
        Assert.True(file.Length>0);
    }

    [Fact]
    public void ShouldGetAnEmbeddedFileFromANamedAssembly()
    {
        // GIVEN
        const string fileName = "files\\invictus.md";
        var assyName = Assembly.GetAssembly(GetType()).FullName;
        // WHEN
        var file = AssemblyUtils.GetEmbeddedFileFromNamedAssembly(assyName, fileName);
        // THEN
        Assert.NotNull(file);
        Assert.True(file.Length>0);
        
    }
    
    
    
}