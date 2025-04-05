using Beans.Models;

namespace Beans.Services
{
    public interface IBeanRepository
    {
        Bean GetBeanById(string id);
        Task<IEnumerable<Bean>> GetAllBeans();
        Bean AddBean(Bean bean);
        Bean UpdateBean(Bean bean);
        bool DeleteBean(string id);
    }
}
