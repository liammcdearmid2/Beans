using Beans.Models;

namespace Beans.Services
{
    public interface IBeanRepository
    {
        // Get a single bean by ID
        Bean GetBeanById(string id);

        // Add a new bean
        Bean AddBean(Bean bean);

        // Update an existing bean
        Bean UpdateBean(Bean bean);

        // Delete a bean by ID
        bool DeleteBean(string id);
    }
}
