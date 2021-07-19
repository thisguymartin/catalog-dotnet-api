using System;
using System.Collections.Generic;
using System.Linq;
using Catalog.Models;

namespace Catalog.Repositories
{

  public class InMemRepository : IItemRepository
  {

    private readonly List<Item> items = new()
    {
      new Item { Id = Guid.NewGuid(), Name = "Magic Sword", Price = 99.00, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Magic Shield", Price = 80.00, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Helmet", Price = 30.00, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Boots", Price = 30.00, CreatedDate = DateTimeOffset.UtcNow }

    };

    public IEnumerable<Item> GetItems()
    {

      return items;

    }


    public Item GetItem(Guid id)
    {
      return items.Where(item => item.Id == id).SingleOrDefault();
    }

    public void CreateItem(Item item)
    {
      items.Add(item);
    }

    public void UpdateItem(Item item)
    {

      var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
      items[index] = item;

    }

    public void DeleteItem(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items.RemoveAt(index);
    }
  }

}