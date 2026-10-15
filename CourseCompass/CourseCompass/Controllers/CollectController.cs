using System;
using System.Web.Mvc;
using System.Linq;
using CourseCompass.Models;
using CourseCompass.Services;

namespace CourseCompass.Controllers
{
    public class CollectController : Controller
    {
        CollectService service = new CollectService();

        // 我的收藏 (首頁)
        public ActionResult Index()
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();
            var list = service.GetCollect(studentID);
            return View(list);
        }

        // ==========================================
        // 加入收藏 (GET: 顯示填寫備註頁面)
        // ==========================================
        public ActionResult Add(string courseID)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();
            var currentCollection = service.GetCollect(studentID);

            // 防重複檢查：如果早就收藏過，直接彈回清單
            if (currentCollection != null && currentCollection.Any(c => c.courseID == courseID))
            {
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = courseID;
            return View();
        }

        // ==========================================
        // 加入收藏 (POST: 接收備註並寫入資料庫)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(string courseID, string note)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();

            Collect collect = new Collect();
            collect.studentID = studentID;
            collect.courseID = courseID;
            collect.collectTime = DateTime.Now;
            collect.note = note ?? ""; // 接收使用者輸入的備註

            service.AddCollect(collect);
            return RedirectToAction("Index");
        }

        // ==========================================
        // 💡 新增：修改備註 (GET: 載入舊備註並顯示修改頁面)
        // ==========================================
        public ActionResult Edit(string courseID)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();
            var collectItem = service.GetSingleCollect(studentID, courseID);

            if (collectItem == null) return RedirectToAction("Index");

            return View(collectItem);
        }

        // ==========================================
        // 💡 新增：修改備註 (POST: 將新備註存回資料庫)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string courseID, string note)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();

            // 執行資料庫更新
            service.UpdateCollectNote(studentID, courseID, note);

            return RedirectToAction("Index");
        }

        // 刪除收藏 (GET: 顯示確認畫面)
        public ActionResult Delete(string courseID)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();
            var collectItem = service.GetSingleCollect(studentID, courseID);

            if (collectItem == null) return RedirectToAction("Index");

            return View(collectItem);
        }

        // 刪除收藏 (POST: 真正執行刪除)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string courseID)
        {
            if (Session["studentID"] == null) return RedirectToAction("Login", "User");

            string studentID = Session["studentID"].ToString();
            service.DeleteCollect(studentID, courseID);
            return RedirectToAction("Index");
        }
    }
}