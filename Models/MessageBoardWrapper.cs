using System;
using System.Collections.Generic;

namespace MessageBoard.Models
{
    public class MessageBoardWrapper
    {
        public User LoggedUser { get; set; }
        public List<User> AllUsers { get; set; }
        public List<Message> AllMessages { get; set; }
        public List<Comment> AllComments { get; set; }

        public Message Msg { get; set; }
        public Comment Cmt { get; set; }
    }

}