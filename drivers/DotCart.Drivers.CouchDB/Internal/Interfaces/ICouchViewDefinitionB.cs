namespace DotCart.Drivers.CouchDB.Internal.Interfaces;

public interface ICouchViewDefinitionB
{
    DesignDocument Doc { get; set; }
    string Name { get; set; }
    ICouchDatabase Db();
    string Path();
    ICouchRequest Request();
}