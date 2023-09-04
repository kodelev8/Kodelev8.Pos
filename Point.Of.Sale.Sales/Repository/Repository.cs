using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Sales.Repository;

public class Repository : GenericRepository<Persistence.Models.Sale>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<IFluentResults<CrudResult<Persistence.Models.Sale>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var sales = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (sales is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Sale>>($"No Sale found with Id {request.EntityId}.");
        }

        sales.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Sale>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = sales,
        });
    }

    public async Task<IFluentResults<List<Persistence.Models.Sale>>> GetByTenantId(int Id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Sales.Where(t => t.TenantId == Id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result!);
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Sale>>> UpsertLineItem(UpsertSaleLineItem request, CancellationToken cancellationToken = default)
    {
        var newLine = NewLine(request);

        var result = await _dbContext.Sales.FirstOrDefaultAsync(t => t.Id == request.SaleId, cancellationToken);

        if (result is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Sale>>($"No Sale found with Id {request.SaleId}.");
        }

        if (!result.LineItems.Any())
        {
            result.LineItems.Add(newLine);
        }

        var lineItem = result.LineItems.FirstOrDefault(t => t.LineId == request.LineId);

        if (lineItem is null)
        {
            result.LineItems.Add(newLine);
        }
        else
        {
            lineItem.TenantId = request.TenantId;
            lineItem.ProductId = request.ProductId;
            lineItem.ProductName = request.ProductName;
            lineItem.ProductDescription = request.ProductDescription;
            lineItem.Quantity = request.Quantity;
            lineItem.UnitPrice = request.UnitPrice;
            lineItem.LineDiscount = request.LineDiscount;
            lineItem.LineTax = request.LineTax;
            lineItem.LineTotal = request.LineTotal;
            lineItem.Active = request.Active;
        }

        return ResultsTo.Something(new CrudResult<Persistence.Models.Sale>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = result,
        });
    }

    private static SaleLineItem NewLine(UpsertSaleLineItem request)
    {
        return new SaleLineItem
        {
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineDiscount = request.LineDiscount,
            Active = true,
            LineTax = request.LineTax,
            ProductDescription = request.ProductDescription,
            LineTotal = request.LineTotal,
        };
    }
}
