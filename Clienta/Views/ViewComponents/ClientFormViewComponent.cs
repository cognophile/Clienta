using Microsoft.AspNetCore.Mvc;
using Clienta.Models;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Views.ViewComponents
{
    [ViewComponent]
    public class ClientFormViewComponent : ViewComponent
    {
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// ClientFormViewComponent constructor
        /// </summary>
        /// <param name="employeeService">IEmployeeService</param>
        public ClientFormViewComponent(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Populates the Employee ViewBag property with a SelectList of Employees
        ///     and returns the ViewComponent
        /// </summary>
        /// <param name="action">string - Controller action to invoke from ViewComponent Form</param>
        /// <param name="model">Client - Plain Client model for the ViewComponent</param>
        /// <returns>IViewComponentResult</returns>
        public async Task<IViewComponentResult> InvokeAsync(string action, Client model)
        {            
            ViewData["Action"] = action;
            ViewBag.Employees = await _employeeService.GetEmployeeSelectListAsync();
            return View(model);
        }
    }
}
