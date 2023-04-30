using DACN.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Email
{
    public interface IMailService
    {
        bool SendMailAsync(string SendMailTo, string SendMailSubject, string SendMailBody);
    }
}
