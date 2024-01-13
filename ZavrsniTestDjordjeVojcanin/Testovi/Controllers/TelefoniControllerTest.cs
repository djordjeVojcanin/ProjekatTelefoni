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
using ZavrsniTestDjordjeVojcanin.Models.DTO;

namespace Testovi.Controllers
{
    public class TelefoniControllerTest
    {
        public class TelefoniControllersTest
        {


            // izmena postojeceg telefona  kada akcija vraca status 400
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


            // preuzimanje telefona po zadatom ID (akcija vraca 200 i objekat)
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

        // preuzimanje proizvodjaca po zadatom id kada akcija vraca status 404
        [Fact]
        public void GetProizvodjac_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IProizvodjacRepository>();

            var controller = new ProizvodjaciController(mockRepository.Object);

            // Act
            var actionResult = controller.GetProizvodjac(12) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
        }

        // preuzimanje svih telefona sa cenom izmedju 2 unete vrednosti, akcija vraca status 200 i vise objekata
        [Fact]
        public void Pretraga_ReturnsCollection()
        {
            // Arrange
            List<Telefon> telefoni = new List<Telefon>() {
                new Telefon() {
                          Id = 1,
                    Model = "A94",
                    OperativniSistem = "Android",
                    DostupnaKolicina = 12,
                    Cena = 31125.42m,
                    ProizvodjacId = 3,
           Proizvodjac = new Proizvodjac {Id = 3, Naziv = "Huawei", DrzavaPorekla = "Kina" }
           },
           new Telefon() {
                    Id = 2,
                   Model = "13T Pro",
                   OperativniSistem = "Android",
                   DostupnaKolicina = 7,
                   Cena = 104999.99m,
                   ProizvodjacId = 1,
           Proizvodjac = new Proizvodjac {Id = 1, Naziv = "Xiaomi", DrzavaPorekla = "Kina" }
           }
       };

            PretragaDTO dto = new()
            {
                Najmanje = 10000,
                Najvise = 50000
            };

            var mockRepository = new Mock<ITelefonRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(telefoni.AsQueryable());

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new TelefonProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new TelefoniController(mockRepository.Object, mapper);

            // Act
            var actionResult = controller.Pretraga(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<TelefonDTO> listResult = (List<TelefonDTO>)actionResult.Value;

            for (int i = 0; i < listResult.Count; i++)
            {
                Assert.Equal(telefoni[i].Id, listResult[i].Id);
                Assert.Equal(telefoni[i].Model, listResult[i].Model);
                Assert.Equal(telefoni[i].OperativniSistem, listResult[i].OperativniSistem);
                Assert.Equal(telefoni[i].DostupnaKolicina, listResult[i].DostupnaKolicina);
                Assert.Equal(telefoni[i].Cena, listResult[i].Cena);
                Assert.Equal(telefoni[i].ProizvodjacId, listResult[i].ProizvodjacId);
                Assert.Equal(telefoni[i].Proizvodjac.Naziv, listResult[i].ProizvodjacNaziv);
                Assert.Equal(telefoni[i].Proizvodjac.DrzavaPorekla, listResult[i].ProizvodjacDrzavaPorekla);
            }
        }



    }
}