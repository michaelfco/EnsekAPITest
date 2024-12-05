using EnsekTest.API.Controllers;
using EnsekTest.Application.Interface;
using EnsekTest.Application.Services;
using EnsekTest.Domain.Common;
using EnsekTest.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekTest.Tests;

public class MeterReadingControllerTests
{
    private readonly Mock<ICsvParserService> _csvParserMock;
    private readonly Mock<IDatabaseSeederService> _databaseSeederMock;
    private readonly Mock<IMeterReadingService> _meterReadingServiceMock;
    private readonly MeterReadingController _controller;

    public MeterReadingControllerTests()
    {
        //mock the dependencies
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();

       
        var meterReadingValidationService = new MeterReadingValidationService();        
        var meterReadingService = new MeterReadingService(
            accountRepositoryMock.Object,
            meterReadingRepositoryMock.Object,
            meterReadingValidationService  
        );

        //mock services
        _csvParserMock = new Mock<ICsvParserService>();
        _databaseSeederMock = new Mock<IDatabaseSeederService>();
       
        _controller = new MeterReadingController(
            meterReadingService,  
            _csvParserMock.Object,
            _databaseSeederMock.Object
        );
    }

    
    [Fact]
    public async Task Upload_NullFile_ReturnsBadRequest()
    {
        //act
        var result = await _controller.Upload(null);

        //assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid file", badRequestResult.Value);
    }

    [Fact]
    public async Task Upload_EmptyFile_ReturnsBadRequest()
    {
        //arrange
        var csvFileMock = new Mock<IFormFile>();
        csvFileMock.Setup(f => f.Length).Returns(0);

        //act
        var result = await _controller.Upload(csvFileMock.Object);

        //assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid file", badRequestResult.Value);
    }

    [Fact]
    public async Task ProcessReadingsAsync_ShouldFailForOlderReadings()
    {       
        var accountId = 1234;
        
        var latestReading = new MeterReading
        {
            AccountId = accountId,
            MeterReadingDateTime = DateTime.Parse("2024-12-04 10:00"),
            MeterReadValue = "1000"
        };
       
        var newReading = new MeterReading
        {
            AccountId = accountId,
            //older date
            MeterReadingDateTime = DateTime.Parse("2024-12-03 10:00"), 
            MeterReadValue = "1100"
        };

        //mock dependencies
        var accountRepositoryMock = new Mock<IAccountRepository>();
        var meterReadingRepositoryMock = new Mock<IMeterReadingRepository>();
        var validationServiceMock = new Mock<MeterReadingValidationService>();

        accountRepositoryMock.Setup(repo => repo.GetAccountAsync(accountId))
            .ReturnsAsync(new Account { AccountId = accountId });

        meterReadingRepositoryMock.Setup(repo => repo.GetLatestReadingAsync(accountId))
            .ReturnsAsync(latestReading);

        validationServiceMock.Setup(service => service.IsValidReading(newReading.MeterReadValue))
            .Returns(true);

        var meterReadingService = new MeterReadingService(
            accountRepositoryMock.Object,
            meterReadingRepositoryMock.Object,
            validationServiceMock.Object
        );

        var readings = new List<MeterReading> { newReading };

        //act
        var result = await meterReadingService.ProcessReadingsAsync(readings);

        //assert
        //non success
        Assert.Equal(0, result.Success);
        //fail for older date
        Assert.Equal(1, result.Failed);
    }

}
