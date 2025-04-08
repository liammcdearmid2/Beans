using Beans.Models;

namespace Beans.Services
{
    public interface IBeanRepository
    {
        Bean GetBeanById(string id);
        Task<IEnumerable<Bean>> GetAllBeans();
        Bean AddBean(Bean bean);
        Bean UpdateBean(string id, Bean bean);
        bool DeleteBean(string id);
        //Bean of the day logic:
        Bean GetPreviousBOTD();
        void UpdateBeanAsBOTD(string id, bool isBOTD, DateTime previousWinnerDate);
        void ResetBOTD();
        Task<IEnumerable<Bean>> SearchBeans(string name = null, string description = null, string country = null);
    }
}
