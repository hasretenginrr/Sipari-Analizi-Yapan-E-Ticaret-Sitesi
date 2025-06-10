using System.Linq.Expressions;
using DataAccessLayer.Abstract;

namespace BitirmeProjesi.DataAccessLayer.Abstract;

public interface IOrderDal :IGenericDal<Orders>
{
    List<Orders> GetList(Expression<Func<Orders, bool>> filter);
}