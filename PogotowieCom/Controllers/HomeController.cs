using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;


namespace PogotowieCom.Controllers
{
    public class HomeController : Controller
    {

        IRepository repository;

        public HomeController(IRepository repo)
        {
            repository = repo;
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}