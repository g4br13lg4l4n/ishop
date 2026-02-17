using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> productRepo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort )
    {
        return Ok(await productRepo.ListAllAsync());
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepo.Add(product);
        if (await productRepo.SaveAllAsync()) {
            return CreatedAtAction(
                nameof(GetProduct), 
                new { id = product.Id }, 
                product
            );
        } 

        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (!ProductExists(id)) return NotFound();
        
        productRepo.Update(product);
        if (await productRepo.SaveAllAsync()) {
            return Ok(product);
        }
        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await productRepo.GetByIdAsync(id);
        if (product == null) return NotFound();
        
        productRepo.Remove(product);
        if (await productRepo.SaveAllAsync()) {
            return Ok();
        }
        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        // TODO: Get the brands from the database
        return Ok();
    } 

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        // TODO: Get the types from the database
        return Ok();
    } 

    private bool ProductExists(int id)
    {
        return productRepo.Exists(id);
    }
}
