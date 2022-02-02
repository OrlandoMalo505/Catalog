using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("/items")]
    public class ItemsController : ControllerBase
    {

        private readonly IInMemItemsRepository _repository;

        public ItemsController(IInMemItemsRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            return _repository.GetItems().Select(item => item.AsDto());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item= _repository.GetItemById(id);
            if (item is null)
            {
                return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            _repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());


        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            Item existingItem = _repository.GetItemById(id);

            if(existingItem is null)
            {
                return NotFound();
            }

            var updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            _repository.UpdateItem(updatedItem);

            return NoContent();

        }

        [HttpDelete]
        public ActionResult DeleteItem(Guid id)
        {
            var item = _repository.GetItemById(id);
            if(item is null)
            {
                return NotFound();
            }
            _repository.DeleteItem(id);

            return NoContent();
        }

    }
}
