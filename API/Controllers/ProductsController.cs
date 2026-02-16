using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository productRepo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
    {
        return Ok(await productRepo.GetProductsAsync());
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepo.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepo.AddProduct(product);
        if (await productRepo.SaveChangesAsync()) {
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
        
        productRepo.UpdateProduct(product);
        if (await productRepo.SaveChangesAsync()) {
            return Ok(product);
        }
        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await productRepo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        
        productRepo.DeleteProduct(product);
        if (await productRepo.SaveChangesAsync()) {
            return Ok();
        }
        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brands = await productRepo.GetBrandsAsync();
        return Ok(brands);
    } 

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var types = await productRepo.GetTypesAsync();
        return Ok(types);
    } 

    private bool ProductExists(int id)
    {
        return productRepo.ProductExists(id);
    }
}
