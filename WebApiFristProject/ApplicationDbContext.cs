using Microsoft.EntityFrameworkCore;
using WebApiFristProject.Model;

namespace WebApiFristProject
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
    }
}
