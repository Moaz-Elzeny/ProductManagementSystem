using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Application.Products.Commands.Create;
using ProductManagementSystem.Application.Products.Commands.Delete;
using ProductManagementSystem.Application.Products.Commands.Edit;
using ProductManagementSystem.Application.Products.Dtos;
using ProductManagementSystem.Application.Products.Queries;

namespace ProductManagementSystem.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllExpenses")]
        public async Task<IActionResult> GetAllExpenses([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var validationResults = await new CreateProductCommandValidator().ValidateAsync(command);

            if (!validationResults.IsValid)
            {
                var errors = validationResults.Errors.Select(r => new { Key = r.PropertyName, Value = r.ErrorMessage }).ToList();

                var result = new ResultDto
                {
                    Result = "Invalid value for AboutExploreTour.",
                };

                return BadRequest(result);
            }
            var response = await _mediator.Send(command);
            return !response.IsSuccess ? BadRequest(response.Message) : Ok(response.Data);
        } 
        
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(int Id, EditProductDto dto)
        {
            var command = new EditProductCommand
            {
                Id = Id,    
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };

            var validationResults = await new EditProductCommandValidator().ValidateAsync(command);

            if (!validationResults.IsValid)
            {
                var errors = validationResults.Errors.Select(r => new { Key = r.PropertyName, Value = r.ErrorMessage }).ToList();

                var result = new ResultDto
                {
                    Result = "Invalid value for AboutExploreTour.",
                };

                return BadRequest(result);
            }
            var response = await _mediator.Send(command);
            return !response.IsSuccess ? BadRequest(response.Message) : Ok(response.Data);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var response = await _mediator.Send(new DeleteProductCommand { Id = Id });
            return !response.IsSuccess ? NotFound(response.Message) : Ok(response.Data);
        }
    }
}
