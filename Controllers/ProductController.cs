using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_k.Context;
using project_k.DataModel.ProductModels;

[ApiController]
public class ProductController : ControllerBase
{
    private readonly ProductDbContext _productDbContext;
    public ProductController(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateUserList([FromBody] User user)
    {
        await _productDbContext.Users.AddAsync(user);
        await _productDbContext.SaveChangesAsync();
        return Ok(user);
    }

    [HttpGet("products/users")]
    public async Task<IActionResult> GetUserList()
    {
        return Ok(await _productDbContext.Users.Include(item => item.Products).ToListAsync());
    }

    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        User user = await _productDbContext.Users.Include(item => item.Products).FirstOrDefaultAsync(item => item.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPut("products")]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        User userToUpdate = await _productDbContext.Users.Include(item => item.Products).FirstOrDefaultAsync(item => item.UserId == user.UserId);
        if (userToUpdate == null)
        {
            return NotFound();
        }
        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        if (userToUpdate.Products.Count > 0)
        {
            foreach (Product product in user.Products)
            {
                Product productToUpdate = await _productDbContext.Products.FirstOrDefaultAsync(item => item.ProductId == product.ProductId);
                productToUpdate.ProductName = product.ProductName;
                productToUpdate.Category = product.Category;
                productToUpdate.Price = product.Price;
            }
        }
        await _productDbContext.SaveChangesAsync();
        return Ok(new { message = "updated like", user = user });
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        User userToDelete = await _productDbContext.Users.Include(item => item.Products).FirstOrDefaultAsync(item => item.UserId == id);
        if (userToDelete == null)
        {
            return NotFound();
        }
        _productDbContext.Users.Remove(userToDelete);
        await _productDbContext.SaveChangesAsync();
        return Ok(new { user = userToDelete, message = "Deleted" });
    }

    [HttpDelete("products/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        Product productToDelete = await _productDbContext.Products.FirstOrDefaultAsync(item => item.ProductId == id);
        if (productToDelete == null)
        {
            return NotFound();
        }
        _productDbContext.Products.Remove(productToDelete);
        await _productDbContext.SaveChangesAsync();
        return Ok(new { product = productToDelete, message = "Deleted" });
    }

    // [HttpDelete("prodcuts/{isAll}")]
    // public async Task<IActionResult> DeleteUsers(List<int> idList, bool isAll)
    // {
    //     if (isAll)
    //     {
    //         List<User> allUsers = await _productDbContext.Users.Include(item => item.Products).ToListAsync();
    //         _productDbContext.Users.RemoveRange(allUsers);
    //         await _productDbContext.SaveChangesAsync();
    //         return Ok(new StatusResponse(200, "All Prodcuts are deleted"));
    //     }
    //     List<int> nullList = new List<int>();
    //     foreach (int id in idList)
    //     {
    //         User userToDelete = await _productDbContext.Users.Include(item => item.Products).FirstOrDefaultAsync(item => item.UserId == id);
    //         if (userToDelete == null)
    //         {
    //             nullList.Add(id);
    //             continue;
    //         }
    //         _productDbContext.Users.Remove(userToDelete);
    //         await _productDbContext.SaveChangesAsync();
    //     }
    //     if (nullList.Count == idList.Count)
    //     {
    //         return NotFound();
    //     }
    //     else if (nullList.Count > 0)
    //     {
    //         return Ok(new {nullList = nullList, deletedList = idList.Where(item => !nullList.Contains(item))});
    //     }
    //     return Ok(new StatusResponse(200, "Prodcuts are deleted"));
    // }

    [HttpPost("products/page")]
    public async Task<IActionResult> GetProductList(Page request)
    {
        var productListQuery = _productDbContext.Products.AsQueryable();
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            productListQuery = productListQuery.Where(item => item.ProductName.ToLower().StartsWith(request.Keyword.ToLower()));
        }
        return Ok(new { list = await productListQuery.Skip(request.Offset).Take(request.Limit).ToListAsync(), total = await productListQuery.CountAsync() });
    }
}