using System.Net;
using Newtonsoft.Json.Linq;

namespace DotCart.Drivers.CouchDB.Internal.Interfaces;

public interface ICouchDatabase
{
    string Name { get; set; }
    ICouchServer Server { get; set; }
    void Copy(string sourceDocumentId, string destinationDocumentId);
    void Copy(string sourceDocumentId, string destinationDocumentId, string destinationRev);
    void Copy(ICouchDocument sourceDocument, ICouchDocument destinationDocument);
    int CountDocuments();
    void Create();
    ICouchDocument CreateDocument(ICouchDocument document);
    JsonDocument CreateDocument(string json);
    void Delete();
    void DeleteAttachment(string id, string rev, string attachmentName);
    ICouchDocument DeleteAttachment(ICouchDocument document, string attachmentName);
    void DeleteDocument(string id, string rev);
    void DeleteDocument(ICouchDocument document);
    void DeleteDocuments(string startKey, string endKey);
    void DeleteDocuments(IEnumerable<ICouchDocument> documents);
    void DeleteDocuments(ICanJson bulk);
    bool Exists();
    void FetchDocumentIfChanged(ICouchDocument document);
    IEnumerable<JsonDocument> GetAllDocuments();
    IEnumerable<T> GetAllDocuments<T>() where T : ICouchDocument, new();
    IEnumerable<Document> GetAllDocumentsWithoutContent();
    T GetArbitraryDocument<T>(string documentId, Func<T> ctor);
    IEnumerable<T> GetArbitraryDocuments<T>(IEnumerable<string> documentIds, Func<T> ctor);
    T GetDocument<T>(string documentId) where T : ICouchDocument, new();
    JsonDocument GetDocument(string documentId);
    IEnumerable<JsonDocument> GetDocuments(IEnumerable<string> documentIds);
    IEnumerable<T> GetDocuments<T>(IEnumerable<string> documentIds) where T : ICouchDocument, new();
    bool HasAttachment(string documentId, string attachmentName);
    bool HasAttachment(ICouchDocument document, string attachmentName);
    bool HasDocument(ICouchDocument document);
    bool HasDocument(string documentId);
    bool HasDocumentChanged(string documentId, string rev);
    bool HasDocumentChanged(ICouchDocument document);
    void Initialize();
    DesignDocument NewDesignDocument(string aName);
    ICouchViewDefinitionB NewTempView(string designDoc, string viewName, string mapText);
    CouchQuery Query(ICouchViewDefinitionB view);
    CouchQuery Query(string designName, string viewName);
    CouchQuery QueryAllDocuments();
    WebResponse ReadAttachment(string documentId, string attachmentName);
    WebResponse ReadAttachment(ICouchDocument document, string attachmentName);
    void ReadDocument(ICouchDocument document);
    JObject ReadDocument(string documentId);
    void ReadDocumentIfChanged(ICouchDocument document);
    string ReadDocumentString(string documentId);
    ICouchRequest Request(string path);
    ICouchRequest Request();
    ICouchRequest RequestAllDocuments();
    T SaveArbitraryDocument<T>(T document);
    void SaveArbitraryDocuments<T>(IEnumerable<T> documents, bool allOrNothing);
    void SaveArbitraryDocuments<T>(IEnumerable<T> documents, int chunkCount, bool allOrNothing);

    void SaveArbitraryDocuments<T>(IEnumerable<T> documents, int chunkCount,
        IEnumerable<ICouchViewDefinitionT> views, bool allOrNothing);

    ICouchDocument SaveDocument(ICouchDocument document);

    void SaveDocuments(IEnumerable<ICouchDocument> documents, int chunkCount,
        IEnumerable<ICouchViewDefinitionT> views, bool allOrNothing);

    void SaveDocuments(IEnumerable<ICouchDocument> documents, bool allOrNothing);
    void SaveDocuments(IEnumerable<ICouchDocument> documents, int chunkCount, bool allOrNothing);
    void SynchDesignDocuments();
    void TouchView(string designDocumentId, string viewName);
    void TouchViews(IEnumerable<ICouchViewDefinitionT> views);

    ICouchDocument WriteAttachment(ICouchDocument document, string attachmentName, Stream attachmentData,
        string mimeType);

    ICouchDocument WriteAttachment(ICouchDocument document, string attachmentName, byte[] attachmentData,
        string mimeType);

    ICouchDocument WriteAttachment(ICouchDocument document, string attachmentName, string attachmentData,
        string mimeType);

    ICouchDocument WriteDocument(string json, string documentId);
    ICouchDocument WriteDocument(ICouchDocument document);
    ICouchDocument WriteDocument(ICouchDocument document, bool batch);
}