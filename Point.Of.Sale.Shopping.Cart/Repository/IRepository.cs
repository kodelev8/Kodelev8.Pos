using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public interface IRepository : IGenericRepository<ShoppingCart>
{
    Task<IFluentResults<CrudResult<ShoppingCart>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<ShoppingCart>>> GetByTenantId(int request, CancellationToken cancellationToken = default);
}