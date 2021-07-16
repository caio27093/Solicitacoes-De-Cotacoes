using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController ( ILogger<HomeController> logger )
        {
            _logger = logger;
        }

        public IActionResult Index ( )
        {
            return View ( );
        }


        public IActionResult Home ( )
        {
            return View ( );
        }

        public IActionResult Lista ( )
        {
            return View ( );
        }
        public IActionResult Upload ( )
        {
            return View ( );
        }
        public IActionResult CadLogin ( )
        {
            return View ( );
        }


    }
}
