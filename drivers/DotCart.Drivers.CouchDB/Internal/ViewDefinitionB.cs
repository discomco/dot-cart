using DotCart.Drivers.CouchDB.Internal.Interfaces;

namespace DotCart.Drivers.CouchDB.Internal;

public abstract class ViewDefinitionB : ICouchViewDefinitionB
{
    protected ViewDefinitionB(string name, DesignDocument doc)
    {
        Doc = doc;
        Name = name;
    }

    public DesignDocument Doc { get; set; }
    public string Name { get; set; }

    public ICouchDatabase Db()
    {
        return Doc.Owner;
    }

    public ICouchRequest Request()
    {
        return Db().Request(Path());
    }

    public virtual string Path()
    {
        if (Doc.Id == "_design/") return Name;
        return Doc.Id + "/_view/" + Name;
    }
}