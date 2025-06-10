using BitirmeProjesi.BusinessLayer.Abstract;
using BitirmeProjesi.DataAccessLayer.Abstract;


namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class OrderDetailsManager : IOrderDetailsService
    {
        private readonly IOrderDetailsDal _orderDetailDal;

        public OrderDetailsManager(IOrderDetailsDal orderDetailDal)
        {
            _orderDetailDal = orderDetailDal;
        }

        public void TAdd(Order_Details detail)
        {
            _orderDetailDal.Insert(detail);
        }

        public void TDelete(Order_Details detail)
        {
            _orderDetailDal.Delete(detail);
        }

        public void TUpdate(Order_Details detail)
        {
            _orderDetailDal.Update(detail);
        }

        public Order_Details TGetByID(int id)
        {
            return _orderDetailDal.GetById(id);
        }

        public List<Order_Details> TGetList()
        {
            return _orderDetailDal.GetList();
        }

        public List<Order_Details> GetDetailsByOrderId(int orderId)
        {
            return _orderDetailDal.GetList(d => d.order_id == orderId);
        }
    }
}