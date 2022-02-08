using Castle.Core.Logging;
using Catalog.Controllers;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using FluentAssertions;
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
            res.Result.Should().BeOfType<NotFoundResult>();

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
            res.Value.Should().BeEquivalentTo(expectedItem,
                options => options.ComparingByMembers<Item>());
            
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnAllItems()
        {
            //Arrange
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);


            //Act
            var result = await controller.GetItemsAsync();


            //Assert
            result.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Item>());  
        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnCreatedItem()
        {
            //Arrange
            var itemToCreate = new CreateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act
            var result = await controller.CreateItemAsync(itemToCreate);

            //Assert
            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }


        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto
            {
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000)
            };
  
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            // Arrange
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.DeleteItemAsync(existingItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
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
