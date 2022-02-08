using Castle.Core.Logging;
using Catalog.Controllers;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Tests
{
    public class ItemsControllerTests
    {

        private readonly Mock<IInMemItemsRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new();


        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            //Arrange
            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);


            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act
            var res = await controller.GetItemAsync(Guid.NewGuid());

            //Assert
            Assert.IsType<NotFoundResult>(res.Result);

        }

        [Fact]

        public async Task GetItemAsync_WithExistingItem_ReturnExpected()
        {
            //Arrange
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);


            //Act
            var res = await controller.GetItemAsync(Guid.NewGuid());


            //Assert
            Assert.IsType<ItemDto>(res.Value);
            var dto = (res as ActionResult<ItemDto>).Value;
            Assert.Equal(expectedItem.Id, dto.Id);
            Assert.Equal(expectedItem.Name, dto.Name);
            Assert.Equal(expectedItem.Price, dto.Price);
            Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);
        }

        private Item CreateRandomItem()
        {
            return new Item
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                CreatedDate = DateTime.Now
            };
        }

    }
}
