using BitirmeProjesi.BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using System.Collections.Generic;

namespace BitirmeProjesi.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal) // Bağımlılık enjeksiyonu
        {
            _userDal = userDal;
        }

        public void TAdd(Users t)
        {
            _userDal.Insert(t);
        }

        public void TDelete(Users t)
        {
            _userDal.Delete(t);
        }

        public void TUpdate(Users t)
        {
            _userDal.Update(t);
        }

        public List<Users> TGetList()
        {
            return _userDal.GetList();
        }

        public Users TGetByID(int id)
        {
            return _userDal.GetById(id);
        }
    }
}
