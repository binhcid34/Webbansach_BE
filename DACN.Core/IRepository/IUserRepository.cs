using DACN.Core.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.IRepository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public User validateUser(string email, string pass);

        public void InsertUser(DynamicParameters parameters);

        public void UpdateUser(DynamicParameters parameters);

        public User getInfo(string email);

        public void DeleteUser(string IdUser);

        public void DeleteAll();

        public IEnumerable<User> searchByEmail(string search);

        public void ChangePassword(string newPass, string email);

        public User getInfoFromSSID(string ssid);
    }
}
