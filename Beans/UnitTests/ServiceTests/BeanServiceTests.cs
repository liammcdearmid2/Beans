using Beans.Models;
using Beans.Services;
using Moq;
using Xunit;

namespace Beans.UnitTests.ServiceTests
{
    public class BeanServiceTests
    {
        private readonly Mock<IBeanRepository> _mockRepository;
        private readonly IBeanService _beanService;

        public BeanServiceTests()
        {
            _mockRepository = new Mock<IBeanRepository>();
            _beanService = new BeanService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllBeans_ReturnsAllBeans_WhenBeansExist()
        {
            var beans = new List<Bean>
            {
                new Bean { _id = "1", Name = "Americano", Cost = "£3.50" },
                new Bean { _id = "2", Name = "Flat White", Cost = "£4.00" }
            };
            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(beans);

            var result = await _beanService.GetAllBeans();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, b => b.Name == "Americano");
            Assert.Contains(result, b => b.Name == "Flat White");
        }

        [Fact]
        public async Task GetAllBeans_ReturnsEmpty_WhenNoBeansExist()
        {
            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(new List<Bean>());

            var result = await _beanService.GetAllBeans();

            Assert.Empty(result);
        }

        [Fact]
        public void GetBeanById_ReturnsBean_WhenBeanExists()
        {
            var beanId = "123";
            var bean = new Bean { _id = beanId, Name = "Espresso", Cost = "£2.50" };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(bean);

            var result = _beanService.GetBeanById(beanId);

            Assert.NotNull(result);
            Assert.Equal(beanId, result._id);
        }

        [Fact]
        public void GetBeanById_ReturnsNull_WhenBeanDoesNotExist()
        {
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            var result = _beanService.GetBeanById(beanId);

            Assert.Null(result);
        }

        [Fact]
        public void AddBean_AddsNewBean()
        {
            var createBean = new Bean
            {
                _id = "125",
                Name = "Americano",
                Cost = "£2.75",
                Index = 1,
                IsBOTD = true,
                Colour = "Black",
                Description = "Strong coffee",
                Country = "USA"
            };
            var bean = createBean;
            _mockRepository.Setup(repo => repo.AddBean(It.IsAny<Bean>())).Returns(bean);

            var result = _beanService.AddBean(createBean);

            Assert.NotNull(result);
            Assert.Equal("125", result._id);
        }

