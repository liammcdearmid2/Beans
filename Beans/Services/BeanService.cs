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

        // GET: Get a single bean by ID
        public Bean GetBeanById(string id)
        {
            return _beanRepository.GetBeanById(id);
        }


        // POST: Add a new bean
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

        // PATCH: Update an existing bean
        public Bean UpdateBean(string id, UpdateBean updateBean)
        {
            var existingBean = _beanRepository.GetBeanById(id);
            if (existingBean == null)
            {
                return null; // Or throw an exception based on your error handling strategy
            }

            existingBean.Name = updateBean.Name ?? existingBean.Name;
            existingBean.Cost = updateBean.Cost ?? existingBean.Cost;
            existingBean.Description = updateBean.Description ?? existingBean.Description;
            existingBean.Colour = updateBean.Colour ?? existingBean.Colour;
            existingBean.Country = updateBean.Country ?? existingBean.Country;

            return _beanRepository.UpdateBean(existingBean);
        }

        // DELETE: Delete a bean by ID
        public bool DeleteBean(string id)
        {
            var existingBean = _beanRepository.GetBeanById(id);
            if (existingBean == null)
            {
                return false; // Or throw an exception
            }

            return _beanRepository.DeleteBean(id);
        }
    }
}
