using Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Service;
using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestFixture]
    public class ReservHourServiceTest
    {
        private BamaTestDbContext _dbContext;
        private IReservHourService _reservHourService;

        [SetUp]
        public void Setup()
        {
            var _dbContextOptions = new DbContextOptionsBuilder<BamaTestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new BamaTestDbContext(_dbContextOptions);
            var _workHourService = new WorkHourService(_dbContext);
            _reservHourService = new ReservHourService(_dbContext, _workHourService);
        }

        [Test]
        public async Task Reserve_ValidInput_ReturnsOk()
        {
            // Arrange
            var validDto = new ReserveDto
            {
                ReservedDate = new DateOnly(2024,6,9),
                Hour = 14,
                UserName = "JohnDoe"
            };

            // Act
            var result = await _reservHourService.Reserve(validDto);

            // Assert
            Assert.IsTrue(result.isSuccess);
            Assert.AreEqual(true, result.data);
        }
    }
}
