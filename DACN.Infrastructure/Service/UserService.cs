using DACN.Core.Email;
using DACN.Core.Entity;
using DACN.Core.IRepository;
using DACN.Core.IService;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Infrastructure.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private IUserRepository _IUserRepository;
        private IMailService _IMailService;
        public UserService(IBaseRepository<User> baseRepository, IUserRepository iUserRepository, IMailService mailService) : base(baseRepository)
        {
            _IUserRepository = iUserRepository;
            _IMailService = mailService;
        }

        public void InsertUser(User user, ResponseModel response)
        {
            // validate
            if (!String.IsNullOrEmpty(user.Email))
            {
                var _user = _IUserRepository.getInfo(user.Email);
                if (_user == null)
                {
                    // insert
                    var parameters = new DynamicParameters();
                    parameters.Add("v_FullName", user.Fullname);
                    parameters.Add("v_Email", user.Email);
                    parameters.Add("v_Password", "12345678@Abc");
                    parameters.Add("v_IsAdmin", user.IsAdmin);
                    parameters.Add("v_CreatedBy", user.CreatedBy);
                    parameters.Add("v_Address1", user.Address1);
                    parameters.Add("v_Address2", user.Address2);
                    parameters.Add("v_Phone", user.Phone);

                    _IUserRepository.InsertUser(parameters);

                    response.Success = true;
                    response.Message = "Đã thêm thành công";
                    response.Status = 200;
                    return;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Đã tồn tại email";
                    response.Status = 301;
                    return;
                }

            };
            response.Success = false;
            response.Message = "Email không thể trống";
            response.Status = 302;
            return;

        }

        public bool RecoverPassword(string email)
        {
            // check xem có email đó không
            var userInfo = _IUserRepository.getInfo(email);
            if (userInfo == null)
            {
                return false;
            };
            bool isValid = true;
            // genarate password mới
            var newPass = GetRandomPassword(8);
            string content = $"Cấp lại mật khẩu";
            string body = $"Password mới của bạn là: {newPass}";
            //gửi email
            isValid =  _IMailService.SendMailAsync(email, content, body);
            // update password mới trong db
            _IUserRepository.ChangePassword(newPass, email);
            return isValid;
        }

        public void UpdateUser(User user, ResponseModel response)
        {
            // validate
            if (!String.IsNullOrEmpty(user.Email))
            {
                var _user = _IUserRepository.getInfo(user.Email);
                if (_user != null)
                {
                    // update
                    var parameters = new DynamicParameters();
                    parameters.Add("v_IdUser", user.IdUser);
                    parameters.Add("v_FullName", user.Fullname);
                    parameters.Add("v_Email", user.Email);
                    parameters.Add("v_Password", "12345678@Abc");
                    parameters.Add("v_IsAdmin", user.IsAdmin);
                    parameters.Add("v_Address1", user.Address1);
                    parameters.Add("v_Address2", user.Address2);
                    parameters.Add("v_Phone", user.Phone);
                    _IUserRepository.UpdateUser(parameters);

                    response.Success = true;
                    response.Message = "Đã chỉnh sửa thành công";
                    response.Status = 200;
                    return;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Tài khoản không đúng";
                    response.Status = 301;
                    return;
                }

            };
            response.Success = false;
            response.Message = "Email không thể trống";
            response.Status = 302;
            return;

        }
        private string GetRandomPassword(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
    }
}
