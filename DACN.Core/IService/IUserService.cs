using DACN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IService
{
    public interface IUserService
    {
        public void InsertUser(User user, ResponseModel response);

        public void UpdateUser(User user, ResponseModel response);

        public bool RecoverPassword(string email);
    }
}
