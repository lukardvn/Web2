using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApp.Models;
using WebApp.Models.DomainEntities;
using WebApp.Persistence.UnitOfWork;
using WebApp.Providers;
using WebApp.Results;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private IUnitOfWork unitOfWork;

        public AccountController()
        {
        }

        public AccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        /// <summary>
        /// GetAll()
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        //mozda bude zezalo konverzija iquer to ienumer----
        public IEnumerable<ApplicationUser> GetUsers()
        {
            return UserManager.Users.ToList();
        }

        /// <summary>
        /// Get()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(ApplicationUser))]
        [Authorize(Roles = "Admin, AppUser, Controller")]
        public IHttpActionResult GetUser(string id)
        {
            ApplicationUser user = UserManager.Users.ToList().Find(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Route("getProfileInfo/{id}")]
        [AllowAnonymous]
        public Osoba GetProfileInfo(string id)
        {
            
            ApplicationUser user = unitOfWork.User.Get(id);
            Osoba o = new Osoba();
            try
            {
                o.Username = user.UserName;
            }
            catch
            {
                
            }
            try
            {
                o.Name = user.Name;
            }
            catch
            {
                o.Name = "";
            }
            try
            {
                o.Surname = user.Surname;
            }
            catch
            {
                o.Surname = "";
            }
            try
            {
                switch (user.UserTypeID)
                {
                    case 1: o.Tip = "Obican"; break;
                    case 2: o.Tip = "Student"; break;
                    case 3: o.Tip = "Penzioner"; break;

                    default: o.Tip = ""; break;
                }
            }
            catch
            {
                o.Tip = "";
            }
            try
            {
                o.Adress = user.Adress;
            }
            catch
            {
                o.Adress = "";
            }
            try
            {
                o.Date = Convert.ToDateTime(user.DateOfBirth);
            }
            catch
            {

            }


            return o;
        }

        [Route("updateProfileInfo")]
        [AllowAnonymous]
        public IHttpActionResult UpdateProfileInfo(Osoba o)
        {
            ApplicationUser auser = unitOfWork.User.Find(x => x.UserName == o.Username).FirstOrDefault();

            auser.Name = o.Name;
            auser.Surname = o.Surname;
            auser.Adress = o.Adress;
            switch(o.Tip)
            {
                case "Obican": auser.UserTypeID = 1; break;
                case "Student": auser.UserTypeID = 2; break;
                case "Penzioner": auser.UserTypeID = 3; break;

                default: break;
            }
            auser.DateOfBirth = Convert.ToDateTime(o.Date);

            unitOfWork.User.Update(auser);
            unitOfWork.Complete();

            return Ok();
        }


        ///// <summary>
        ///// Put()
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="user"></param>
        ///// <returns></returns>
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutUser(string id, ApplicationUser user)
        //{
        //    if (!ModelState.IsValid || user == null)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    ApplicationUser existingUser = UserManager.Users.Where(x => x.Id == id).FirstOrDefault();

        //    if (existingUser == null)
        //    {
        //        return BadRequest("Korisnik ne postoji");
        //    }

        //    //da li ovo treba ovako pitati ?
        //    existingUser.Email = user.Email;
        //    existingUser.EmailConfirmed = user.EmailConfirmed;
        //    existingUser.AccessFailedCount = user.AccessFailedCount;
        //    //??
        //    existingUser.Id = user.Id;
        //    //videti da li treba uopste ovo ovako?? ako treba nastaviti tamo

        //    IdentityResult result = await UserManager.UpdateAsync(existingUser);

        //    if (!result.Succeeded)
        //    {
        //        return BadRequest();
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}


        /// <summary>
        /// Delete()
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(ApplicationUser))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteUser(string id)
        {
            ApplicationUser user = UserManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            UserManager.Delete(user);

            return Ok(user);
        }

        [AllowAnonymous]
        public bool UserExists(string email)
        {
            return UserManager.Users.FirstOrDefault(x => x.Email == email) != null;
        }

        ///


        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(true);
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            if (user.UserName == "admin@yahoo")
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }

            
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        [NonAction]
        private int ConvertStringToInt(string tipKorisnika)
        {
            if (tipKorisnika.Equals("regularan"))
            {
                return 0;
            }
            else if (tipKorisnika.Equals("student"))
            {
                return 1;
            }
            else if (tipKorisnika.Equals("penzioner"))
            {
                return 2;
            }
            else
            {
                return -1;
            }

        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        [ResponseType(typeof(ApplicationUser))]
        [HttpPost]
        public async Task<IHttpActionResult> Register(/*RegisterBindingModel model*/)
            {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var httpRequest = HttpContext.Current.Request;


            var samoDavidim = httpRequest.Files;

            string status = "verified";
            if (httpRequest.Form.Get("tip") != "Obican")
                status = "not verified";



            var user = new ApplicationUser() { 
                UserName = httpRequest.Form.Get("email"), 
                Email = httpRequest.Form.Get("email"),
                DateOfBirth = DateTime.Parse(httpRequest.Form.Get("date")),
                Adress = httpRequest.Form.Get("adress"),
                Name = httpRequest.Form.Get("name"),
                Surname = httpRequest.Form.Get("surname"),
                Status = status,
                UserTypeID = ConvertStringToInt(httpRequest.Form.Get("tip")),
                Files = ""};

            IdentityResult result = await UserManager.CreateAsync(user,httpRequest.Form.Get("password"));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            } else
            {
                UserManager.AddToRole(user.Id, "AppUser");

                if(httpRequest.Files.Count > 0)
                {
                    var path = GetUserFolderPath(user.Id);
                    CreateUserFolder(path);

                    List<string> uploadedFiles = new List<string>();
                    foreach(string file in httpRequest.Files)
                    {
                        var uploadedFile = httpRequest.Files[file];

                        if (IsCorrectFileExtension(uploadedFile.FileName)){
                            uploadedFile.SaveAs(path + "/" + uploadedFile.FileName);
                            uploadedFiles.Add(uploadedFile.FileName);
                        }
                    }

                    if(uploadedFiles.Count > 0)
                    {
                        if (user.Files != null && user.Files.Length > 0)
                            user.Files += "," + string.Join(",", uploadedFiles);
                        else
                            user.Files = string.Join(",", uploadedFiles);

                        user.Status = "processing";
                        UserManager.Update(user);
                    }
                }
            }

            return Ok(user);
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        [Route("ProcessingUsers")]
        [Authorize(Roles = "Controller")]
        [HttpGet]
        public List<ApplicationUser> GetProcessingUsers()
        {
            return UserManager.Users.ToList().FindAll(x => x.Status == "processing");
        }

        [Route("VerifyUser/{id}")]
        [Authorize(Roles ="Controller")]
        [HttpPut]
        public IHttpActionResult VerifyUser(string id)
        {
            var a = UserManager.Users.ToList();

            var user = UserManager.Users.FirstOrDefault(x => x.Id == id);

            if(user == null)
            {
                return BadRequest("User does not exists");
            }

            if (!UserManager.IsInRole(id, "AppUser"))
            {
                return BadRequest();
            }

            user.Status = "verified";
            UserManager.Update(user);

            return Ok();
        }

        [Route("DenyUser/{id}")]
        [Authorize(Roles = "Controller")]
        [HttpPut]
        public IHttpActionResult DenyUser(string id)
        {
            var user = UserManager.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return BadRequest("User does not exists");
            }

            if (!UserManager.IsInRole(id, "AppUser"))
            {
                return BadRequest();
            }

            user.Status = "denied";
            UserManager.Update(user);

            return Ok();
        }

        [Route("UploadPictures")]
        [ResponseType(typeof(string))]
        [HttpPost]
        public IHttpActionResult PictureUpload()
        {
            var httpRequest = HttpContext.Current.Request;

            var userId = User.Identity.GetUserId();

            var user = UserManager.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
                return BadRequest("User does not exist");

            var userPath = GetUserFolderPath(userId);
            CreateUserFolder(userPath);

            if (httpRequest.Files.Count == 0)
                return BadRequest("No files selected");

            List<string> uploadedFiles = new List<string>();
            foreach(string file in httpRequest.Files)
            {
                var uploadedFile = httpRequest.Files[file];

                if(IsCorrectFileExtension(uploadedFile.FileName))
                {
                    uploadedFile.SaveAs(userPath + "/" + uploadedFile.FileName);
                    uploadedFiles.Add(uploadedFile.FileName);
                }
            }

            if (uploadedFiles.Count == 0)
                return BadRequest("Error occured invalid file type");

            if (user.Files == null)
                user.Files = "";

            if (user.Files != null && user.Files.Length > 0)
                user.Files += "," + string.Join(",", uploadedFiles);
            else
                user.Files = string.Join(",", uploadedFiles);

            user.Status = "processing";
            UserManager.Update(user);

            return Ok(user.Files);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private void CreateUserFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private string GetUserFolderPath(string userId)
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/imgs/users/" + userId);
        }

        private bool IsCorrectFileExtension(string fileName)
        {
            var fileExtension = fileName.Split('.').Last();

            if (fileExtension == "jpg" || fileExtension == "jpeg" || fileExtension == "png" || fileExtension == "bmp")
            {
                return true;
            }
            return false;
        }



        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
