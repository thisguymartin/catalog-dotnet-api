using System;
using System.Collections.Generic;
using System.Linq;
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
    public IEnumerable<ItemDto> GetItems()
    {
      return repository.GetItems().Select(item => item.AsDto());
    }


    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
      var item = repository.GetItem(id);

      if (item is null)
      {
        return NotFound();
      }

      return Ok(item);
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

      repository.CreateItem(item);

      return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());

    }

    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, CreateItemDto itemDto)
    {

      var existingItem = repository.GetItem(id);

      if (existingItem is null)
      {
        return NotFound();
      }


      Item updatedItem = existingItem with
      {
        Name = itemDto.Name,
        Price = itemDto.Price
      };

      repository.UpdateItem(updatedItem);

      return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteItem(Guid id)
    {
      var existingItem = repository.GetItem(id);
      if (existingItem is null)
      {
        return NotFound();
      }

      repository.DeleteItem(existingItem.Id);


      return NoContent();
    }

  }
}