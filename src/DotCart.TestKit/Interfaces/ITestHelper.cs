using AutoFixture;

namespace DotCart.TestKit.Interfaces;

public interface ITestHelper
{
    string GetXml(byte[] fileData);
    byte[] GetBytes(string filepath);
    string GetEmbeddedResourceLocation(string filename);
    string GetEmbeddedResourceText(string resourceName);

    Fixture CreateFixture();
    IEnumerable<T> CreateMany<T>(int count = 3);
    public T Create<T>();
}