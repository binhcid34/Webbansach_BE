using DACN.Core.Entity;
using DACN.Core.IRepository;
using DACN.Core.IService;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace DACN.Infrastructure.Service
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private IEmployeeRepository _IEmployeeRepository;
        private IDepartmentRepository _IDepartmentRepository;

        public EmployeeService(IBaseRepository<Employee> baseRepository, IEmployeeRepository iEmployeeRepository, IDepartmentRepository iDepartmentRepository) : base(baseRepository)
        {
            _IEmployeeRepository = iEmployeeRepository;
            _IDepartmentRepository = iDepartmentRepository;
        }

        public dynamic filter(string filter, int pageNumber, int pageSize)
        {
            // validate
            return _IEmployeeRepository.filter(filter,pageNumber, pageSize); 
        }

        public override void Insert(Employee newEmployee)
        {
            // validate employee




            // insert
            _IEmployeeRepository.Insert(newEmployee);
            
        }

        public dynamic sortByList(ResponseModel response, string listSort)
        {
            // validate listSort
            if(String.IsNullOrEmpty(listSort))
            {
                response.Status = 401;
                response.Message= "Danh sách sắp xếp trống";
                return response;
            }
            List<string> resultSort = listSort.Split(',').ToList();
            // ghép câu query

            var propertyInfolist = "";
            foreach (var propertyInfo in typeof(Employee).GetProperties())
            {
                propertyInfolist += propertyInfo.Name + ",";
            }
            var isCheck = true;
            foreach (var itemSort in resultSort)
            {
                isCheck = propertyInfolist.Contains(itemSort);
            }
            if (!isCheck)
            {
                response.Status = 401;
                response.Message = "Danh sách chứa cột không có trong danh sách";
                return response;
            }
            else
            {
                response.Status = 200;
                response.Data = _IEmployeeRepository.sortByList(listSort);
                return response;
            }
            
        }


        public dynamic insertNewEmployee(ResponseModel response, Employee newEmployee, int type)
        {
            // validate



            // Tạo ra dữ liệu
            var parameters = new DynamicParameters();
            var jsonEmployee = System.Text.Json.JsonSerializer.Serialize(newEmployee);
            var data = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(jsonEmployee);
            // add to pram

            foreach (PropertyInfo propertyInfo in newEmployee.GetType().GetProperties())
            {
                var itemProperty = propertyInfo.Name.ToString();
                var valueItem = data[itemProperty].ToString();
                if(itemProperty != "DepartmentID" || itemProperty != "DoB")
                {
                    parameters.Add($"v_{itemProperty}",
                                  valueItem);
                }

                // do stuff here
            }
            // case TH các param riêng với kiểu dữ liệu khác string
            parameters.Add("v_DepartmentID", newEmployee.DepartmentID);
            parameters.Add("v_DoB", newEmployee.DoB);


            if (type == 1)
            _IEmployeeRepository.insertNewEmployee(parameters);
            else _IEmployeeRepository.updateEmployee(parameters);
            
            return response;
        }


        public dynamic importExcel(ResponseImportModel response, IFormFile fileImport)
        {
            response.Message = "";
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (!Path.GetExtension(fileImport.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                response.Message = "File không đúng định dạng";
                response.Status = 400;
                return response;
            }
            response.listEmployeesInsert = new List<object>();
            response.listEmployeesUpdate = new List<object>();
            response.listEmployeesError = new List<object>();
            // Lấy danh sách phòng ban
            var lisDepartment = _IDepartmentRepository.getAllDepartment();
            using (var stream = new MemoryStream())
            {
                fileImport.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    response.TotalRow = rowCount - 3;
                    for (int row = 2; row < rowCount - 1; row++)
                    {
                        bool isValidRow = true;
                        string messageError = "";
                        var employeeCode = ValidateString(worksheet.Cells[row, 2].Value);
                        
                        var empName = ValidateString(worksheet.Cells[row,3].Value);

                        var email = ValidateString(worksheet.Cells[row, 4].Value);

                        var currentLevel = ValidateString(worksheet.Cells[row, 5].Value);

                        var departmentName = ValidateString(worksheet.Cells[row, 6].Value);

                        var phone = ValidateString(worksheet.Cells[row, 7].Value);


                        // Check xem employeCode đã có chưa 
                        
                        
                            var departmentID = -1;
                            
                            foreach(var item in lisDepartment)
                            {
                                if (item.DepartmentName.Equals(departmentName, StringComparison.OrdinalIgnoreCase))
                                {
                                    departmentID= item.DepartmentID;
                                    break;
                                }
                            }
                            // validate dữ liệu
                            if (departmentID == -1)
                            {
                            isValidRow = false;
                            messageError += "Phòng ban không hợp lệ;";
                            }
                            if (string.IsNullOrEmpty(employeeCode))
                            {
                                isValidRow = false;
                                messageError += "Mã nhân viên không được để trống;";
                            }

                            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
                            {
                                isValidRow = false;
                                messageError += "Email không hợp lệ;";
                            }
                        var id = Guid.NewGuid();
                            
                            Employee newEmployee = new Employee
                            {
                                ID = id.ToString(),
                                Name = empName,
                                CurrentLevel = currentLevel,
                                Email = email,
                                EmployeeCode = employeeCode,
                                DepartmentID = departmentID,
                                Phone = phone,

                            };
                           if (isValidRow)
                        {
                            // Nếu chưa có thì insert
                            // có thì update
                            if (!_IEmployeeRepository.checkExtendCode(employeeCode, email))
                            {
                                insertNewEmployee(new ResponseModel(), newEmployee, 1);
                                response.listEmployeesInsert.Add(newEmployee);
                            }
                            else
                            {
                                insertNewEmployee(new ResponseModel(), newEmployee, 2);
                                response.listEmployeesUpdate.Add(newEmployee);
                            }
                        } else
                        {
                            response.listEmployeesError.Add(newEmployee);
                            response.Message += $"Dòng {row}: {messageError};";
                        }
                           
                       
                        
                        // Làm mới list danh sách lỗi 


                    }
                }
            }
            if (response.Message != "")
            {
                response.Status = 400;
                return response;
            }
            else
            {
                response.Status = 200;
                return response;
            }
           
        }

        private string ValidateString(object a)
        {
            if (a != null)
            {
                return a.ToString().Trim();
            }
            else return string.Empty;
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
