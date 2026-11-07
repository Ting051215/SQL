using System.Web.Mvc;
using CourseCompass.Services;

namespace CourseCompass.Controllers
{
    public class CourseController : Controller
    {
        CourseService service = new CourseService();
        // 引入評價服務
        EvaluationService evalService = new EvaluationService();

        // 課程列表
        public ActionResult Index()
        {
            var list = service.GetAllCourses();
            return View(list);
        }

        // 搜尋
        [HttpPost]
        public ActionResult Search(string keyword)
        {
            var list = service.Search(keyword);
            return View("Index", list);
        }

        // 詳細資料 (在這裡把該課評論與平均分傳給前端)
        public ActionResult Detail(string id)
        {
            var course = service.GetCourse(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            // 抓取評論清單
            ViewBag.Evaluations = evalService.GetEvaluationsByCourse(id);

            // 💡 關鍵修改：抓取這門課的「四項平均分數」物件，傳給前端
            ViewBag.Averages = evalService.GetCourseAverages(id);

            return View(course);
        }
    }
}