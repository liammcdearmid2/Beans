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

        //Update an existing bean
        public Bean UpdateBean(string id, UpdateBean updateBean)
        {
            var existingBean = _beanRepository.GetBeanById(id);
            if (existingBean == null)
            {
                return null; //TO-DO: Add exception here
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
                return false; //TO-DO: Add exception here
            }

            return _beanRepository.DeleteBean(id);
        }
    }
}
