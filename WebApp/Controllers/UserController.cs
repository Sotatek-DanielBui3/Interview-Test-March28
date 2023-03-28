using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppUserRequest user)
    {
        var res = await _userManager.CreateAsync(new()
        {
            Age = user.Age,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.Phone,
            UserName = user.Email
        });

        if (res.Succeeded)
        {
            return StatusCode(201, (await _userManager.FindByEmailAsync(user.Email!))?.Id);
        }

        return BadRequest(res.Errors);
    }


    [HttpPut("{id})")]
    public async Task<IActionResult> Update([Required] string id, [FromBody] AppUserRequest user)
    {
        var res = await _userManager.FindByIdAsync(id);
        if (res == null)
        {
            return NotFound();
        }

        res.PhoneNumber = user.Phone;
        res.FullName = user.FullName;
        res.Email = user.Email;
        res.Age = user.Age;

        var identityResult = await _userManager.UpdateAsync(res);

        if (identityResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(identityResult.Errors);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(string id)
    {
        var res = await _userManager.FindByIdAsync(id);
        if (res == null)
        {
            return NotFound();
        }

        var identityResult = await _userManager.DeleteAsync(res);
        if (identityResult.Succeeded)
        {
            return Ok();
        }

        return BadRequest(identityResult.Errors);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([Required] SearchUserRequest request)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase("MemoryBaseDataBase");
        using (var context = new ApplicationDbContext(optionsBuilder.Options))
        {
            if (request.SearchByEmail)
            {
                return Ok(await context.Users.Where(x => x.Email == request.Keyword).OrderBy(x => x.Email).ToListAsync());
            }

            Ok(await context.Users.Where(x => x.PhoneNumber == request.Keyword).OrderBy(x => x.PhoneNumber).ToListAsync());
        }

        return BadRequest();
    }
}
