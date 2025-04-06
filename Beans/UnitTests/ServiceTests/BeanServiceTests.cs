using Beans.Models;
using Beans.Services;
using Moq;
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

        [Fact]
        public async Task GetAllBeans_ReturnsAllBeans_WhenBeansExist()
        {
            // Arrange:
            var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Americano", Cost = 3.5m },
            new Bean { _id = "2", Name = "Flat White", Cost = 4.0m }
        };
            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(beans);

            // Act:
            var result = await _beanService.GetAllBeans();

            // Assert: 
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Name == "Americano");
            Assert.Contains(result, b => b.Name == "Flat White");
        }

        [Fact]
        public async Task GetAllBeans_ReturnsEmpty_WhenNoBeansExist()
        {
            // Arrange: 
            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(new List<Bean>());

            // Act:
            var result = await _beanService.GetAllBeans();

            // Assert: 
            Assert.Empty(result);
        }

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

        [Fact]
        public void AddListOfBeans_ReturnsAddedBeans()
        {
            // Arrange
            var inputBeans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Bean A" },
            new Bean { _id = "2", Name = "Bean B" }
        };

            foreach (var bean in inputBeans)
            {
                _mockRepository.Setup(r => r.AddBean(bean)).Returns(bean);
            }

            // Act
            var result = _beanService.AddListOfBeans(inputBeans);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Bean A", result[0].Name);
            Assert.Equal("Bean B", result[1].Name);
        }
    }
}
}
