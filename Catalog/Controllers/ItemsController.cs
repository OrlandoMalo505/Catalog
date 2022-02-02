using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

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
        public IEnumerable<Item> GetItems()
        {
            return _repository.GetItems();
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item= _repository.GetItemById(id);
            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
    }
}
