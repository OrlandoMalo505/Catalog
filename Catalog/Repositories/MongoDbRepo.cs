using Catalog.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Catalog.Repositories
{
    public class MongoDbRepo : IInMemItemsRepository
    {
        private const string DatabaseName = "catalog";

        private const string CollectionName = "items";

        private readonly IMongoCollection<Item> itemsCollection;

        public MongoDbRepo(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
            itemsCollection = database.GetCollection<Item>(CollectionName);

        }
        public void CreateItem(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItemById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
