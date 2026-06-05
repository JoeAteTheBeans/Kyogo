namespace Kyogo.Application.Serialisation;

public interface ISerialiser
{ 
    public ValueTask<T> DeserialiseAsync<T>(Stream source, CancellationToken cancellationToken = default);
    public T Deserialise<T>(byte[] byteArray);
}