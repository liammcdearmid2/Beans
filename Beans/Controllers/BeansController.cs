using Beans.Models;
using Beans.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beans.Controllers;

[ApiController]
[Route("[controller]")]
public class BeansController : ControllerBase
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeanController : ControllerBase
    {
        private readonly BeanService _beanService;

        public BeanController(BeanService beanService)
        {
            _beanService = beanService;
        }

        [HttpGet]
        public IActionResult GetBeans()
        {
            var beans = _beanService.GetBeans();
            return Ok(beans);
        }

    }


}
