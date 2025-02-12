using Microsoft.AspNetCore.Mvc;

namespace E_Commerce511.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class WelcomeController : Controller
    {
        public IActionResult Home()
        {
            string name = "Mohamed";
            int age = 25;
            List<string> skills = new List<string>();
            skills.Add("C++");
            skills.Add("C#");
            skills.Add("SQL");

            return View(skills);
        }
    }
}