        [Fact]
        public void UpdateBean_ReturnsNull_WhenBeanDoesNotExist()
        {
            // Arrange
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino", Cost = "£3.00" };

            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            // Act
            var result = _beanService.UpdateBean(beanId, updateBean);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateBean_ReturnsUpdatedBean_WhenBeanExists()
        {
            // Arrange
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino", Cost = "£3.00" };
            var existingBean = new Bean { _id = beanId, Name = "Espresso", Cost = "£2.50" };
            var updatedBean = new Bean { _id = beanId, Name = "Cappuccino", Cost = "£3.00" };

            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(existingBean);
            _mockRepository.Setup(repo => repo.UpdateBean(beanId, It.IsAny<Bean>())).Returns(updatedBean);

            // Act
            var result = _beanService.UpdateBean(beanId, updateBean);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cappuccino", result.Name);
            Assert.Equal("£3.00", result.Cost);
        }

        [Fact]
        public void DeleteBean_ReturnsTrue_WhenDeleted()
        {
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(new Bean());
            _mockRepository.Setup(repo => repo.DeleteBean(beanId)).Returns(true);

            var result = _beanService.DeleteBean(beanId);

            Assert.True(result);
        }

        [Fact]
        public void DeleteBean_Throws_WhenNotFound()
        {
            var beanId = "123";
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            Assert.Throws<KeyNotFoundException>(() => _beanService.DeleteBean(beanId));
        }

        [Fact]
        public void AddListOfBeans_ReturnsAddedBeans()
        {
            var inputBeans = new List<Bean>
            {
                new Bean { _id = "1", Name = "Bean A" },
                new Bean { _id = "2", Name = "Bean B" }
            };

            foreach (var bean in inputBeans)
            {
                _mockRepository.Setup(r => r.AddBean(bean)).Returns(bean);
            }

            var result = _beanService.AddListOfBeans(inputBeans);

            Assert.Equal(2, result.Count);
            Assert.Equal("Bean A", result[0].Name);
            Assert.Equal("Bean B", result[1].Name);
        }

        [Fact]
        public async Task PickBeanOfTheDay_ShouldReturnBean_WhenThereArePotentialWinners()
        {
            var allBeans = new List<Bean>
            {
                new Bean { _id = "1", Name = "Bean 1" },
                new Bean { _id = "2", Name = "Bean 2" },
                new Bean { _id = "3", Name = "Bean 3" }
            };

            var todaysBOTD = new Bean { _id = "1", Name = "Bean 1" };

            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(allBeans);
            _mockRepository.Setup(repo => repo.GetPreviousBOTD()).Returns(todaysBOTD);
            _mockRepository.Setup(repo => repo.ResetBOTD()).Verifiable();
            _mockRepository.Setup(repo => repo.UpdateBeanAsBOTD(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<DateTime>())).Verifiable();

            var result = await _beanService.PickBeanOfTheDay();

            Assert.NotNull(result);
            Assert.NotEqual(todaysBOTD._id, result._id);
            _mockRepository.Verify(repo => repo.ResetBOTD(), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateBeanAsBOTD(It.IsAny<string>(), true, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task PickBeanOfTheDay_ShouldThrowException_WhenNoPotentialWinners()
        {
            var allBeans = new List<Bean> { new Bean { _id = "1", Name = "Bean 1" } };
            var todaysBOTD = new Bean { _id = "1", Name = "Bean 1" };

            _mockRepository.Setup(repo => repo.GetAllBeans()).ReturnsAsync(allBeans);
            _mockRepository.Setup(repo => repo.GetPreviousBOTD()).Returns(todaysBOTD);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _beanService.PickBeanOfTheDay());
        }

        [Fact]
        public async Task SearchBeans_ReturnsBeans_WhenValidSearchCriteria()
        {
            var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Espresso", Description = "Strong coffee", Country = "Italy" },
            new Bean { _id = "2", Name = "Latte", Description = "Creamy coffee", Country = "USA" }
        };

            _mockRepository.Setup(repo => repo.SearchBeans("Espresso", null, null)).ReturnsAsync(new List<Bean> { beans[0] });

            var result = await _beanService.SearchBeans("Espresso", null, null);

            Assert.Single(result);
            Assert.Equal("Espresso", result.First().Name);
        }

        [Fact]
        public async Task SearchBeans_ReturnsEmpty_WhenNoBeansMatch()
        {
            _mockRepository.Setup(repo => repo.SearchBeans("NonExistent", null, null)).ReturnsAsync(new List<Bean>());

            var result = await _beanService.SearchBeans("NonExistent", null, null);

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchBeans_ReturnsBeans_WhenValidDescription()
        {
            var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Espresso", Description = "Strong coffee", Country = "Italy" },
            new Bean { _id = "2", Name = "Latte", Description = "Creamy coffee", Country = "USA" }
        };

            _mockRepository.Setup(repo => repo.SearchBeans(null, "Strong coffee", null)).ReturnsAsync(new List<Bean> { beans[0] });

            var result = await _beanService.SearchBeans(null, "Strong coffee", null);

            Assert.Single(result);
            Assert.Equal("Espresso", result.First().Name);
        }

        [Fact]
        public async Task SearchBeans_ReturnsBeans_WhenValidCountry()
        {
            var beans = new List<Bean>
        {
            new Bean { _id = "1", Name = "Espresso", Description = "Strong coffee", Country = "Italy" },
            new Bean { _id = "2", Name = "Latte", Description = "Creamy coffee", Country = "USA" }
        };

            _mockRepository.Setup(repo => repo.SearchBeans(null, null, "USA")).ReturnsAsync(new List<Bean> { beans[1] });

            var result = await _beanService.SearchBeans(null, null, "USA");

            Assert.Single(result);
            Assert.Equal("Latte", result.First().Name);
        }
    }
}
