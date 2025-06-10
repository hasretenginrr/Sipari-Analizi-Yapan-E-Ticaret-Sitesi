using System.Linq.Expressions;
using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework;

public class EfOrderDal : GenericRepository<Orders> , IOrderDal
{
    private readonly AppDbContext _context;

    public EfOrderDal(AppDbContext context) : base(context)
    {
        _context = context;
    }
    
    public List<Orders> GetList(Expression<Func<Orders, bool>> filter)
    {
        return _context.Orders.Where(filter).ToList();
    }
}