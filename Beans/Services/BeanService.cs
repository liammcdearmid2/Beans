using Beans.Models;
using MySql.Data.MySqlClient;

namespace Beans.Services
{
    public class BeanService
    {
        private readonly IBeanRepository _beanRepository;

        public BeanService(IBeanRepository beanRepository)
        {
            _beanRepository = beanRepository;
        }

        //Get All Beans
        public async Task<IEnumerable<Bean>> GetAllBeans()
        {
            return await _beanRepository.GetAllBeans();
        }

        //Get a single bean by ID
        public Bean GetBeanById(string id)
        {
            return _beanRepository.GetBeanById(id);
        }


        //Add a new bean
        public Bean AddBean(Bean createBean)
        {
            var bean = new Bean
            {
                _id = createBean._id,
                Name = createBean.Name,
                Cost = createBean.Cost,
                Index = createBean.Index,
                IsBOTD = createBean.IsBOTD,
                Colour = createBean.Colour,
                Description = createBean.Description,
                Country = createBean.Country,
            };

            return _beanRepository.AddBean(bean);
        }

        //Add a list of beans
        public List<Bean> AddListOfBeans(List<Bean> beans)
        {
            var addedBeans = new List<Bean>();

            foreach (var bean in beans)
            {
                addedBeans.Add(_beanRepository.AddBean(bean));
            }

            return addedBeans;
        }

        //Update an existing bean
        public Bean UpdateBean(string id, UpdateBean updateBean)
        {
            var existingBean = _beanRepository.GetBeanById(id);
            if (existingBean == null)
            {
                throw new KeyNotFoundException($"Bean with ID '{id}' not found.");
            }

            existingBean.Name = updateBean.Name ?? existingBean.Name;
            existingBean.Cost = updateBean.Cost ?? existingBean.Cost;
            existingBean.Description = updateBean.Description ?? existingBean.Description;
            existingBean.Colour = updateBean.Colour ?? existingBean.Colour;
            existingBean.Country = updateBean.Country ?? existingBean.Country;

            return _beanRepository.UpdateBean(existingBean);
        }

        //Delete a bean by ID
        public bool DeleteBean(string id)
        {
            var existingBean = _beanRepository.GetBeanById(id);
            if (existingBean == null)
            {
                throw new KeyNotFoundException($"Bean with ID '{id}' not found.");
            }

            return _beanRepository.DeleteBean(id);
        }
    }
}
