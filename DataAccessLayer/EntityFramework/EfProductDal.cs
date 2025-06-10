using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Products>, IProductDal
    {
        public EfProductDal(AppDbContext context) : base(context)
        {
        }
    }
}
