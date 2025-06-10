using DataAccessLayer.Abstract;

namespace BitirmeProjesi.DataAccessLayer.Abstract
{
    public interface ICartDal : IGenericDal<Cart>
    {
        List<Cart> GetCartItemsByUserId(int userId);
    }
}