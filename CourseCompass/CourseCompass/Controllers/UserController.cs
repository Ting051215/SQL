using System.Web.Mvc;
using CourseCompass.Models;
using CourseCompass.Services;

namespace CourseCompass.Controllers
{
    public class UserController : Controller
    {
        UserService service = new UserService();

        //登入頁
        public ActionResult Login()
        {
            return View();
        }

        //登入
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            User user = service.Login(account, password);

            if (user != null)
            {
                Session["studentID"] = user.studentID;
                Session["account"] = user.account;

                return RedirectToAction("Index", "Course");
            }

            ViewBag.msg = "帳號或密碼錯誤";

            return View();
        }



        //註冊頁
        public ActionResult Register()
        {
            return View();
        }

        //註冊
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (service.AccountExists(user.account))
            {
                ViewBag.msg = "帳號已存在";
                return View();
            }

            service.Register(user);

            return RedirectToAction("Login");
        }

        //登出
        public ActionResult Logout()
        {
            // 清空所有的 Session 資料
            Session.Clear();
            Session.Abandon();

            // 登出後，導回登入頁面
            return RedirectToAction("Login", "User");
        }
    }
}