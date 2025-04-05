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

        [HttpPost]
        public IActionResult AddBean([FromBody] Bean bean)
        {
            try
            {
                _beanService.AddBean(bean);
                return Ok(new { message = "Bean added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }


}
