using System.Linq.Expressions;
using DataAccessLayer.Abstract;

namespace BitirmeProjesi.DataAccessLayer.Abstract;

public interface IOrderDetailsDal :IGenericDal <Order_Details>
{
    List<Order_Details> GetList(Expression<Func<Order_Details, bool>> filter);
}