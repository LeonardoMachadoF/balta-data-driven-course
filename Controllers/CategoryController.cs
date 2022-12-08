using DataDriven.Data;
using DataDriven.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("categories")]
public class CategoryController : ControllerBase
{
    private DataContext _context;
    public CategoryController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> Get([FromQuery] int take = 3, [FromQuery] int skip = 0)
    {
        var categories = await _context.Categories.AsNoTracking().Skip(skip).Take(take).ToListAsync();
        return categories;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (category == null) return NotFound(new { message = "Categoria não encontrada" });
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<List<Category>>> Post([FromBody] Category model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            _context.Categories.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível inserir a categoria!" });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (model.Id != id) return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            _context.Entry<Category>(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel atualizar o registro!" });
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null) return NotFound(new { message = "Não foi possivel encontrar o registro!" });

        try
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso!" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel deletar o registro!" });
        }
    }
}