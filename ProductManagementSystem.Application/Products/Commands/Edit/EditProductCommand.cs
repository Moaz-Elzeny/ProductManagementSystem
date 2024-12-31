using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Application.Helper;
using ProductManagementSystem.Application.Interfaces;
using ProductManagementSystem.Application.Products.Dtos;
using System;

namespace ProductManagementSystem.Application.Products.Commands.Edit
{
    public class EditProductCommand : IRequest<ResponseDto>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }

    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, ResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;


        public EditProductCommandHandler(IApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<ResponseDto> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return ResponseDto.Failure($"Product with ID {request.Id} not found.");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseDto.Success(new { Message = "Product updated successfully.", ProductId = product.Id });
        }
    }
}
