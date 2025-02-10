using Microsoft.EntityFrameworkCore;

namespace MyWeb_MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }
    }
}
