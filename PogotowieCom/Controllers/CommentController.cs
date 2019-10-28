using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;

namespace PogotowieCom.Controllers
{
    public class CommentController : Controller
    {
        private IRepository repository;

        public CommentController(IRepository repository)
        {
            this.repository = repository;
        }


        public IActionResult DoctorRank()
        {
            List<DoctorRankViewModel> model = repository.GetCommentDetails();

            return View();
        }
    }
}