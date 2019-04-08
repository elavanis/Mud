namespace Objects.Global.Serialization.Interface
{
    public interface ISerialization
    {
        T Deserialize<T>(string serializedObject);
        string Serialize(object obj);
    }
}