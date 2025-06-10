using DataAccessLayer.Abstract;

namespace BitirmeProjesi.DataAccessLayer.Abstract
{
    public interface IWishlistDal : IGenericDal<Wishlist>
    {
        List<Wishlist> GetCartItemsByUserId(int userId);
    }
}