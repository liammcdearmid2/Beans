using Beans.Models;
using Beans.Services;
using Microsoft.AspNetCore.Mvc;

namespace Beans.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeansController : ControllerBase
{
    private readonly IBeanService _beanService;

    // Constructor to inject the BeanService
    public BeansController(IBeanService beanService)
    {
        _beanService = beanService;
    }

    // GET:
    [HttpGet("allBeans")]
    public async Task<IActionResult> GetAllBeans()
    {
        var beans = await _beanService.GetAllBeans();
        if (beans == null || !beans.Any())
        {
            return NotFound("No beans found.");
        }
        return Ok(beans);
    }

    // GET:
    [HttpGet("{id}")]
    public IActionResult GetBeanById(string id)
    {
        var bean = _beanService.GetBeanById(id);

        if (bean == null)
        {
            return NotFound($"Bean with ID {id} not found.");
        }

        return Ok(bean);
    }

    // POST: 
    [HttpPost("singleBean")]
    public IActionResult AddBean([FromBody] Bean createBean)
    {
        if (createBean == null)
        {
            return BadRequest("Invalid data.");
        }

        var bean = _beanService.AddBean(createBean);
        return CreatedAtAction(nameof(GetBeanById), new { id = bean._id }, bean);
    }

    //POST:
    [HttpPost("multipleBeans")]
    public IActionResult AddListOfBeans([FromBody] List<Bean> beans)
    {
        if (beans == null || !beans.Any())
        {
            return BadRequest("Bean list is invalid or empty.");
        }

        var addedBeans = _beanService.AddListOfBeans(beans);
        return Ok(addedBeans);
    }

    // PATCH:
    [HttpPatch("{id}")]
    public IActionResult UpdateBean(string id, [FromBody] UpdateBean updateBean)
    {
        if (updateBean == null)
        {
            return BadRequest("Invalid data.");
        }

        var updatedBean = _beanService.UpdateBean(id, updateBean);

        if (updatedBean == null)
        {
            return NotFound($"Bean with ID {id} not found.");
        }

        return Ok(updatedBean);
    }

    // DELETE:
    [HttpDelete("{id}")]
    public IActionResult DeleteBean(string id)
    {
        var result = _beanService.DeleteBean(id);

        if (!result)
        {
            return NotFound($"Bean with ID {id} not found.");
        }

        return Ok($"Bean {id} successfully deleted");
    }

    // POST:
    [HttpPost("pick-botd")]
    public async Task<IActionResult> PickBeanOfTheDayWinner()
    {
        try
        {
            var beanOfTheDay = await _beanService.PickBeanOfTheDay();
            if (beanOfTheDay == null)
            {
                return NotFound("No beans found.");
            }
            return Ok(beanOfTheDay);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error assigning Bean of the Day: {ex.Message}");
        }
    }
}
