using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly RMSDBContext _rMSDBContext;

        public CategoryRepository(RMSDBContext rMSDBContext)
        {
            _rMSDBContext = rMSDBContext;
        }

        public void Create(CategoryEntity categoryEntity)
        {
            _rMSDBContext.Categories.Add(categoryEntity);
            _rMSDBContext.SaveChanges();
        }

        public void Delete(string Id)
        {
            var entity = _rMSDBContext.Categories.Where(x => x.Id.Equals(Id)).SingleOrDefault();
            if (entity == null)
            {
                return;
            }
            _rMSDBContext.Categories.Remove(entity);
            _rMSDBContext.SaveChanges();
        }

        public CategoryEntity GetById(string Id)
        {
            return _rMSDBContext.Categories.Where(x => x.Id == Id).SingleOrDefault();
        }

        public IList<CategoryEntity> Retrieve()
        {
            return _rMSDBContext.Categories.ToList();
        }

        public void Update(CategoryEntity categoryEntity)
        {
            _rMSDBContext.Entry(categoryEntity).State = EntityState.Modified;
            _rMSDBContext.SaveChanges();
        }
    }
}
