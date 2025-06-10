using System.Collections.Generic;
using System.Linq;
using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework
{
    public class EfCartDal : GenericRepository<Cart>, ICartDal
    {
        private readonly AppDbContext _context;

        public EfCartDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Cart> GetCartItemsByUserId(int userId)
        {
            return _context.Cart
                .Where(c => c.user_id == userId)
                .ToList();
        }
    }
}