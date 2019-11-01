using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PogotowieCom.Models;

namespace PogotowieCom.Controllers
{
    public class CommentController : Controller
    {
        private IRepository repository;
        private UserManager<AppUser> userManager;
        public CommentController(IRepository repository,  UserManager<AppUser> userManager)
        {
            this.userManager=userManager;
            this.repository = repository;
        }


        public ViewResult AddCommentAndVote()
        {

             AppUser user=  userManager.GetUserAsync(HttpContext.User).Result;
           
            CommentData data = repository.CommentAndVoteCheck(user);


            AddVoteCommentViewModel model = new AddVoteCommentViewModel();
            model.Comment.AppointmentId = data.appointment.AppointmentId;
            model.Comment.CommentAvailable = (DateTime)data.appointment.AppointmentEnd;
            model.Comment.CommentDate = DateTime.Now;
            model.Comment.Done = false;
            model.Comment.Points = 0;
            model.Comment.PatientEmail = user.Email;
            model.Comment.PatientId = (int)user.PatientId;

            AppUser doctor = repository.GetUserByDoctorId((int)data.appointment.DoctorId);
            model.Message=$"Prosimy o komentarz i ocenę dotyczącą wizyty u {doctor.UserName + " "+doctor.Surname} z dnia {((DateTime)data.appointment.AppointmentEnd).ToShortDateString() }";


            return View(model);
        }

        [HttpPost]
        public IActionResult AddCommentAndVote(AddVoteCommentViewModel model)
        {
            if (String.IsNullOrEmpty(model.Comment.Text))
                {
                ModelState.AddModelError("Comment.Text", "Komentarz nie może być pusty");
            }

            if(ModelState.IsValid)
            {

                bool succes = repository.ChangeComment(model.Comment);

                if(succes)
                {
                    return RedirectToAction("UsersPanel", "Home");
                }
                else
                {
                    return View("Error");
                }
               

            }
            else
            {
                return View(model);
            }

            
        }

        public IActionResult ShowComments(string UserId)
        {
           ShowCommentsViewModel commentList=repository.GetCommentsAndDoctorData(UserId);

            return View(commentList);
        }

        public IActionResult DoctorRank()
        {
            List<DoctorRankViewModel> model = repository.GetCommentDetails();

            return View(model);
        }
    }
}