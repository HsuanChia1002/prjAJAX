using Microsoft.AspNetCore.Mvc;
using prjAJAX.Models;
using prjAJAX.ViewModels;

namespace prjAJAX.Controllers
{
    public class ApiController : Controller
    {

        private readonly IWebHostEnvironment _host;
        private readonly DemoContext _demo;
       public ApiController(IWebHostEnvironment host,DemoContext demo)
        {
            _host = host;
            _demo = demo;
        }
        public IActionResult Index(string? name, int? age)
        {
            System.Threading.Thread.Sleep(5000);

            if (string.IsNullOrEmpty(name))
                name = "Bar";
            if (age == null)
                age = 1;
            return Content($"AJAX {name},{age}");
        }


        public IActionResult register(MemberViewModel member, IFormFile formFile)
        {
            //實際路徑
            //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot\uploads\abc.jpg
            //string strPath = _host.ContentRootPath; //C:\Users\User\Documents\workspace\MSIT153Site
            //string strPath = _host.WebRootPath; //C:\Users\User\Documents\workspace\MSIT153Site\wwwroot
            string strPath = Path.Combine(_host.WebRootPath, "uploads", formFile.FileName);
            using(var filestream=new FileStream(strPath,FileMode.Create))
            {
                formFile.CopyTo(filestream);
            }

            return Content(strPath);

            //string fileInfo = $"{formFile?.FileName} - {formFile?.Length} - {formFile?.ContentType}";
            //return Content(fileInfo);

            //return Content("<h2>Ajax 你好 !!</h2>","text/html", System.Text.Encoding.UTF8);
            //return Content($"Hello {member.name}，{member.email},  You are {member.age} years old.");
        }


        public IActionResult CheckAccount(string? name)
        {
            //if (vm.name == "")
            //    return Content("請輸入姓名");
            //var data = _demo.Members.Any(m=>m.Name==name);
            if(name==null)
                return Content("請輸入姓名");
            if (!_demo.Members.Any(m => m.Name == name))
                return Content("此名稱可使用");
            else
                return Content("此名稱已被註冊");

        }
    }
}
