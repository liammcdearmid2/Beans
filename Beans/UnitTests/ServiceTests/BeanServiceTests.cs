using Beans.Models;
using Beans.Services;
using Moq;
using Mysqlx.Crud;
using Xunit;

namespace Beans.UnitTests.ServiceTests
{
    public class BeanServiceTests
    {
        private readonly Mock<IBeanRepository> _mockRepository;
        private readonly BeanService _beanService;

        public BeanServiceTests()
        {
            _mockRepository = new Mock<IBeanRepository>();
            _beanService = new BeanService(_mockRepository.Object);
        }

        // Test GetBeanById
        [Fact]
        public void GetBeanById_ReturnsBean_WhenBeanExists()
        {
            // Arrange
            var beanId = "123";
            var bean = new Bean { _id = beanId, Name = "Espresso", Cost = 2.50m };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(bean);

            // Act
            var result = _beanService.GetBeanById(beanId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(beanId, result._id);
        }

        [Fact]
        public void GetBeanById_ReturnsNull_WhenBeanDoesNotExist()
        {
            // Arrange
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            // Act
            var result = _beanService.GetBeanById(beanId);

            // Assert
            Assert.Null(result);
        }

        // Test AddBean
        [Fact]
        public void AddBean_AddsNewBean()
        {
            // Arrange
            var createBean = new Bean
            {
                _id = "125",
                Name = "Americano",
                Cost = 2.75m,
                Index = 1,
                IsBOTD = true,
                Colour = "Black",
                Description = "Strong coffee",
                Country = "USA"
            };
            var bean = new Bean
            {
                _id = "125",
                Name = "Americano",
                Cost = 2.75m,
                Index = 1,
                IsBOTD = true,
                Colour = "Black",
                Description = "Strong coffee",
                Country = "USA"
            };
            _mockRepository.Setup(repo => repo.AddBean(It.IsAny<Bean>())).Returns(bean);

            // Act
            var result = _beanService.AddBean(createBean);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("125", result._id);
        }

        // Test UpdateBean
        [Fact]
        public void UpdateBean_UpdatesExistingBean()
        {
            // Arrange
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino", Cost = 3.00m };
            var existingBean = new Bean { _id = beanId, Name = "Espresso", Cost = 2.50m };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(existingBean);
            _mockRepository.Setup(repo => repo.UpdateBean(It.IsAny<Bean>())).Returns(existingBean);

            // Act
            var result = _beanService.UpdateBean(beanId, updateBean);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cappuccino", result.Name);
            Assert.Equal(3.00m, result.Cost);
        }

        [Fact]
        public void UpdateBean_ReturnsNull_WhenBeanNotFound()
        {
            // Arrange
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino" };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            // Act
            var result = _beanService.UpdateBean(beanId, updateBean);

            // Assert
            Assert.Null(result);
        }

        // Test DeleteBean
        [Fact]
        public void DeleteBean_ReturnsTrue_WhenDeleted()
        {
            // Arrange
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(new Bean());
            _mockRepository.Setup(repo => repo.DeleteBean(beanId)).Returns(true);

            // Act
            var result = _beanService.DeleteBean(beanId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteBean_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            // Act
            var result = _beanService.DeleteBean(beanId);

            // Assert
            Assert.False(result);
        }
    }
