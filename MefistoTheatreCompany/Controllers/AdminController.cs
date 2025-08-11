// Tiani Perera - Mefisto Theatre Company 03/02/2024
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MefistoTheatreCompany.Models;
using MefistoTheatreCompany.Models.ViewModels;
using System.Web.Security;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Microsoft.Owin.BuilderProperties;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using Members = MefistoTheatreCompany.Models.Member;
using System.Runtime.Remoting.Contexts;
using System.Web.Configuration;

namespace MefistoTheatreCompany.Controllers { 

    //controller inherits from AccountController so it can borrow the login/registration stuff
    [Authorize(Roles="Admin")]//this controller can be accessed only by admin role


    public class AdminController : AccountController
    {

        //here is the instance of the mefisto db contexxt
        private MefistoTheatreCompanyDbContext db = new MefistoTheatreCompanyDbContext();
        public AdminController() : base()
        {
            
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(userManager,signInManager)
        {

        }



        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {

            return View();

        }


        [Authorize]
        public ActionResult ViewAllUsers()
        {

            return View();

        }

        public ActionResult ViewAllStaff()
        {
            //get all the staff and order them by registration date
            var users = db.Users.ToList();

            // Separate staff 
            var staffMembers = users.OfType<Staff>().OrderBy(s => s.StaffId).ToList();
            return View(staffMembers);

        }

        [Authorize]

        public ActionResult ViewAllMembers(string SearchString)
        {
            //get all the users and order them by registration date
            var users = db.Users.OfType<MefistoTheatreCompany.Models.Member>().ToList();        
            if (!users.Any())
            {
                ViewBag.ErrorMessage("no users with that username found");
            }


            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by users
                users = users.Where(u => u.UserName.Equals(SearchString.Trim())).ToList();

                if (!users.Any())
                {
                    ViewBag.ErrorMessage("no users with that username found");
                }
            }
            return View(users);
        }






        [HttpGet]
        public ActionResult CreateEmployee()
        {
            CreateEmployeeViewModel employee = new CreateEmployeeViewModel();
            //get rles from the data base- store them as selct list items
            var roles = db.Roles.Where(r => r.Name == "Admin" || r.Name == "Employee" || r.Name == "Moderator").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name //r.name
            }).ToList();
            //assign roles to the employee roles property
            employee.Roles = roles; ; //


