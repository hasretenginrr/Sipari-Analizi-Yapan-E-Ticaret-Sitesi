using BitirmeProjesi.DataAccessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Repository;

namespace BitirmeProjesi.DataAccessLayer.EntityFramework
{
    public class EfUserDal : GenericRepository<Users>, IUserDal
    {
        public EfUserDal(AppDbContext context) : base(context)
        {
        }
    }
}
