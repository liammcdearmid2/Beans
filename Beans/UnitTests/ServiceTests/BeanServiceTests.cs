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
                new Bean { _id = "1", Name = "Americano", Cost = 3.5m },
                new Bean { _id = "2", Name = "Flat White", Cost = 4.0m }
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
            var bean = new Bean { _id = beanId, Name = "Espresso", Cost = 2.50m };
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
                Cost = 2.75m,
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
        public void UpdateBean_UpdatesExistingBean()
        {
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino", Cost = 3.00m };
            var existingBean = new Bean { _id = beanId, Name = "Espresso", Cost = 2.50m };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns(existingBean);
            _mockRepository.Setup(repo => repo.UpdateBean(It.IsAny<Bean>())).Returns(existingBean);

            var result = _beanService.UpdateBean(beanId, updateBean);

            Assert.NotNull(result);
            Assert.Equal("Cappuccino", result.Name);
            Assert.Equal(3.00m, result.Cost);
        }

        [Fact]
        public void UpdateBean_Throws_WhenBeanNotFound()
        {
            var beanId = "123";
            var updateBean = new UpdateBean { Name = "Cappuccino" };
            _mockRepository.Setup(repo => repo.GetBeanById(beanId)).Returns((Bean)null);

            Assert.Throws<KeyNotFoundException>(() => _beanService.UpdateBean(beanId, updateBean));
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
    }
}
