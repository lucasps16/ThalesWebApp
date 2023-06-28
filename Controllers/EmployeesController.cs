using Microsoft.AspNetCore.Mvc;
using ThalesWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Mail;

namespace ThalesWebApp.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            //List<EmployeeModel> employees = GetEmployeeList().Result;
            return View();
        }


        public async Task<List<EmployeeModel>> GetEmployeeList()
        {
            try
            {
                List<EmployeeModel> employees = new List<EmployeeModel>();

                HttpClient client = new HttpClient();
                HttpResponseMessage res = await client.GetAsync("https://dummy.restapiexample.com/api/v1/employees");
                string jsonRes = await res.Content.ReadAsStringAsync();
                if (jsonRes.StartsWith("<"))
                {
                    throw new Exception("Too Many Requests, try again later...");
                }
                dynamic data = GetDynamicObj(jsonRes);
                foreach(dynamic item in data.data)
                {
                    EmployeeModel employee= new EmployeeModel();
                    employee.id = item.id;
                    employee.employee_name = item["employee_name"];
                    employee.employee_salary = item["employee_salary"];
                    employee.employee_anual_salry = CalculateAnualSalary(employee.employee_salary);
                    employee.employee_age = item["employee_age"];
                    employee.profile_image = item["profile_image"];
                    employees.Add(employee);
                }
                int a = 0;
                return employees;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<EmployeeModel>> GetEmployee(int id)
        {
            try
            {
                List<EmployeeModel> employees = new List<EmployeeModel>();

                HttpClient client = new HttpClient();
                HttpResponseMessage res = await client.GetAsync("https://dummy.restapiexample.com/api/v1/employee/"+id);
                string jsonRes = await res.Content.ReadAsStringAsync();
                if (jsonRes.StartsWith("<"))
                {
                    throw new Exception("Too Many Requests");
                }
                dynamic data = GetDynamicObj(jsonRes);
                if(data.data == null)
                {
                    throw new HttpRequestException();
                }
                EmployeeModel employee = new EmployeeModel();
                employee.id = data.data["id"];
                employee.employee_name = data.data["employee_name"];
                employee.employee_salary = data.data["employee_salary"];
                employee.employee_anual_salry = CalculateAnualSalary(employee.employee_salary);
                employee.employee_age = data.data["employee_age"];
                employee.profile_image = data.data["profile_image"];
                employees.Add(employee);
                int a = 0;
                return employees;
            }
            catch (HttpRequestException http_ex)
            {
                throw http_ex;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Search(int? id)
        {
            try
            {
                List<EmployeeModel> employees = new List<EmployeeModel>();
                if (id.HasValue)
                {
                    employees = GetEmployee(id.Value).Result;
                    if(employees == null)
                    {
                        return View("~/Views/Error/Index.cshtml");
                    }
                }
                else
                {
                    employees = GetEmployeeList().Result;

                }

                EmployeeListModel model = new EmployeeListModel
                {
                    Employees= employees
                };
                int a = 0;
                return View("EmployeeList", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View("~/Views/Error/Error404.cshtml");
            }
        }



        private dynamic GetDynamicObj(string json)
        {
            try
            {
                dynamic data = JObject.Parse(json);
                return data;

            }catch(Exception ex)
            {
                return null;
            }
        }

        public int CalculateAnualSalary(int salary)
        {
            int employee_anual_salary = salary * 12;
            return employee_anual_salary;
        }
    }
}
