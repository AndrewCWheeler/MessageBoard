using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// Allows us to reference this class to instantiate instances of models:
using MessageBoard.Models;
// Necessary for using session:
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
// Necessary for password hashing:
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
// Necessary for connecting to our dbContext -> then to our database:
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class MessageBoardController : Controller
    {
        private MessageBoardContext _context;
        public MessageBoardController(MessageBoardContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Registration");
            }

            // User LoggedUser = _context.User.FirstOrDefault(u => u.UserId == (int)LoggedId);
            MessageBoardWrapper MBWrap = new MessageBoardWrapper()
            {
                AllMessages = _context.Messages
                    .Include(m => m.Creator)
                    .Include(children => children.ChildrenComments)
                    .ThenInclude(c => c.Creator)
                    .OrderByDescending(d => d.CreatedAt)
                    .ToList(),
            };
            return View("Dashboard", MBWrap);
        }

        [HttpPost]
        // lowercase "message" as parameter indicates the message obj from the form
        public IActionResult CreateMessage(MessageBoardWrapper message)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Registration");
            }
            User LoggedUser = _context.User.FirstOrDefault(u => u.UserId == (int)LoggedId);
            message.Msg.UserId = LoggedUser.UserId;
            _context.Messages.Add(message.Msg);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpGet("delete/{MessageId}")]
        public RedirectToActionResult DeleteMessage(int MessageId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Registration");
            }
            Message ToDelete = _context.Messages.FirstOrDefault(m => m.MessageId == MessageId);

            if(ToDelete == null || ToDelete.UserId != (int)LoggedId)
            {
                return RedirectToAction("Dashboard");
            }
            _context.Remove(ToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("messageboard/createcomment/{MessageId}")]

        public IActionResult CreateComment(MessageBoardWrapper comment, int MessageId)
        {
            int? LoggedId = HttpContext.Session.GetInt32("UserId");
            if(LoggedId == null)
            {
                return RedirectToAction("Registration");
            }
            User LoggedUser = _context.User.FirstOrDefault(u => u.UserId == (int)LoggedId);
            comment.Cmt.UserId = LoggedUser.UserId;
            comment.Cmt.MessageId = MessageId;
            _context.Comments.Add(comment.Cmt);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");

            
        }
    }
}