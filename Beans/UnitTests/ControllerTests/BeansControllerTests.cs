using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Beans.Models;
using Beans.Services;
using static Beans.Controllers.BeansController;

public class BeanControllerTests
{
    private readonly Mock<BeanService> _mockService;
    private readonly BeanController _controller;

    public BeanControllerTests()
    {
        _mockService = new Mock<BeanService>();
        _controller = new BeanController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllBeans_ReturnsOkResult_WhenBeansExist()
    {
        // Arrange
        var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Espresso", Cost = 3.5m },
            new Bean { _id = "2", Name = "Latte", Cost = 4.0m }
        };
        _mockService.Setup(service => service.GetAllBeans()).ReturnsAsync(beans);

        // Act
        var result = await _controller.GetAllBeans();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Bean>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task GetAllBeans_ReturnsNotFound_WhenNoBeansExist()
    {
        // Arrange
        var beans = new List<Bean>();
        _mockService.Setup(service => service.GetAllBeans()).ReturnsAsync(beans);

        // Act
        var result = await _controller.GetAllBeans();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No beans found.", notFoundResult.Value);
    }

    [Fact]
    public void GetBeanById_ReturnsOk_WhenBeanExists()
    {
        // Arrange
        var beanId = "123";
        var bean = new Bean { _id = beanId, Name = "Espresso", Cost = 2.50m };
        _mockService.Setup(service => service.GetBeanById(beanId)).Returns(bean);

        // Act
        var result = _controller.GetBeanById(beanId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(bean, okResult.Value);
    }

    [Fact]
    public void GetBeanById_ReturnsNotFound_WhenBeanDoesNotExist()
    {
        // Arrange
        var beanId = "123";
        _mockService.Setup(service => service.GetBeanById(beanId)).Returns((Bean)null);

        // Act
        var result = _controller.GetBeanById(beanId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void AddBean_ReturnsCreatedAtAction_WhenBeanIsCreated()
    {
        // Arrange
        var createBean = new Bean { _id = "124", Name = "Latte", Cost = 3.00m };
        var bean = new Bean { _id = "124", Name = "Latte", Cost = 3.00m };
        _mockService.Setup(service => service.AddBean(createBean)).Returns(bean);

        // Act
        var result = _controller.AddBean(createBean);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(bean, createdAtActionResult.Value);
    }

    [Fact]
    public void UpdateBean_ReturnsOk_WhenBeanIsUpdated()
    {
        // Arrange
        var beanId = "123";
        var updateBean = new UpdateBean { Name = "Cappuccino" };
        var updatedBean = new Bean { _id = beanId, Name = "Cappuccino" };
        _mockService.Setup(service => service.UpdateBean(beanId, updateBean)).Returns(updatedBean);

        // Act
        var result = _controller.UpdateBean(beanId, updateBean);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedBean, okResult.Value);
    }

    [Fact]
    public void UpdateBean_ReturnsNotFound_WhenBeanDoesNotExist()
    {
        // Arrange
        var beanId = "123";
        var updateBean = new UpdateBean { Name = "Cappuccino" };
        _mockService.Setup(service => service.UpdateBean(beanId, updateBean)).Returns((Bean)null);

        // Act
        var result = _controller.UpdateBean(beanId, updateBean);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void DeleteBean_ReturnsNoContent_WhenDeleted()
    {
        // Arrange
        var beanId = "123";
        _mockService.Setup(service => service.DeleteBean(beanId)).Returns(true);

        // Act
        var result = _controller.DeleteBean(beanId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteBean_ReturnsNotFound_WhenBeanDoesNotExist()
    {
        // Arrange
        var beanId = "123";
        _mockService.Setup(service => service.DeleteBean(beanId)).Returns(false);

        // Act
        var result = _controller.DeleteBean(beanId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}