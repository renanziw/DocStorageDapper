using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocStorage.Repository.Contracts
{
    public interface IBaseRepository<T>
    {
        public T Get(int id);
        public List<T> GetAll();
        public bool Delete(int id);
    }
}
