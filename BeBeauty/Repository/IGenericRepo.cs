using BeBeauty.Models;

namespace BeBeauty.Repository
{
    public interface IGenericRepo<T> where T : class
    {

       T GetById(int id);
       List<T> GetAll();
       void Add(T entity);
        void Update(T entity);
        void  Delete(int id);
        void Save();
    }
}
