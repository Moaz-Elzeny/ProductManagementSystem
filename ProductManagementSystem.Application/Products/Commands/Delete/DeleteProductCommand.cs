using MediatR;
using ProductManagementSystem.Application.Interfaces;
using ProductManagementSystem.Application.Products.Dtos;

namespace ProductManagementSystem.Application.Products.Commands.Delete
{
    public class DeleteProductCommand : IRequest<ResponseDto>
    {
        public int Id { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product == null)
            {
                return ResponseDto.Failure($"Product with ID {request.Id} not found.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return ResponseDto.Success(new { Message = "Product deleted successfully." });
        }
    }
}