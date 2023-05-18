using Microsoft.AspNetCore.Mvc;
using TrackSpense.BL.Contracts;
using TrackSpense.DAL.Models;
using TrackSpense.Shared.BusinessModels;

namespace TrackSpense.Server.Controllers;

    [ApiController]
    [Route("[controller]")]
public class CategoryController :Controller
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpGet("GetCategories")]
    public async Task<List<Business_Category>> Get(string UserId)
    {
        return await _categoryService.Get(UserId);
    }
    [HttpPost("AddCategory")]
    public async Task<Business_Category> Add(Business_Category category)
    {
        return await _categoryService.Add(category);
    }
    [HttpDelete("DeleteCategory")]
    public async Task<Business_Category> Delete(Business_Category category)
    {
        return await _categoryService.Delete(category);
    }


}
