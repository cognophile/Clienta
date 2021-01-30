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
    public class EmployeeFormViewComponent : ViewComponent
    {
        /// <summary>
        /// EmployeeFormViewComponent constructor
        /// </summary>
        public EmployeeFormViewComponent()
        {
        }

        /// <summary>
        /// Invoke the ViewComponent
        /// </summary>
        /// <param name="action">string - Controller action to invoke from ViewComponent Form</param>
        /// <param name="model">Employee - Plain Employee model for the ViewComponent</param>
        /// <returns>IViewComponentResult</returns>
        public IViewComponentResult Invoke(string action, Employee model)
        {            
            ViewData["Action"] = action;
            return View(model);
        }
    }
}
