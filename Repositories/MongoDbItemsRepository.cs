using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{

  public class MongoDbItemsRepository : IItemRepository
  {
    private readonly IMongoCollection<Item> itemsCollection;
    private const string databaseName = "catalog";
    private const string collectionName = "items";
    private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
      IMongoDatabase database = mongoClient.GetDatabase(databaseName);
      itemsCollection = database.GetCollection<Item>(collectionName);
    }
    public void CreateItem(Item item)
    {
      itemsCollection.InsertOne(item);
    }

    public void DeleteItem(Guid id)
    {
      var filter = filterBuilder.Eq(existingItem => existingItem.Id, id);
      itemsCollection.DeleteOne(filter);
    }

    public Item GetItem(Guid id)
    {
      return itemsCollection.Find(item => item.Id == id).SingleOrDefault();
    }

    public IMongoCollection<Item> GetItems()
    {
      return itemsCollection;
    }


    IEnumerable<Item> IItemRepository.GetItems()
    {
      return itemsCollection.Find(new BsonDocument()).ToList();
    }

    public void UpdateItem(Item item)
    {
      var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
      itemsCollection.ReplaceOneAsync(filter, item);
    }
  }

}