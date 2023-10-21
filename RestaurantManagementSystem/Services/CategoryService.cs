using RestaurantManagementSystem.DAO;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.Models.ViewModels;
using RestaurantManagementSystem.Repositories;
using RestaurantManagementSystem.Utilities;

namespace RestaurantManagementSystem.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepositoryI;

        public CategoryService(ICategoryRepository categoryRepositoryI)
        {
            _categoryRepositoryI = categoryRepositoryI;
        }
        public void Create(CategoryViewModel categoryViewModel)
        {
            CategoryEntity category = new CategoryEntity()
            {
                Code = categoryViewModel.Code,
                Name = categoryViewModel.Name,
                Id = Guid.NewGuid().ToString(),
                Ip = NetworkHelper.GetLocalIp()
            };
            _categoryRepositoryI.Create(category);
        }
        public void Update(CategoryViewModel categoryViewModel)
        {
            var entity = new CategoryEntity()
            {
                Id = categoryViewModel.Id,//not to generate new id because this is update processs 
                Name = categoryViewModel.Name,//c101
                Code = categoryViewModel.Code,
                Ip = NetworkHelper.GetLocalIp()
            };
            _categoryRepositoryI.Update(entity);
        }
        public void Delete(String Id)
        {
            _categoryRepositoryI.Delete(Id);
        }
        public IList<CategoryViewModel> GetAll()
        {
            IList<CategoryViewModel> categories = _categoryRepositoryI.Retrieve().Select(x => new CategoryViewModel
            //data exchange between View Model and Model >> DTO  
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
            }).OrderBy(o => o.Code).ToList();
            return categories;
        }
        public  CategoryViewModel GetById(String Id)
        {
            var x= _categoryRepositoryI.GetById(Id);
            return new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
            };
        }
    }
}
