using Catalog.Entities;
using System;
using System.Collections.Generic;

namespace Catalog.Repositories
{
    public interface IInMemItemsRepository
    {
        Item GetItemById(Guid id);
        IEnumerable<Item> GetItems();
    }
}