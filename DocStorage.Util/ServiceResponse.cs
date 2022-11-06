namespace DocStorage.Util
{
    public class ServiceResponse<T>
    {
        public ServiceResponse() { }
        public ServiceResponse(T data) => Data = data;
        public List<ServiceError> Errors { get; set; } = new List<ServiceError>();
        public bool Success { get => !Errors?.Any() ?? true; }
        public T Data { get; set; }
        
        public static implicit operator T(ServiceResponse<T> value) => value.Data;
        public static implicit operator ServiceResponse<T>(T value) => new ServiceResponse<T>(value);
    }

    public class ServiceResponse : ServiceResponse<object>
    {
    }
}