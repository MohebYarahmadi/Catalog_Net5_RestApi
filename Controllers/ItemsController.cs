using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Extensions;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ItemsController : ControllerBase
	{
		private readonly IItemsRepository _repository;
		public ItemsController(IItemsRepository repository)
		{
			this._repository = repository;
		}


		// GET /items
		[HttpGet]
		public async Task<IEnumerable<ItemDto>> GetItemsAsync()
		{
			var items = (await _repository.GetItemsAsync())
									.Select(item => item.AsDto());
			return items;
		}

		// GET /items/id
		[HttpGet("{id}")]
		public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
		{
			var item = await _repository.GetItemAsync(id);

			if (item is null)
			{
				return NotFound();
			}

			return Ok(item.AsDto());
		}

		// POST /items
		[HttpPost]
		public async Task<ActionResult<ItemDto>> CreateItemAsync(ItemToCreateDto item)
		{
			Item newItem = new()
			{
				Id = Guid.NewGuid(),
				Name = item.Name,
				Price = item.Price,
				CreatedDate = DateTimeOffset.UtcNow
			};

			await _repository.CreateItemAsync(newItem);

			return CreatedAtAction(nameof(GetItemAsync), new { id = newItem.Id }, newItem.AsDto());
		}

		// PUT /items/id
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateItemAsync(Guid id, ItemToUpdateDto item)
		{
			var existingItem = await _repository.GetItemAsync(id);

			if (existingItem is null)
			{
				return NotFound();
			}

			Item updatedItem = existingItem with
			{
				Name = item.Name,
				Price = item.Price
			};

			await _repository.UpdtaeItemAsync(updatedItem);

			return NoContent();
		}

		// DELETE /items/id
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteItemAsync(Guid id)
		{
			var existingItem = await _repository.GetItemAsync(id);

			if (existingItem is null)
			{
				return NotFound();
			}

			await _repository.DeleteItemAsync(id);

			return NoContent();
		}
	}
}