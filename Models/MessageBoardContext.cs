using Microsoft.EntityFrameworkCore;
using MessageBoard.Models;

namespace MessageBoard.Data
{
    public class MessageBoardContext : DbContext
    {
        public MessageBoardContext (DbContextOptions<MessageBoardContext> options)
        : base(options) {}
        public DbSet<User> User { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}