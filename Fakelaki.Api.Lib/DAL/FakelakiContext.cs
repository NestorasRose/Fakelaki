using Fakelaki.Api.Lib.Models;
using Microsoft.EntityFrameworkCore;

namespace Fakelaki.Api.Lib.DAL
{
    public class FakelakiContext : DbContext
    {

        public FakelakiContext(DbContextOptions<FakelakiContext> options) : base(options)
        {
        }

        public DbSet<Models.Fakelaki> Fakelakia { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
    }
}