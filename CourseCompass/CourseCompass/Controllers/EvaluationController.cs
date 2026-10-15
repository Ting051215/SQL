using System.Web.Mvc;
using CourseCompass.Models;
using CourseCompass.Services;

namespace CourseCompass.Controllers
{
    public class EvaluationController : Controller
    {
        EvaluationService service = new EvaluationService();

        // 💡 已經將不需要的 Index (全部評論) 方法刪除，因為評論已經整合進課程詳細頁面了！

        // ==========================================
        // 新增頁 (GET: 接收從網頁傳過來的 courseID)
        // ==========================================
        public ActionResult Create(string courseID)
        {
            if (Session["studentID"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            // 用 ViewBag 記住是哪一門課要被評論，等等要在前端當成隱藏欄位傳回
            ViewBag.CourseID = courseID;
            return View();
        }

        // ==========================================
        // 新增 (POST: 寫入資料庫並導回詳細頁)
        // ==========================================
        [HttpPost]
        public ActionResult Create(Evaluation eva, string courseID)
        {
            if (Session["studentID"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            string studentID = Session["studentID"].ToString();

            // 呼叫更新後的 Service，將三個關鍵資訊一起綁定寫入
            service.AddEvaluation(eva, courseID, studentID);

            // 評論成功後，直接導回「該課程的詳細資訊頁面」看成果！
            return RedirectToAction("Detail", "Course", new { id = courseID });
        }
    }
}