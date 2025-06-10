using BitirmeProjesi.BusinessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.Abstract;
using System.Collections.Generic;

namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class WishlistManager : IWishlistService
    {
        private readonly IWishlistDal _wishlistDal;

        public WishlistManager(IWishlistDal wishlistDal)
        {
            _wishlistDal = wishlistDal;
        }

        public void TAdd(Wishlist t)
        {
            _wishlistDal.Insert(t);
        }

        public void TDelete(Wishlist t)
        {
            _wishlistDal.Delete(t);
        }

        public void TUpdate(Wishlist t)
        {
            _wishlistDal.Update(t);
        }

        public List<Wishlist> TGetList()
        {
            return _wishlistDal.GetList();
        }

        public Wishlist TGetByID(int id)
        {
            return _wishlistDal.GetById(id);
        }

        // Eğer Cart'a özel ekstra metot istersek buraya yazabiliriz.
        // Mesela: Kullanıcıya göre sepeti listelemek gibi
        public List<Wishlist> GetCartItemsByUserId(int userId)
        {
            return _wishlistDal.GetCartItemsByUserId(userId);
        }
    }
}