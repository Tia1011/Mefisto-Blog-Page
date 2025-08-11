// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static MefistoTheatreCompany.Models.Staff;
using System.Web.UI.WebControls;

namespace MefistoTheatreCompany.Models
{
    public class DatabaseInitialiser : DropCreateDatabaseAlways<MefistoTheatreCompanyDbContext>
    {
        protected override void Seed(MefistoTheatreCompanyDbContext context)
        {
            //base.Seed(context);

            
            //if there are no records stored in the users table
            if (!context.Users.Any())
            {
                //first we are going to create some roles and store them in the roles table
                //to create and store roles we need a roles table
                RoleManager<IdentityRole> identityRoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if admin doesnt exist
                if (!identityRoleManager.RoleExists("Admin"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Admin"));
                }

                //if moderator doesnt exist
                if (!identityRoleManager.RoleExists("Moderator"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Moderator"));
                }

                //if STAFF doesnt exist
                if (!identityRoleManager.RoleExists("Employee"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Employee"));
                }

                //if Member doesnt exist
                if (!identityRoleManager.RoleExists("Member"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Member"));
                }

                //if suspend doesnt exist
                if (!identityRoleManager.RoleExists("Suspended"))
                {
                    //then we create one
                    identityRoleManager.Create(new IdentityRole("Suspended"));
                }

                



                //save the new roles to the database
                context.SaveChanges();



            }

            //********************CREATE USERS**********************

            //to create users-customers or employees we need a Usermanager
            UserManager<User> UserManager = new UserManager<User>(new UserStore<User>(context));

            


                //Super literal password validation for  password for seeds
                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequireDigit = false,
                    RequiredLength = 1,
                    RequireLowercase = false,
                    RequireNonLetterOrDigit = false,
                    RequireUppercase = false,
                };

            //CREATE AN ADMIN 
            var administrator = new Staff
            {
                    StaffId = 1,
                    UserName = "admin@mefistotheatre.com",
                    Email = "admin@mefistotheatre.com",
                    Firstname = "Amanda",
                    Lastname = "Cloud",
                    Address1 = "10/A",
                    Street = "Poker Street",
                    City = "Glasgow",
                    Postcode = "G0 0RF",
                    EmailConfirmed = true,
                    IsSuspended = false,
                    RegisteredAt = DateTime.Now.AddYears(-6),
                   

                };


                //CREATE AN ADMIN
                //FIRST check if the admin exists in the database
                if (UserManager.FindByName("admin@mefistotheatre.com") == null)
                {
                    //add admin to the users table
                    UserManager.Create(administrator, "admin123");
                    //assign it to the admin role
                    UserManager.AddToRole(administrator.Id, "Admin");

                }


            //create a  moderator
            var moderator = new Staff
            {
                StaffId = 2,
                UserName = "moderator@mefistotheatre.com",
                Email = "moderator@mefistotheatre.com",
                Firstname = "Sarah",
                Lastname = "Bernard",
                Address1 = "10",
                Street = "Happy Street",
                City = "Glasgow",
                Postcode = "G0 5sT",
                EmailConfirmed = true,
                IsSuspended = false,
                RegisteredAt = DateTime.Now.AddYears(-3),


            };


            //CREATE a moderator
            //FIRST check if the moderator exists in the database
            if (UserManager.FindByName("moderator@mefistotheatre.com") == null)
            {
                //add admin to the users table
                UserManager.Create(moderator, "123");
                //assign it to the admin role
                UserManager.AddToRole(moderator.Id, "Moderator");

            }



            //CREATE AN EMPLOYEE
            var alex = new Staff
                {
                    StaffId = 10,
                    UserName = "alex@mefistotheatre.com",
                    Email = "alex@mefistotheatre.com",
                    Firstname = "Alex",
                    Lastname = "Cole",
                    Address1="15",
                    Address2="120A",
                    Street = "Townhead Street",
                    City = "Hamilton",
                    Postcode = "ML2 8B4",
                    EmailConfirmed = true,
                    IsSuspended = false,
                RegisteredAt = DateTime.Now.AddYears(-7),

            };

            //FIRST check if the staff exists in the database
            if (UserManager.FindByName("alex@mefistotheatre.com") == null)
            {
                //add admin to the users table
                UserManager.Create(alex, "staff2");
                //assign it to the Employee role
                UserManager.AddToRole(alex.Id, "Employee");

            }







            //CREATE AN EMPLOYEE
            var amelia = new Staff
            {

                //StaffId = 12,
                UserName = "amelia@mefistotheatre.com",
                Email = "amelia@mefistotheatre.com",
                Firstname = "Amelia",
                Lastname = "Rose",
                Address1="101",
                Street = "Hooded Street",
                City = "Glasgow",
                Postcode = "G2 8B4",
                EmailConfirmed = true,
                IsSuspended = false,
                RegisteredAt = DateTime.Now.AddYears(-6),
            };
            //FIRST check if the staff exists in the database
            if (UserManager.FindByName("amelia@mefistotheatre.com") == null)
            {
                //add admin to the users table
                UserManager.Create(amelia, "staff3");
                //assign it to the admin role
                UserManager.AddToRole(amelia.Id, "Employee");
            }


            //CREATE A member 
            var bill = new Member
            {

                UserName = "bill@gmail.com",
                Email = "bill@gmail.com",
                Firstname = "Bill",
                Lastname = "Gates",
                Address1 = "10",
                Street = "Armour Street",
                City = "Edinburgh",
                Postcode = "E1 5ST",
                EmailConfirmed = true,
                IsSuspended = false,
                RegisteredAt = DateTime.Now,


            };


            //CREATE A member
            //FIRST check if the member exists in the database
            if (UserManager.FindByName("bill@gmail.com") == null)
            {
                //add member to the users table
                UserManager.Create(bill, "bill123");
                //assign it to the member role
               UserManager.AddToRole(bill.Id, "Member");

            }

            //CREATE A member 
            var aryan = new Member
            {

                UserName = "aryan@gmail.com",
                Email = "aryan@gmail.com",
                Firstname = "Aryan",
                Lastname = "Abeykoon",
                Address1 = "101",
                Street = "Armour Street",
                City = "Edinburgh",
                Postcode = "E1 5AT",
                EmailConfirmed = true,
                IsSuspended = true,
                RegisteredAt = DateTime.Now,


            };


            //CREATE A member
            //FIRST check if the member exists in the database
            if (UserManager.FindByName("aryan@gmail.com") == null)
            {
                //add member to the users table
                UserManager.Create(aryan, "123");
                //assign it to the member role
                UserManager.AddToRole(aryan.Id, "Suspended");

            }
            context.SaveChanges();




            //seeding the categories
            var Cat1 = new Category() { CategoryName = "Announcements" };
            var Cat2 = new Category() { CategoryName = "Movie Reviews" };
            var Cat3 = new Category() { CategoryName = "Performance Reviews" };
            var Cat4 = new Category() { CategoryName = "Other" };

            //add each category  to the Categories Table
            context.Categories.Add(Cat1);
            context.Categories.Add(Cat2);
            context.Categories.Add(Cat3);
            context.Categories.Add(Cat4);

            context.SaveChanges();

            //seeding the posts table

            var post1 = new BlogPost()
            {
                BlogPostId = 1,
                Title = "Welcome",
                Content = "Hi Everyone! Welcome to our brand new website for the Mefisto Theatre Company",
                Approved = true,
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 15),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(14),
                User = administrator,
                Category = Cat1,
            };

            context.BlogPosts.Add(post1);

            var post2 = new BlogPost()
            {
                BlogPostId = 2,
                Title = "Details",
                Content = "We hope you will give us your suggestions for movies for us to review",
                Approved = true,
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 16),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(14),
                User = administrator,
                Category = Cat1,
            };

            context.BlogPosts.Add(post2);



            var post3 = new BlogPost()
            {
                BlogPostId = 3,
                Title = "Harry Potter and the philosopher's stone",
                Content = "There's nothing like the first in a series, is there? The introduction to the characters, the immersion into the fictional world, the first time you laugh, cry, care, and fear for someone's safety can never be repeated. No matter how many Harry Potter movies they crank out, or if they ever remake them in the future, none will come close to the wonderful first film, Harry Potter and the Sorcerer's Stone.\r\n\r\nI'm sure everyone has their own childhood memories of reading the Harry Potter books that they'll tell their grandkids about, but I'll never forget going to see the first movie in the theaters. The lights dimmed, John Williams's perfect theme played its first notes as Richard Harris walked down Privet Drive, and everyone in the theater was transported to another world. John Williams's numerous themes, all wonderful and a personification of the wizarding world, took the early movies to another level. As other composers tried their hands at the later films, that quality was missing. There's something truly special about going to see this movie on the big screen, and while the \"magical\" qualities might not all be credited to the music, it's certainly one of them.\r\n\r\nWelcome to the world of Harry Potter, where if you're a ten-year-old kid who doesn't fit in, you might get a letter delivered by an owl telling you you have magical powers and should go to a special school to hone them. Believe it or not, there are people who watch this movie without reading the books, so a bit of description is necessary. Obviously the stars of the show are the children, who were selected out of millions of other kids to be able to memorize lines, not look in the camera, endear themselves to worldwide audiences, and hopefully act. Daniel Radcliffe, Emma Watson, Rupert Grint, and Tom Felton are so cute and tiny in this first movie, you'll undoubtedly find yourself re-watching it as the years pass just to see them as kids again. I always marvel that child actors train themselves not to look in the camera, so even if their performances aren't perfect, I cut them slack, knowing firsthand how hard it is. And these kids had to dress in funny costumes, recite incantations without laughing, and pretend they're looking at things that were added in post production!\r\n\r\nUsually, in kids' movies, there's a grown-up or two who add to the cast and make the adult audience members feel less silly that they're watching it. In the Harry Potter movies, everyone wanted to be in them! Throughout the series you'll see a host of familiar faces as \"guest stars\" but the regulars will make special places in your heart. Richard Harris, Maggie Smith, Alan Rickman, and Robbie Coltrane are household names for little kids, because they're so convincing as the kindhearted Dumbledore, the wizened but sentimental McGonagall, the endlessly mimicable Snape, and the jolly Hagrid, kids today can't imagine they've had any other career prior to these movies! Is there any kid who doesn't immediately attribute the word \"earwax\" to Richard Harris, point out striped cats as \"Maggie Smith cats\", mumble \"Shouldn't have said that,\" when they make a mistake, or practice putting pauses in their sentences like Alan Rickman?\r\n\r\nFirst movies are so special, since they introduce audiences to a world that will hopefully capture their attention for however many more movies will be made. In J.K. Rowling's fantasy world, there's so much to fall in love with; and in the film adaptation you can really believe it exists. Seeing the Hogwarts structure for the first time creates a special feeling in your heart that can only be recaptured by watching the movie again or going to see the next in the series. The Great Hall, Quidditch, the Sorting Hat, talking portraits, flying lessons, selecting the perfect wand-all these Harry Potter moments are perfectly recreated in the first of a series that saw an entire generation grow up buying toy wands and trying Bertie Bott's Every Flavor Beans.\r\n\r\nEach installment has its special moments, and this first one has quite a few, even outside of the exposition. If a three-headed dog doesn't immediately conjure the name \"Fluffy,\" chess pieces have never come to life in your imagination, you don't laugh at the idea of counting your birthday presents, and you don't know what Richard Harris wants most in the world, you're missing out on one of the great joys in life. If somehow 2001 passed you by without a trip to the movie theaters Thanksgiving weekend, go find yourself a copy of this iconic, lovely movie.",
                Approved = false,
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 20),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 20).AddDays(0),
                User = alex,
                Category = Cat2,
            };

