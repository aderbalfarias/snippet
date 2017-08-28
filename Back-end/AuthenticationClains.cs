namespace xpto.MVC5.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (AuthenticationManager.User.Identity.IsAuthenticated == true)
                return RedirectToLocal(Url.Action("Index", "Home"));

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserService serviceUser = new UserService();
                User userLogin = new Domain.User();
                userLogin.Login = model.UserName;
                userLogin.AddressMail = model.UserName;
                userLogin.Pwd = Utils.ConvertToHash(model.Password);
                Domain.User user = serviceUser.UserAcess(userLogin);
                if (user != null)
                {
                    SignInAsync(user, model.RememberMe);
                    if (user.LastAccess.HasValue == false || user.LastAccess.Value == DateTime.MinValue)
                        return RedirectToAction("Manager", model);

                    serviceUser.UserLastAcess(user.Id);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("UserName", "Usuário ou senha inválido.");
                }
            }
			
            return View("Index", model);
        }

        private void SignInAsync(User user, bool isPersistent)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Authentication, user.Login));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Login));
            claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, user.UserProfile.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.AddressMail));

            ClaimsIdentity id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            IOwinContext ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Administrador, Usuario")]
        public ActionResult Manager()
        {
            ManagerUserViewModel viewModelManage = new ManagerUserViewModel();
            ViewBag.ChangePassword = true;
            ViewBag.ReturnUrl = Url.Action("Manager");
            return View(viewModelManage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manager(ManagerUserViewModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manager");
            if (ModelState.IsValid)
            {
                UserService userService = new UserService();
                User userNow = userService.GetItem(new Domain.User() { Id = AuthenticationBase.UserId, Active = true });
                userNow.Pwd = Utils.ConvertToHash(model.OldPassword);
                userNow = userService.UserAcess(userNow);
                if (userNow == null)
                    return RedirectToAction(Url.Action("Index"));

                userNow.Pwd = Utils.ConvertToHash(model.NewPassword);
                if (userService.ChangePassWord(userNow))
                    userService.UserLastAcess(userNow.Id);

                return RedirectToLocal(Url.Action("Index", "Home"));
            }
			
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Index");
            if (ModelState.IsValid)
            {
                UserService userService = new UserService();
                User result = userService.GetItem(new Domain.User() { AddressMail = model.Email, Active = true });
                if (result != null)
                {
                    userService.ResetPassword(result);
                }
                ModelState.AddModelError("Email", "Conta de usuário inválida!");
            }
			
            return RedirectToAction("Index");
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }
        }
        #endregion
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace xpto.MVC5.Controllers.Base
{
    public static class AuthenticationBase
    {
        private static IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        public static int UserId
        {
            get
            {
                return Convert.ToInt32(AuthenticationManager.User.FindFirst(ClaimTypes.Sid).Value);
            }
        }

        public static String CompanyUser
        {
            get
            {
                if (AuthenticationManager.User.Identity.IsAuthenticated)
                    return AuthenticationManager.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                return String.Empty;

            }
        }

        public static String UserName
        {
            get
            {
                if (AuthenticationManager.User.Identity.IsAuthenticated)
                    return AuthenticationManager.User.FindFirst(ClaimTypes.Name).Value;

                return String.Empty;

            }
        }

        public static String MailUser
        {
            get
            {
                if (AuthenticationManager.User.Identity.IsAuthenticated)
                    return AuthenticationManager.User.FindFirst(ClaimTypes.Email).Value;

                return String.Empty;

            }
        }

        public static UserProfile UserProfile
        {
            get
            {
                if (AuthenticationManager.User.IsInRole(Domain.UserProfile.Administrador.ToString()))
                    return UserProfile.Administrador;

                return UserProfile.Usuario;
            }
        }
    }