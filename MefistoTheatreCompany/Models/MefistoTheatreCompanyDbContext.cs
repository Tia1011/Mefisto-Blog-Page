// Tiani Perera - Mefisto Theatre Company 03/02/2024

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

using System.Data.Entity;

namespace MefistoTheatreCompany.Models
{
    
        public class MefistoTheatreCompanyDbContext : IdentityDbContext<User>
        {
        
        //db sets of category, posts and comments
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //create dbcontext
        public MefistoTheatreCompanyDbContext() : base("MefistoTheatreConnectionV4", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitialiser());
        }

        public static MefistoTheatreCompanyDbContext Create()
            {
                return new MefistoTheatreCompanyDbContext();
            }

        

        }
    
}