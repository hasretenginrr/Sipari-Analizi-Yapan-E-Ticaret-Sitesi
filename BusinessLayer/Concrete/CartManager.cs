using BitirmeProjesi.BusinessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.Abstract;
using System.Collections.Generic;

namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class CartManager : ICartService
    {
        private readonly ICartDal _cartDal;

        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }

        public void TAdd(Cart t)
        {
            _cartDal.Insert(t);
        }

        public void TDelete(Cart t)
        {
            _cartDal.Delete(t);
        }

        public void TUpdate(Cart t)
        {
            _cartDal.Update(t);
        }

        public List<Cart> TGetList()
        {
            return _cartDal.GetList();
        }

        public Cart TGetByID(int id)
        {
            return _cartDal.GetById(id);
        }

        // Eğer Cart'a özel ekstra metot istersek buraya yazabiliriz.
        // Mesela: Kullanıcıya göre sepeti listelemek gibi
        public List<Cart> GetCartItemsByUserId(int userId)
        {
            return _cartDal.GetCartItemsByUserId(userId);
        }
    }
}