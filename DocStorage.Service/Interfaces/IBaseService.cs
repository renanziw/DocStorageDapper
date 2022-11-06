using DocStorage.Util;

namespace DocStorage.Service.Interfaces
{
    public interface IBaseService<T>
    {
        public ServiceResponse Delete(int id);
    }
}
