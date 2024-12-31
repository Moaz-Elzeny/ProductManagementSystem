using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Application.Interfaces;
using ProductManagementSystem.Application.Products.Dtos;

namespace ProductManagementSystem.Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<ResponseDto>
    {
        public int PageNumber { get; set; }
        public string? SearchKeyword { get; set; }
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = 10;

            var query = _context.Products.AsQueryable();

            // Filter by search keyword if provided
            if (!string.IsNullOrWhiteSpace(request.SearchKeyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(request.SearchKeyword) ||
                    p.Description.Contains(request.SearchKeyword));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var products = await query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    CreationDate = p.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync(cancellationToken);

            var paginatedList = new PaginatedList<ProductListDto>
            {
                Data = products,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return ResponseDto.Success(new ResultDto { Result = paginatedList });
        }
    }
}
