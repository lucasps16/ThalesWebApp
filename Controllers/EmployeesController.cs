using Microsoft.AspNetCore.Mvc;
using ThalesWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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
                dynamic data = GetDynamicObj(jsonRes);
                foreach(dynamic item in data.data)
                {
                    EmployeeModel employee= new EmployeeModel();
                    employee.id = item.id;
                    employee.employee_name = item["employee_name"];
                    employee.employee_salary = item["employee_salary"];
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
                return new List<EmployeeModel>();
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
                    throw new Exception();
                }
                dynamic data = GetDynamicObj(jsonRes);
                foreach (dynamic item in data)
                {
                    EmployeeModel employee = new EmployeeModel();
                    employee.id = item.id;
                    employee.employee_name = item["employee_name"];
                    employee.employee_salary = item["employee_salary"];
                    employee.employee_age = item["employee_age"];
                    employee.profile_image = item["profile_image"];
                    employees.Add(employee);
                }
                int a = 0;
                return employees;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Error();
                return new List<EmployeeModel>();
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
                }
                else
                {
                    employees = GetEmployeeList().Result;

                }
               /* string jsonRes = await res.Content.ReadAsStringAsync();
                dynamic data = GetDynamicObj(jsonRes);
                foreach (dynamic item in data.data)
                {
                    EmployeeModel employee = new EmployeeModel();
                    employee.id = item.id;
                    employee.employee_name = item["employee_name"];
                    employee.employee_salary = item["employee_salary"];
                    employee.employee_anual_salry = CalculateAnualSalary(employee.employee_salary);
                    employee.employee_age = item["employee_age"];
                    employee.profile_image = item["profile_image"];
                    employees.Add(employee);
                }*/

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
                return View("Index");
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
