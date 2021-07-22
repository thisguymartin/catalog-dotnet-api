using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Models;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
  [Produces("application/json")]
  [ApiController]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemRepository repository;


    public ItemsController(IItemRepository repository)
    {
      this.repository = repository;
    }


    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
      var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());
      return items;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
      var item = await repository.GetItemAsync(id);

      if (item is null)
      {
        return NotFound();
      }

      return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {

      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };

      await repository.CreateItemAsync(item);

      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItem(Guid id, CreateItemDto itemDto)
    {

      var existingItem = await repository.GetItemAsync(id);

      if (existingItem is null)
      {
        return NotFound();
      }


      Item updatedItem = existingItem with
      {
        Name = itemDto.Name,
        Price = itemDto.Price
      };

      await repository.UpdateItemAsync(updatedItem);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItem(Guid id)
    {
      var existingItem = await repository.GetItemAsync(id);
      if (existingItem is null)
      {
        return NotFound();
      }

      await repository.DeleteItemAsync(existingItem.Id);

      return NoContent();
    }

  }
}