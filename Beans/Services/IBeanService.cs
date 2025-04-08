using Beans.Models;

namespace Beans.Services
{
    public interface IBeanService
    {
        //Get all beans
        Task<IEnumerable<Bean>> GetAllBeans();

        //Get bean by ID
        Bean GetBeanById(string id);

        //Add new bean
        Bean AddBean(Bean createBean);

        //Add list of beans
        List<Bean> AddListOfBeans(List<Bean> beans);

        //Update existing bean
        Bean UpdateBean(string id, UpdateBean updateBean);

        //Delete bean by ID
        bool DeleteBean(string id);

        //Select BOTD
        Task<Bean> PickBeanOfTheDay();
        Task<IEnumerable<Bean>> SearchBeans(string name = null, string description = null, string country = null);
    }
}
