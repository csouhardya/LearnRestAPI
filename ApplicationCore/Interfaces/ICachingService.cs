namespace ApplicationCore.Interfaces
{
    public interface ICachingService
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T data);
        void ReInsertData<T>(string key, T data);
        void RemoveData<T>(string key);
    }
}
