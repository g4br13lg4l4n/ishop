using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort )
    {
        // this is to create the specification
        var spec = new ProductSpecification(brand, type, sort);
        // this is to get the products from the repository
        var products = await repo.ListAsync(spec);
        // this is to return the products
        return Ok(products);
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync()) {
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
        
        repo.Update(product);
        if (await repo.SaveAllAsync()) {
            return Ok(product);
        }
        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
        
        repo.Remove(product);
        if (await repo.SaveAllAsync()) {
            return Ok();
        }
        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        var brands = await repo.ListAsync(spec);
        return Ok(brands);
    } 

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();
        var types = await repo.ListAsync(spec);
        return Ok(types);
    } 

    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
