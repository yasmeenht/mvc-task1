using Microsoft.AspNetCore.Mvc;

namespace mvc_task1.Controllers
{
    public class SupplyController : Controller
    {
        public IActionResult Index()
        {
            string[] categories = {"Drinks",
            "Food", "Snacks", "Hot Drinks",
            "Cold Drinks"};

            ViewBag.Categories = categories;
            return View();
        }
        public IActionResult Products()
        {
            ViewData["PageTitle"] = "Cafeteria Products";
            ViewData["ManagerName"] = "Ahmad Ali";
            ViewData["NumberOfProducts"] = 5;

            string[] products = { "Water", "Orange Juice", "Coffee", "Sandwich", "Chocolate" };
            ViewData["Products"] = products;
            return View();
            
        }
        public IActionResult Suppliers()
        {
            ViewBag.PageTitle = "Our Suppliers";
            ViewBag.NumberOfSuppliers = 3;
            string[] suppliers = {"Fresh Food Company",
            "Happy Drinks Company",
            "Daily Suppliers Company"};
            ViewBag.Suppliers = suppliers;
            return View();
        }

        [HttpGet]
        public IActionResult CreateRequests()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitRequests()
        {
            string employeeName = Request.Form["employeeName"].ToString();
            string productName = Request.Form["productName"].ToString();
            string quantity = Request.Form["quantity"].ToString();
            string requiredDate = Request.Form["requiredDate"].ToString();


            if (string.IsNullOrWhiteSpace(employeeName))
            {
                ViewBag.Message = "Employee Name is required!";
                return View();
            }
            TempData["Message"] = "Request submitted successfully!";
            return RedirectToAction("About");

        }
        public IActionResult About()
        {
           
            return View();
        }
    }
}
