using System.Linq.Expressions;
using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework;

public class EfOrderDetailsDal : GenericRepository<Order_Details> , IOrderDetailsDal
{
    private readonly AppDbContext _context;

    public EfOrderDetailsDal(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public List<Order_Details> GetList(Expression<Func<Order_Details, bool>> filter)
    {
        return _context.Order_Details.Where(filter).ToList();
    }
}