using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Repositories
{
    public interface ICategoryRepository
    {
        void Create(CategoryEntity categoryEntity);
        IList<CategoryEntity> Retrieve();
        void Update(CategoryEntity categoryEntity);
        void Delete(string Id);//Id = category Id
        CategoryEntity GetById(string Id);
    }
}