            context.BlogPosts.Add(post3);


            var post4 = new BlogPost()
            {
                BlogPostId = 4,
                Title = "Why do some people not like Emma Granger as Harry Potter",
                Content = "The main complaint people have about Emma Watson herself as Hermione(that means not how her role was written…. You can't blame her for that), is that she was “too pretty to be Hermione”.\r\n\r\nReading the books, I agree that she became way too pretty to be Hermione, but that's also no one's fault. In the beginning, when they hired her, she was the perfect Hermione (minus the buck teeth). She had the bushy hair, nearly perfect straight-A, bookish personality. She was a book to movie screen writer’s freakin’ dream (and honestly, so were all of the main characters). So, she grew up to have straight hair and really, be too pretty for Hermione. But so what? You can't blame the writers for that, and you certainly can't blame Emma Watson.\r\n\r\nThe only other complaint I've heard about Emma Watson as Hermione, is the fact that she is white, and some imagined her as black. I shall leave my opinion on this one for the sake of not re-opening that extra-dramatic can of worms.",
                Approved = true,
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 29),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 29).AddDays(14),
                User = amelia,
                Category = Cat3,
            };

            context.BlogPosts.Add(post4);

            context.SaveChanges();




            //seed comments

            var comment1 = new Comment()
            {
                CommentId = 1,
                Content = "Hi Everybody",
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(1),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(2),
                BlogId = 1,
                User=bill,
                Approved = true,
            };
            context.Comments.Add(comment1);

            var comment2 = new Comment()
            {
                CommentId = 2,
                Content = "Hi guys, so excited to be here!",
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(2),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(2),
                BlogId = 1,
                User = bill,
                Approved = true,

            };
            context.Comments.Add(comment2);
            

            var comment3 = new Comment()
            {
                CommentId = 3,
                Content = "As mentioned already, some felt that Emma was too “traditionally attractive” to play the part of a girl who is not really described in such in the books",
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 29),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 29),
                BlogId = 4,
                User = amelia,
                Approved = true,
            };
            context.Comments.Add(comment3);

            var comment4 = new Comment()
            {
                CommentId = 4,
                Content = "Because they took all her flaws, gave them to other characters, and turned her into a PC Social Justice figurine just oozing with Grrrrl power. Add to that the issue of “Hollywood Homely,” where the casting director picks someone who could be a fashion model, dresses them in dorky clothes and informs us that he or she is supposed to be a “plain looking” or even homely character",
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 29),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 29),
                BlogId = 4,
                User = alex,
                Approved = true,
            };
            context.Comments.Add(comment4);


            var comment5 = new Comment()
            {
                CommentId = 5,
                Content = "Hi Everybody",
                DatePosted = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(1),
                DateEdited = new DateTime(2018, 1, 1, 8, 0, 15).AddDays(2),
                BlogId = 1,
                User = aryan,
                Approved = false,
            };
            context.Comments.Add(comment5);

            context.SaveChanges();



        }


    }


    }
