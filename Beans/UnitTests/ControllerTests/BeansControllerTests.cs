using Beans.Controllers;
using Beans.Models;
using Beans.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Beans.UnitTests.ControllerTests
{
    public class BeanControllerTests
    {
        private readonly Mock<IBeanService> _mockService;
        private readonly BeansController _controller;

        public BeanControllerTests()
        {
            _mockService = new Mock<IBeanService>();
            _controller = new BeansController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllBeans_ReturnsOkResult_WhenBeansExist()
        {
            var beans = new List<Bean>
            {
                new Bean { _id = "1", Name = "Espresso", Cost = "£3.50" },
                new Bean { _id = "2", Name = "Latte", Cost = "£4.00" }
            };
            _mockService.Setup(service => service.GetAllBeans()).ReturnsAsync(beans);

            var result = await _controller.GetAllBeans();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Bean>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task GetAllBeans_ReturnsNotFound_WhenNoBeansExist()
        {
            _mockService.Setup(service => service.GetAllBeans()).ReturnsAsync(new List<Bean>());

            var result = await _controller.GetAllBeans();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No beans found.", notFoundResult.Value);
        }

        [Fact]
        public void GetBeanById_ReturnsOk_WhenBeanExists()
        {
            var beanId = "123";
            var bean = new Bean { _id = beanId, Name = "Espresso", Cost = "£2.50" };
            _mockService.Setup(service => service.GetBeanById(beanId)).Returns(bean);

            var result = _controller.GetBeanById(beanId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(bean, okResult.Value);
        }

        [Fact]
        public void GetBeanById_ReturnsNotFound_WhenBeanDoesNotExist()
        {
            var beanId = "123";
            _mockService.Setup(service => service.GetBeanById(beanId)).Returns((Bean)null);

            var result = _controller.GetBeanById(beanId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddBean_ReturnsCreatedAtAction_WhenBeanIsCreated()
        {
            var createBean = new Bean { _id = "124", Name = "Latte", Cost = "£3.00" };
            var bean = new Bean { _id = "124", Name = "Latte", Cost = "£3.00" };
            _mockService.Setup(service => service.AddBean(createBean)).Returns(bean);

            var result = _controller.AddBean(createBean);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(bean, createdAtActionResult.Value);
        }

        [Fact]
        public void UpdateBean_ReturnsOk_WhenBeanIsUpdated()
        {
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino" };
            var updatedBean = new Bean { _id = beanId, Name = "Cappuccino" };
            _mockService.Setup(service => service.UpdateBean(beanId, updateBean)).Returns(updatedBean);

            var result = _controller.UpdateBean(beanId, updateBean);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedBean, okResult.Value);
        }

        [Fact]
        public void UpdateBean_ReturnsNotFound_WhenBeanDoesNotExist()
        {
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino" };
            _mockService.Setup(service => service.UpdateBean(beanId, updateBean)).Returns((Bean)null);

            var result = _controller.UpdateBean(beanId, updateBean);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteBean_ReturnsOK_WhenDeleted()
        {
            var beanId = "123";
            _mockService.Setup(service => service.DeleteBean(beanId)).Returns(true);

            var result = _controller.DeleteBean(beanId);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteBean_ReturnsNotFound_WhenBeanDoesNotExist()
        {
            var beanId = "123";
            _mockService.Setup(service => service.DeleteBean(beanId)).Returns(false);

            var result = _controller.DeleteBean(beanId);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void AddListOfBeans_ReturnsOk_WhenValidList()
        {
            var inputBeans = new List<Bean>
            {
                new Bean { _id = "1", Name = "Bean A", Cost = "£2.00" },
                new Bean { _id = "2", Name = "Bean B", Cost = "£2.50" }
            };

            _mockService.Setup(s => s.AddListOfBeans(inputBeans)).Returns(inputBeans);

            var result = _controller.AddListOfBeans(inputBeans);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBeans = Assert.IsAssignableFrom<List<Bean>>(okResult.Value);
            Assert.Equal(2, returnedBeans.Count);
        }

        [Fact]
        public void AddListOfBeans_ReturnsBadRequest_WhenListIsNull()
        {
            var result = _controller.AddListOfBeans(null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Bean list is invalid or empty.", badRequest.Value);
        }

        [Fact]
        public void AddListOfBeans_ReturnsBadRequest_WhenListIsEmpty()
        {
            var result = _controller.AddListOfBeans(new List<Bean>());

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Bean list is invalid or empty.", badRequest.Value);
        }

        [Fact]
        public async Task SelectBeanOfTheDay_ReturnsOk_WhenBeanIsSelected()
        {
            var winningBean = new Bean { _id = "2", Name = "Bean 2", Cost = "£3.30" };
            _mockService.Setup(service => service.PickBeanOfTheDay()).ReturnsAsync(winningBean);

            var result = await _controller.PickBeanOfTheDayWinner();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBean = Assert.IsType<Bean>(okResult.Value);
            Assert.Equal("2", returnedBean._id);
            Assert.Equal("Bean 2", returnedBean.Name);
        }

        [Fact]
        public async Task SelectBeanOfTheDay_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var errorMessage = "No potential winners available";
            _mockService.Setup(service => service.PickBeanOfTheDay()).ThrowsAsync(new Exception(errorMessage));

            var result = await _controller.PickBeanOfTheDayWinner();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Error assigning Bean of the Day: {errorMessage}", badRequestResult.Value);
        }

        [Fact]
        public async Task SearchBeans_ReturnsOkResult_WhenBeansMatchSearchCriteria()
        {
            var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Espresso", Description = "Strong coffee", Country = "Italy" },
            new Bean { _id = "2", Name = "Latte", Description = "Creamy coffee", Country = "USA" }
        };

            _mockService.Setup(service => service.SearchBeans("Espresso", null, null)).ReturnsAsync(new List<Bean> { beans[0] });

            var result = await _controller.SearchBeans(name: "Espresso");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Bean>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Espresso", returnValue.First().Name);
        }

        [Fact]
        public async Task SearchBeans_ReturnsNotFound_WhenNoBeansMatchSearchCriteria()
        {
            _mockService.Setup(service => service.SearchBeans("NonExistent", null, null)).ReturnsAsync(new List<Bean>());

            var result = await _controller.SearchBeans(name: "NonExistent");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No beans found matching the search criteria.", notFoundResult.Value);
        }
    }
}
