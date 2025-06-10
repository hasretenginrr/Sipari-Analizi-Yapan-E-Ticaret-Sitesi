using BitirmeProjesi.BusinessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.EntityFramework;

namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IOrderDal _orderDal;
        private readonly IOrderDetailsDal _orderDetailDal;

        public OrderManager(IOrderDal orderDal, IOrderDetailsDal orderDetailDal)
        {
            _orderDal = orderDal;
            _orderDetailDal = orderDetailDal;
        }

        public void TAdd(Orders t)
        {
            _orderDal.Insert(t);
        }

        public void TDelete(Orders t)
        {
            _orderDal.Delete(t);
        }

        public void TUpdate(Orders t)
        {
            _orderDal.Update(t);
        }

        public Orders TGetByID(int id)
        {
            return _orderDal.GetById(id);
        }

        public List<Orders> TGetList()
        {
            return _orderDal.GetList();
        }

        public int CreateOrder(int userId, List<Cart> cartItems)
        {
            var order = new Orders
            {
                user_id = userId,
                order_date = DateTime.Now
            };

            _orderDal.Insert(order); // order.Id otomatik atanır (serial)

            foreach (var item in cartItems)
            {
                var detail = new Order_Details
                {
                    order_id = order.id,
                    product_id = item.product_id,
                    quantity = item.quantity,
                };
                _orderDetailDal.Insert(detail);
            }

            return (int)order.id;
        }

        public List<Orders> GetOrdersByUser(int userId)
        {
            return _orderDal.GetList(o => o.user_id == userId);
        }
    }
}
