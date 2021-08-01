using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EatTogether.Controllers
{
    public class ValidationController : Controller
    {
        [HttpPost]
        public JsonResult IsValidDate(DateTime Date)
        {
            var max = DateTime.Today.AddYears(2);
            var min = DateTime.Today;
            var msg = string.Format("Please enter a valid date.");
            var msg2 = string.Format("Exception Thrown. Please enter a valid date.");
            try
            {
                var date = Date;
                if (date < min || date > max)
                    return Json(msg);
                else
                    return Json(true);
            }
            catch (Exception)
            {
                return Json(msg2);
            }
        }
    }
}