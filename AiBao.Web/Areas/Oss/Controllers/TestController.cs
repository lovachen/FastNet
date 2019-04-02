using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AiBao.Web.Areas.Oss.Controllers
{
    [Area("oss")]
    [Route("oss/test")]
    public class TestController : Controller
    {

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
         
    }
}