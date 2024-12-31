using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductManagementSystem.Application.Helper;
using ProductManagementSystem.Application.Interfaces;
using ProductManagementSystem.Application.Products.Dtos;
using ProductManagementSystem.Domain.Entity;
using System;

namespace ProductManagementSystem.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<ResponseDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        public CreateProductCommandHandler(IApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<ResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CreationDate = DateTime.Now,
                ImageUrl = request.ImageUrl
            };

            //product.ImageUrl = await FileHelper.SaveImageAsync(request.ImageUrl, _environment);


            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            // Return success response
            return ResponseDto.Success(new { product.Id, Message = "Product created successfully." });
        }
    }
}
