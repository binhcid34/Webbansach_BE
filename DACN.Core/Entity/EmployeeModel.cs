using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DACN.Core.Entity
{
    public class Employee
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        
        public string Email { get; set; }

        public string CurrentLevel { get; set; }

        
        public int DepartmentID { get; set; }  
        public string Phone { get; set;}

        // Mã số nhân viên  = mã Department + số thứ tự
        public string EmployeeCode { get; set; }


        
            

    }
}
