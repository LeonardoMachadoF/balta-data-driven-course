using DataDriven.Data;
using DataDriven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataDriven.Controllers;

[Route("products")]
public class ProductController : ControllerBase
{
    private DataContext _context;
    public ProductController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromQuery] int take = 3, [FromQuery] int skip = 0)
    {
        var products = await _context.Products.OrderByDescending(x => x.Price).Include(x => x.Category).AsNoTracking().Skip(skip).Take(take).ToListAsync();
        return products;
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) return NotFound(new { message = "Categoria não encontrada" });
        return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetByCategory(int id)
    {
        var products = await _context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.CategoryId == id).ToListAsync();
        if (products == null) return NotFound(new { message = "Categoria não encontrada!" });
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Product>>> Post([FromBody] Product model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            _context.Products.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível inserir a categoria!" });
        }
    }
}