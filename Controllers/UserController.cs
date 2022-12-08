using DataDriven.Data;
using DataDriven.Models;
using DataDriven.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DataDriven.Controllers;

[Route("users")]
public class UserController : ControllerBase
{
    private DataContext _context;
    public UserController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> Get()
    {
        var users = await _context
            .Users
            .AsNoTracking()
            .ToListAsync();
        return users;
    }

    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    // [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Post([FromBody] User model)
    {
        // Verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            // Força o usuário a ser sempre "funcionário"
            model.Role = "employee";

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            // Esconde a senha
            model.Password = "";
            return model;
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível criar o usuário" });

        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Put(int id, [FromBody] User model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != model.Id)
            return NotFound(new { message = "Usuário não encontrada" });

        try
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model;
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível criar o usuário" });

        }
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Username == model.Username && x.Password == model.Password)
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound(new { message = "Usuário ou senha inválidos" });

        var token = TokenService.GenerateToken(user);
        // Esconde a senha
        user.Password = "";
        return new
        {
            user = user,
            token = token
        };
    }
}