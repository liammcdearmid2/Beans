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

    //Search db
    [HttpGet("search")]
    public async Task<IActionResult> SearchBeans([FromQuery] string name = null, [FromQuery] string description = null, [FromQuery] string country = null)
    {
        var beans = await _beanService.SearchBeans(name, description, country);
        if (beans.Any())
        {
            return Ok(beans);
        }
        return NotFound("No beans found matching the search criteria.");
    }

    [HttpPost("singleBean")]
    public IActionResult AddBean([FromBody] Bean createBean)
    {
        if (createBean == null)
        {
            return BadRequest("Invalid data.");
        }

        // Check if the bean with the same ID already exists
        var existingBean = _beanService.GetBeanById(createBean._id);
        if (existingBean != null)
        {
            return BadRequest($"A bean with ID '{createBean._id}' already exists.");
        }

        // Add the new bean
        var bean = _beanService.AddBean(createBean);

        // Return the created bean with the appropriate status code
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

        // List to hold existing bean IDs
        var existingBeanIds = new List<string>();

        // Loop through the beans and check if the ID already exists
        foreach (var bean in beans)
        {
            var existingBean = _beanService.GetBeanById(bean._id);
            if (existingBean != null)
            {
                existingBeanIds.Add(bean._id);
            }
        }

        // If any existing bean IDs are found, return a bad request with the IDs
        if (existingBeanIds.Any())
        {
            return BadRequest($"The following Bean IDs already exist: {string.Join(", ", existingBeanIds)}");
        }

        // If no existing IDs, proceed to add the beans
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

        var existingBean = _beanService.GetBeanById(id);
        if (existingBean == null)
        {
            return NotFound($"Bean with ID '{id}' not found.");
        }

        var updatedBean = _beanService.UpdateBean(id, updateBean);
        if (updatedBean == null)
        {
            return NotFound($"Bean with ID '{id}' not found.");
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
