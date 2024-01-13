using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ZavrsniTestDjordjeVojcanin.Controllers;
using ZavrsniTestDjordjeVojcanin.Interface;
using ZavrsniTestDjordjeVojcanin.Models;

namespace Testiranje.Controllers
{
    public class TelefoniControllersTest
    {

        [Fact]
        public void PutTelefon_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            Telefon telefon = new Telefon()
            {
                Id = 1,
                Model = "A94",
                OperativniSistem = "Android",
                DostupnaKolicina = 12,
                Cena = 31125.42m,
                ProizvodjacId = 3
            };

            var mockRepository = new Mock<ITelefonRepository>();
            var mockMapper = new Mock<IMapper>();

            var controller = new TelefoniController(mockRepository.Object, mockMapper.Object);

            // Act
            var actionResult = controller.PutTelefon(24, telefon) as BadRequestResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void GetTelefon_ValidId_ReturnsObject()
        {
            // Arrange
            Telefon telefon = new Telefon()
            {
                Id = 1,
                Model = "A94",
                OperativniSistem = "Android",
                DostupnaKolicina = 12,
                Cena = 31125.42m,
                ProizvodjacId = 3
            };


            var mockRepository = new Mock<ITelefonRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(telefon);

            var mockMapper = new Mock<IMapper>();
            var controller = new TelefoniController(mockRepository.Object, mockMapper.Object);

            // Act
            var actionResult = controller.GetTelefon(1) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);
            Assert.Equal(telefon, actionResult.Value);
        }
    }
}
