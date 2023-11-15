using Microsoft.AspNetCore.Mvc;
using prjAJAX.ViewModels;

namespace prjAJAX.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index(string? name, int? age)
        {
            System.Threading.Thread.Sleep(5000);

            if (string.IsNullOrEmpty(name))
                name = "Bar";
            if (age == null)
                age = 1;
            return Content($"AJAX {name},{age}");
        }


        public IActionResult register(MemberViewModel member)
        {

            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            return Content($"Hello {member.name}，{member.email},  You are {member.age} years old.");
        }
    }
}
