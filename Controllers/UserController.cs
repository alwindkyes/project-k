using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace project_k.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    Person[] users = new Person[]
    {
        new Person { Id = 1, Name = "Carmelo Hayes", Email = "carmelo@hayes.com", Mobile = 8967563412 }
    };
    private static int count = 0;

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = users.FirstOrDefault(users => users.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(users);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] Person user)
    {
        user.Id = count + 1;
        users[count++] = user;
        return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult EditUser([FromBody] Person user, int id)
    {
        var index = Array.FindIndex(users, user => user.Id == id);
        users[index].Name = user.Name;
        users[index].Email = user.Email;
        users[index].Mobile = user.Mobile;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var index = Array.FindIndex(users, user => user.Id == id);
        users[index] = null;
        return NoContent();
    }
}