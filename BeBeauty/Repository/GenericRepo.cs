
using BeBeauty.Models;
using Microsoft.EntityFrameworkCore;

namespace BeBeauty.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {

        public ApplicationDbContext _Context;
        public GenericRepo(ApplicationDbContext applicationDbContext) {
        
            _Context = applicationDbContext;
        }


        public List<T> GetAll()
        {

            return _Context.Set<T>().ToList();
        }
        public T GetById(int id)
        {

           return _Context.Set<T>().Find(id);
            
        }
        public void Add(T entity)
        {
            _Context.Set<T>().Add(entity);
        }

        public void Delete(int id)
        {
           var entity = _Context.Set<T>().Find(id);
            if (entity != null)
            {
                _Context.Set<T>().Remove(entity);
            }
        }

       

        

        public void Save()
        {
           _Context.SaveChanges();
        }

        public void Update(T entity)
        {
            
            _Context.Entry(entity).State = EntityState.Modified;
        }
    }
}
