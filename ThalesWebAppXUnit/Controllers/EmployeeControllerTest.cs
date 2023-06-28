using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ThalesWebApp.Controllers;
using ThalesWebApp.Models;
using Xunit;

namespace ThalesWebAppXUnit.Controllers
{
    public class EmployeeControllerTest
    {
        private EmployeesController _employeeController;

        public EmployeeControllerTest() 
        {
            _employeeController = new EmployeesController();
        }
        [Fact]
        public void EmployeeController_Index_ReturnsSuccess()
        {
            //Arrange

            //Act
            var result = _employeeController.Index();
            //Assert
            result.Should().BeOfType<ViewResult>();
        }
        //Test might fail if API is overloaded and returns error "Too Many requests"
        [Fact]
        public void EmployeeController_GetEmployee_ReturnsSucces() 
        {
            //Arrange
            int id = 1;
            //Act
            var result = _employeeController.GetEmployee(id).Result;
            //Assert
            result.Should().BeOfType<List<EmployeeModel>>();

        }
        //Test might fail if API is overloaded and returns error "Too Many requests"

        [Fact]
        public void EmployeeController_GetEmployee_ReturnsHttpExc()
        {
            //Arrange
            int id = 9999;
            //Act
            var result = Assert.Throws<HttpRequestException>(()=> _employeeController.GetEmployee(id).Result);
            //Assert
            Assert.Equal("404",result.Message);

        }
        //Test might fail if API is overloaded and returns error "Too Many requests"

        [Fact]
        public void EmployeeController_GetEmployeeList_ReturnsSucces()
        {
            //Arrange

            //Act
            var result = _employeeController.GetEmployeeList().Result;
            //Assert 
            result.Should().BeOfType<List<EmployeeModel>>();

        }


    }
}
