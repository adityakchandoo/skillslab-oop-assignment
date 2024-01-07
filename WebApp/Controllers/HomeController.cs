using BusinessLayer.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        IFeedbackService _feedbackService;
        public HomeController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // TODO: FeedBack
        public ActionResult Feedback()
        {
            return Content("Your Feedback page.");
        }

        [HttpPost]
        public async Task<ActionResult> FeedbackPost(string text)
        {
            await _feedbackService.AddFeedbackAsync(text);

            return Json(new { status = "ok" });
        }
    }
}