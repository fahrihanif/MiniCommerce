using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Services.Categories.CreateCategory;
using MiniCommerce.API.Services.Categories.DeleteCategory;
using MiniCommerce.API.Services.Categories.GetAllCategory;
using MiniCommerce.API.Services.Categories.GetByIdCategory;
using MiniCommerce.API.Services.Categories.UpdateCategory;

namespace MiniCommerce.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController(ISender sender) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Handle(UpdateCategoryCommand command)
    {
        await sender.Send(command);
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        await sender.Send(command);
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand command)
    {
        var result = await sender.Send(command);
        if (result.IsFailure)
            return BadRequest();
        
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCategoryQuery();
        var result = await sender.Send(query);
        
        if (!result.Any())
            return NotFound();
        
        return Ok(result);
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