            //return View(employee);
            return View(new CreateEmployeeViewModel
            {
                Roles = roles,
            });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmployee(CreateEmployeeViewModel model)//, CreateEmployeeViewModel model
        {
            
            if (ModelState.IsValid)
            {

                //var eStaff = db.Users.OfType<Staff>().FirstOrDefault(u => u.StaffId == model.StaffId);
                //if (eStaff!=null)
                //{
                //    ModelState.AddModelError("", "staff with same id already exists");

                //    return View(model);
                //}

                Staff newStaff = new Staff
                {
                   // StaffId = model.StaffId,
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Street = model.Street,
                    City = model.City,
                    Postcode = model.Postcode,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    IsSuspended = model.IsSuspended,
                    RegisteredAt=DateTime.Now,
                    //Role=model.Role,
                };

                // ViewBag.Roles = new SelectList(db.Roles, "RoleId", "RoleName", model.Role);

                var result=UserManager.Create(newStaff, model.Password);
                //var result =

                //if user was stored in databbase successfully
                 if (result.Succeeded)
                { 

                var selectedRoleId = model.Role;

                //then add user to role selected
                UserManager.AddToRole(newStaff.Id, selectedRoleId);
                //create TempData
                TempData["AlertMessage"] = "Employee has been created";
                return RedirectToAction("ViewAllStaff", "Admin");
                }
            }

            var roles = db.Roles.Where(r => r.Name == "Admin" || r.Name == "Employee" || r.Name == "Moderator").Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString() //r.name
            }).ToList(); //???

            model.Roles = roles;//?
            return View(model);//?
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            //FIND EMPLOYEE IN THE DATA BASE BY ID
            User Staff = db.Users.Find(id) as User;
            return View(new EditEmployeeViewModel
            {

                Address1 = Staff.Address1,
                Address2 = Staff.Address2,
                Street = Staff.Street,
                City = Staff.City,
                Postcode = Staff.Postcode,
                Firstname = Staff.Firstname,
                Lastname = Staff.Lastname,
                Email = Staff.Email,
                EmailConfirm = Staff.EmailConfirmed,
                Password = Staff.PasswordHash,

            });

}
       
        
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditEmployee(string id,
            [Bind(Include = "FirstName,LastName,Address1,Address2,Street,City,Postcode,SelectedRole")] EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                User staff =(User)await UserManager.FindByIdAsync(id);
                UpdateModel(staff);

                IdentityResult result = await UserManager.UpdateAsync(staff);

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewAllStaff","Admin");
                }
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            //FIND EMPLOYEE IN THE DATA BASE BY ID
            User user = db.Users.Find(id) as User;
            return View(new DeleteViewModel
            {

                Address1 = user.Address1,
                Address2 = user.Address2,
                Street = user.Street,
                City = user.City,
                Postcode = user.Postcode,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                EmailConfirm = user.EmailConfirmed,
            }); ;


        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, DeleteViewModel model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            //check we are not deleting our own account
            User user =await UserManager.FindByIdAsync(id);//get user id
            //if user does exist
            if (user == null)
            {
                return HttpNotFound();
            }

            //calls the methods
            RemoveUserComment(user);
            RemoveUserBlogPost(user);
            await UserManager.DeleteAsync(user);
            return RedirectToAction("ViewAllUsers", "Admin");
        }


        //this is used to remove comments of users whos accounts are going to be deleted

        private void RemoveUserComment(User user)
        {
            var userComments = db.Comments.Where(c=>c.UserId == user.Id).ToList();

            foreach(var comment in userComments)
            {
                db.Comments.Remove(comment);

            }

          
            db.SaveChanges();
        }

        //this is used to remove posts of users whos accounts are going to be deleted
        private void RemoveUserBlogPost(User user)
        {
            var blogPosts = db.BlogPosts.Where(c => c.UserId == user.Id).ToList();

            foreach (var post in blogPosts)
            {
                db.BlogPosts.Remove(post);

            }


            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        //this action will process the search form on the view all staff page
        //the name of the sting parameter SearchString must be the same
        //with the name of the textbox on the view
        public ViewResult ViewAllStaff(string SearchString)
        {
            var users = db.Users.OfType<Staff>().ToList();

            if (!users.Any())
            {
                ViewBag.ErrorMessage("no users with that username found");
            }

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                // If a search string is provided, filter the posts by users
                users = users.Where(u => u.UserName.Equals(SearchString.Trim())).ToList();

                if (!users.Any())
                {
                    ViewBag.ErrorMessage("no users with that username found");
                }
            }
            return View(users);
        }


        [HttpGet]
        public ActionResult EditMember(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            //FIND MEMBER IN THE DATA BASE BY ID
            MefistoTheatreCompany.Models.Member member = db.Users.Find(id) as MefistoTheatreCompany.Models.Member;

            
            return View(new EditMemberViewModel
            {
                //displays the current member info on the view
                Address1 = member.Address1,
                Address2 = member.Address2,
                Street = member.Street,
                City = member.City,
                Postcode = member.Postcode,
                Firstname = member.Firstname,
                Lastname = member.Lastname,
                Email = member.Email,
                EmailConfirmed=member.EmailConfirmed,
            });


        }




        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditMember(string id,
            [Bind(Include = "FirstName,LastName,Address1,Address2,Street,City,Postcode,EmailConfirmed")] EditMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                //GET member from database by id
                MefistoTheatreCompany.Models.Member member = (MefistoTheatreCompany.Models.Member)await UserManager.FindByIdAsync(id);
               
                UpdateModel(member);//update details using the values from model

                IdentityResult result = await UserManager.UpdateAsync(member);

                if (result.Succeeded)
                {
                    return RedirectToAction("ViewAllMembers", "Admin");
                }
            }
            return View(model);
        }


        //get user details
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            User user = db.Users.Find(id);

            //displys a specific user details page depending on the role
            if(user == null)
            {
                return HttpNotFound();
            }
            if(user is Staff)
            {
                return View("DetailsStaff",(Staff)user);
            }
            if(user is MefistoTheatreCompany.Models.Member)
            {
                return View("DetailsMember", (MefistoTheatreCompany.Models.Member)user);
            }

            return HttpNotFound();
        }



        public ActionResult ViewAllRoles()
        {
            //displays all roles found in the database as a list
            
            var roles = db.Roles.ToList();

            return View(roles);

        }

        [HttpGet]
        public ActionResult CreateRole( ) 
        {
            return View();
        }

        [HttpPost]  
        public ActionResult CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //get the role manager
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                //make sure that there are no duplicate roles stored in the database
                if (!roleManager.RoleExists(model.RoleName))
                {
                    roleManager.Create(new IdentityRole(model.RoleName));
                    return RedirectToAction("ViewAllRoles", "Admin");

                }
            }

            return View(model);
        }




        [HttpGet]
        public  ActionResult EditRole(string roleName)
        {
            
           var role = db.Roles.SingleOrDefault(r=>r.Name== roleName);

            if (role == null)
            {
                return HttpNotFound();
            }

            //if user is admin cannot edit
            if (String.Equals(role.Name, "admin", StringComparison.OrdinalIgnoreCase))//User.Identity.GetUserId()
            {
                return RedirectToAction("ViewAllRoles", "Admin");//set pop up
            }

            return View(new EditRoleViewModel
            {

                OldRoleName = role.Name,
            }) ;

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> EditRole( EditRoleViewModel model, string roleName)
        {
               
            if (ModelState.IsValid)
            {
                
                //retrieve role
                var role = db.Roles.SingleOrDefault(r => r.Name ==  roleName);

               
                //if no role is found
                if (role == null)
                {
                    return HttpNotFound();
                }

                //update the name
                role.Name = model.NewRoleName;

                //save changes to the database
                await db.SaveChangesAsync();
               
               return RedirectToAction("ViewAllRoles","Admin"); 
                
                
            }
            return View(model);
        }




        [HttpGet]
        public ActionResult DeleteRole(string roleName)
        {
            var role = db.Roles.SingleOrDefault(r => r.Name == roleName);

            if (role == null)
            {
                return HttpNotFound();
            }
            //if user is admin cannot delete
            if (String.Equals(role.Name, "admin", StringComparison.OrdinalIgnoreCase))//User.Identity.GetUserId()
            {
                return RedirectToAction("ViewAllRoles", "Admin");//set pop up
            }
            return View(new RoleViewModel
            {

                RoleName = role.Name
            });

        }


        [HttpPost]
        public async Task<ActionResult> DeleteRole(DeleteRoleViewModel model, string roleName)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles.SingleOrDefault(r => r.Name == roleName);


                //if no role is found
                if (role == null)
                {
                    return HttpNotFound();
                }


                //delete from database
                db.Roles.Remove(role);
                await db.SaveChangesAsync();

                return RedirectToAction("ViewAllRoles", "Admin");
            }
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //cant change yyour own role
            if (id == User.Identity.GetUserId())
            {
                return RedirectToAction("ViewAllStaff", "Admin");
            }

            //get user by id
            User user = await UserManager.FindByIdAsync(id);

            //get users current role
            string oldRole = (await UserManager.GetRolesAsync(id)).Single();

            //GET ALL the roles from the database and store them asa list selectedistitems
            var items = db.Roles.Select(r => new SelectListItem
            {
                Text=r.Name,
                Value=r.Name,
                Selected=r.Name==oldRole

            }).ToList();

            //build the changeroleviewmodel object including the list of roles
            //and send it tot he view displaying the roles in a drop down list with the users current role display

            return View(new ChangeRoleViewModel
            {
                Username=user.UserName,
                Roles=items,
                OldRole=oldRole,
                
            });

            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ChangeRole")]
        public async Task<ActionResult>ChangeRoleConfirmed(string id, [Bind(Include ="Role")]ChangeRoleViewModel model)
        {
            //cant change your own role
            if(id==User.Identity.GetUserId())
            {
                return RedirectToAction("ViewAllStaff", "Admin");
            }



            if(ModelState.IsValid)
            {
                User user=await UserManager.FindByIdAsync(id);//get user id
                string oldRole = (await UserManager.GetRolesAsync(id)).Single();//onlyy ever a single role

               
                //if current role is the same with the selected role then there is no point to update the database
                if (oldRole == model.Role)
                {
                    return RedirectToAction("ViewAllUsers", "Admin");
                    
                }




                //remove user from the old role first
                await UserManager.RemoveFromRoleAsync(id, oldRole);

                //now we are adding the user to the new role
                await UserManager.AddToRoleAsync(id, model.Role);


                //if user was suspended
                if (model.Role == "Suspended")
                {
                    //then set isSuspended to true
                    user.IsSuspended = true;
                    await UserManager.UpdateAsync(user);
                }

                // If role is anything other than suspended we need to change user type.
                if (model.Role != "Suspended")
                {
                    string result;
                    if (model.Role == "Admin" || model.Role == "Employee" || model.Role == "Moderator")
                    {
                        result = "Staff";//name of class
                    }
                    else
                    {
                        result = model.Role;//name of class Member
                    }
                    //Update discriminator to change the type of this user.This is a bit of a hack, but it works!
                    db.Database.ExecuteSqlCommand(
                    "UPDATE AspNetUsers SET Discriminator={0} WHERE id={1}", result, id);

                }


                return RedirectToAction("ViewAllUsers", "Admin");
            }
           
            return View(model);

        }
        

    }




}