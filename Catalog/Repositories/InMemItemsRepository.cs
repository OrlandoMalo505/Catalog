﻿using Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Repositories
{
    public class InMemItemsRepository
    {

        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = System.DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = System.DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedDate = System.DateTimeOffset.UtcNow }
        };


        public IEnumerable<Item> GetItems()
        {
            return items;   
        }

        public Item GetItemById(Guid id)
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }
    }
}