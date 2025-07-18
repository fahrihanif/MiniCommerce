using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Services.Categories.CreateCategory;
using MiniCommerce.API.Services.Categories.DeleteCategory;
using MiniCommerce.API.Services.Categories.GetAllCategory;
using MiniCommerce.API.Services.Categories.GetByIdCategory;
using MiniCommerce.API.Services.Categories.UpdateCategory;

namespace MiniCommerce.API.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoryController(ISender sender) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Update(UpdateCategoryCommand command)
    {
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return NotFound(result.Error);
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await sender.Send(command);
        
        if (result.IsFailure)
            return NotFound(result.Error);
        
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        await sender.Send(command);
        
        return Ok();
    }

    
    /// <summary>
    /// Get all category data.
    /// </summary>
    /// <remarks>
    /// This endpoint allows to Get all Data
    /// </remarks>
    /// <returns>Category Data</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCategoryQuery();
        var result = await sender.Send(query);
        
        if (result.IsFailure)
            return NotFound(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdCategoryQuery(id);
        var result = await sender.Send(query);
        
        if (result.IsFailure)
            return NotFound(result.Error);
        
        return Ok(result.Value);
    }
}