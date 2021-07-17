using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projeto.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        MySqlConnection connection = new MySqlConnection ( "DATABASE=PROJETO; port=3306; SERVER=localhost; UID=root; pwd=deidara337;" );

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
            try
            {

                //connection.Open ( );

                //connection.Close ( );
                return View ( );
            }
            catch (Exception ex)
            {
                return View ( );
            }
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
