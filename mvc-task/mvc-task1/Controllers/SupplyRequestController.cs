
using Microsoft.AspNetCore.Mvc;
using mvc_task1.Models;
using System.Text.Json;

namespace mvc_task1.Controllers
{
    public class SupplyRequestController : Controller
    {
        private static List<SupplyRequest> confirmedRequests = new();

        public IActionResult Index()
        {
            ViewBag.Priority = Request.Cookies["PreferredPriority"] ?? "Normal";
            return View();
        }

        [HttpPost]
        public IActionResult AddItem(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName) ||
                product.Quantity <= 0 || product.UnitPrice <= 0)
            {
                return View("Index");
            }

            string? saved = HttpContext.Session.GetString("Products");

            List<Product> products = new();

            if (saved != null)
                products = JsonSerializer.Deserialize<List<Product>>(saved) ?? new();

            products.Add(product);

            HttpContext.Session.SetString("Products",
                JsonSerializer.Serialize(products));

            return RedirectToAction("Index");
        }

        public IActionResult Review()
        {
            string? saved = HttpContext.Session.GetString("Products");

            List<Product> products = new();

            if (saved != null)
                products = JsonSerializer.Deserialize<List<Product>>(saved) ?? new();

            decimal subtotal = 0;

            foreach (Product product in products)
                subtotal += product.Quantity * product.UnitPrice;

            decimal fee = 0;

            if (products.Count > 0)
            {
                switch (products[0].Priority)
                {
                    case "Normal": fee = 2; break;
                    case "Urgent": fee = 5; break;
                    case "Emergency": fee = 10; break;
                }
            }

            decimal discount = 0;

            if (subtotal > 200)
                discount = subtotal * 0.15m;
            else if (subtotal > 100)
                discount = subtotal * 0.10m;

            ViewBag.Subtotal = subtotal;
            ViewBag.Fee = fee;
            ViewBag.Discount = discount;
            ViewBag.FinalTotal = subtotal + fee - discount;

            return View(products);
        }

        [HttpPost]
        public IActionResult ConfirmRequest()
        {
            string? saved = HttpContext.Session.GetString("Products");

            if (string.IsNullOrEmpty(saved))
                return RedirectToAction("Review");

            List<Product> products =
                JsonSerializer.Deserialize<List<Product>>(saved) ?? new();

            if (products.Count == 0)
                return RedirectToAction("Review");

            decimal subtotal = 0;

            foreach (Product product in products)
                subtotal += product.Quantity * product.UnitPrice;

            decimal fee = 0;

            switch (products[0].Priority)
            {
                case "Normal": fee = 2; break;
                case "Urgent": fee = 5; break;
                case "Emergency": fee = 10; break;
            }

            decimal discount = 0;

            if (subtotal > 200)
                discount = subtotal * 0.15m;
            else if (subtotal > 100)
                discount = subtotal * 0.10m;

            SupplyRequest request = new SupplyRequest
            {
                RequestId = confirmedRequests.Count + 1,
                Products = products,
                Subtotal = subtotal,
                Fee = fee,
                Discount = discount,
                FinalTotal = subtotal + fee - discount,
                Status = "Confirmed"
            };

            confirmedRequests.Add(request);

            HttpContext.Session.Remove("Products");

            return RedirectToAction("MyRequests");
        }

        public IActionResult ClearDraft()
        {
            HttpContext.Session.Remove("Products");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SavePreference(string priority)
        {
            if (priority != "Normal" &&
                priority != "Urgent" &&
                priority != "Emergency")
                return RedirectToAction("Index");

            CookieOptions options = new CookieOptions();
            options.Expires = DateTimeOffset.Now.AddDays(7);

            Response.Cookies.Append("PreferredPriority", priority, options);

            return RedirectToAction("Index");
        }

        public IActionResult MyRequests()
        {
            return View(confirmedRequests);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
