using System.Collections.Generic;
using System.Linq;
using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework
{
    public class EfWishlistDal : GenericRepository<Wishlist>, IWishlistDal
    {
        private readonly AppDbContext _context;

        public EfWishlistDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Wishlist> GetCartItemsByUserId(int userId)
        {
            return _context.Wishlist
                .Where(c => c.user_id == userId)
                .ToList();
        }
    }
}