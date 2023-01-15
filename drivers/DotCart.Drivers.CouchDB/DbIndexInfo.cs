using CouchDB.Driver.Indexes;
using DotCart.Abstractions.Schema;

namespace DotCart.Drivers.CouchDB;

public class DbIndexInfo<TReadModel> where TReadModel : IState
{
    private DbIndexInfo(string name, Action<IIndexBuilder<CDoc<TReadModel>>> builder, IndexOptions options)
    {
        Name = name;
        Builder = builder;
        Options = options;
    }

    public string Name { get; set; }

    public string DesignDoc { get; set; }
    public Action<IIndexBuilder<CDoc<TReadModel>>> Builder { get; set; }
    public IndexOptions Options { get; set; }

    public static DbIndexInfo<TReadModel> New(string indexName,
        Action<IIndexBuilder<CDoc<TReadModel>>> indexBuilder,
        IndexOptions options)
    {
        return new DbIndexInfo<TReadModel>(indexName, indexBuilder, options);
    }
}