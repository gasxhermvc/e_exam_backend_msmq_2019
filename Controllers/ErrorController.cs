using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace e_exam_backend_msmq_2019.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [Route("")]
        public ActionResult Index()
        {
            return Redirect("http://www.google.com/");
        }
    }
}