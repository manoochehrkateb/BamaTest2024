using Core.Domain;
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
    public class WorkHourServiceTest
    {
        private BamaTestDbContext _dbContext;
        private IWorkHourService _workHourService;

        [SetUp]
        public void Setup()
        {
            var _dbContextOptions = new DbContextOptionsBuilder<BamaTestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new BamaTestDbContext(_dbContextOptions);
            _workHourService = new WorkHourService(_dbContext);
        }


        [Test]
        public async Task ChangeCapacity_AddsNewRecord_WhenNoOverlappingDataExists()
        {
            // Arrange
            var dto = new ChangeCapacityDto
            {
                capacity = 100,
                startDate = new DateOnly(2024, 5, 5),
                endDate = new DateOnly(2024, 5, 15),
                endHour = 17,
                startHour = 10
            };

            // Act
            var result = await _workHourService.ChangeCapacity(dto);

            // Assert
            Assert.That(result.isSuccess, Is.True);
            // Add more assertions as needed
        }

        [Test]
        public async Task ChangeCapacity_ReturnsNotOk_WhenDuplicateDataExists()
        {
            // Arrange
            var existingRecord = new CustomeWorkHour
            {
                Capacity = 3,
                StartDate = new DateOnly(2024, 5, 5),
                EndDate = new DateOnly(2024, 5, 15),
                EndHour = 17,
                StartHour = 10
            };
            _dbContext.CustomeWorkHours.Add(existingRecord);
            await _dbContext.SaveChangesAsync();

            var dto = new ChangeCapacityDto
            {
                capacity = 100,
                startDate = new DateOnly(2024, 5, 5),
                endDate = new DateOnly(2024, 5, 15),
                endHour = 17,
                startHour = 10
            };

            // Act
            var result = await _workHourService.ChangeCapacity(dto);

            // Assert
            Assert.That(result.isSuccess, Is.False);
            Assert.That(result.error.FirstOrDefault(w => w.Contains("duplicate")), Is.EqualTo("it has duplicate row"));
        }

        [Test]
        public async Task ChangeCapacity_SplitsExistingRecord_WhenOverlappingDataExists()
        {
            // Arrange
            var existingRecord = new CustomeWorkHour
            {
                Capacity = 3,
                StartDate = new DateOnly(2024, 5, 16),
                EndDate = new DateOnly(2024, 5, 20),
                EndHour = 17,
                StartHour = 10
            };
            _dbContext.CustomeWorkHours.Add(existingRecord);
            await _dbContext.SaveChangesAsync();

            var dto = new ChangeCapacityDto
            {
                capacity = 100,
                startDate = new DateOnly(2024, 5, 17),
                endDate = new DateOnly(2024, 5, 18),
                endHour = 17,
                startHour = 10
            };

            // Act
            var result = await _workHourService.ChangeCapacity(dto);

            // Assert
            Assert.That(result.isSuccess, Is.True);
            // Add more assertions as needed
        }


        [Test]
        public void DefaultCapacity_ValidInput_ReturnsExpectedValue()
        {
            // Arrange
            var day = DayOfWeek.Monday;
            var hour = 10;

            // Act
            var result = _workHourService.DefaultCapacity(day, hour);

            // Assert
            Assert.AreEqual(2, result.data); // Expected default capacity
        }

        // Test for invalid input (invalid day)
        [Test]
        public void DefaultCapacity_InvalidDay_ReturnsNotOk()
        {
            // Arrange
            var day = (DayOfWeek)42; // Invalid day
            var hour = 14;

            // Act
            var result = _workHourService.DefaultCapacity(day, hour);

            // Assert
            Assert.IsFalse(result.isSuccess); // Should return NotOk
        }
    }
}
