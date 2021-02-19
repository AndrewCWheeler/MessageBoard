using System;
using System.Collections.Generic;

namespace MessageBoard.Models
{
    public class MessageBoardWrapper
    {
        public User LoggedUser { get; set; }
        public List<User> AllUsers { get; set; }
    }

}