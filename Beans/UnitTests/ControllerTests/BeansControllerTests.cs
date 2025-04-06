﻿using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Beans.Models;
using Beans.Services;
using static Beans.Controllers.BeansController;
using Beans.Controllers;

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

    [Fact]
    public void AddListOfBeans_ReturnsOk_WhenValidList()
    {
        // Arrange
        var inputBeans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Bean A" },
            new Bean { _id = "2", Name = "Bean B" }
        };

        _mockService.Setup(s => s.AddListOfBeans(inputBeans)).Returns(inputBeans);

        // Act
        var result = _controller.AddListOfBeans(inputBeans);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBeans = Assert.IsAssignableFrom<List<Bean>>(okResult.Value);
        Assert.Equal(2, returnedBeans.Count);
    }

    [Fact]
    public void AddListOfBeans_ReturnsBadRequest_WhenListIsNull()
    {
        // Act
        var result = _controller.AddListOfBeans(null);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Bean list is invalid or empty.", badRequest.Value);
    }

    [Fact]
    public void AddListOfBeans_ReturnsBadRequest_WhenListIsEmpty()
    {
        // Act
        var result = _controller.AddListOfBeans(new List<Bean>());

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Bean list is invalid or empty.", badRequest.Value);
    }

    [Fact]
    public void SelectBeanOfTheDay_ReturnsOk_WhenBeanIsSelected()
    {
        // Arrange
        var winningBean = new Bean { _id = "2", Name = "Bean 2" };
        _mockService.Setup(service => service.PickBeanOfTheDay()).ReturnsAsync(winningBean);

        // Act
        var result = _controller.PickBeanOfTheDayWinner();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedBean = Assert.IsType<Bean>(okResult.Value);
        Assert.Equal("2", returnedBean._id);
        Assert.Equal("Bean 2", returnedBean.Name);
    }

    [Fact]
    public void SelectBeanOfTheDay_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var errorMessage = "No potential winners available";
        _mockService.Setup(service => service.PickBeanOfTheDay()).Throws(new Exception(errorMessage));

        // Act
        var result = _controller.PickBeanOfTheDayWinner();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal($"Error selecting Bean of the Day: {errorMessage}", badRequestResult.Value);
    }
}