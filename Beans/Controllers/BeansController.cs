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
        [HttpPost]
        public IActionResult AddBean([FromBody] CreateBean bean)
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

        [HttpPatch("{id}")]
        public IActionResult UpdateBean(string id, [FromBody] UpdateBean bean)
        {
            try
            {
                var success = _beanService.UpdateBean(id, bean);

                if (!success)
                    return NotFound(new { message = "Bean not found or no fields to update." });

                return Ok(new { message = "Bean updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }


}
