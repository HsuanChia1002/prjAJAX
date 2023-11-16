using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index(UserViewModel vm)
        {
            //System.Threading.Thread.Sleep(5000);

            if (string.IsNullOrEmpty(vm.name))
                vm.name = "Bar";
            if (vm.age == null)
                vm.age = 1;
            if (vm.email == null)
                vm.email = "Bar@gmail.com";
            return Content($"AJAX {vm.name},{vm.email},{vm.age}years old");
        }


        public IActionResult register(Members member, IFormFile formFile)
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
            member.FileName=formFile.FileName;
            //處理圖片資料=>MemoryStream
            byte[]? bytes = null;
            using(var memoryStream=new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                bytes= memoryStream.ToArray();
            }
            member.FileData=bytes;

            //資料庫新增Member
            _demo.Members.Add(member);
            _demo.SaveChanges();            
            
            
            return Content("新增成功");

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

        public IActionResult cities()
        {
            var cities = _demo.Address.Select(c => c.City).Distinct();
            return Json(cities);
        }
        public IActionResult districts(string city)
        {
            var districts = _demo.Address.Where(a => a.City == city).Select(a => a.SiteId).Distinct();
            return Json(districts);
        }

        public IActionResult road(string siteId)
        {
            var road = _demo.Address.Where(a => a.SiteId == siteId).Select(a => a.Road).Distinct();
            return Json(road);
        }

        //讀取資料庫中二進位的圖片
        public IActionResult GetImageByte(int id = 1)
        {
            Members? member = _demo.Members.Find(id);
            byte[]? img = member?.FileData;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }
    }
}
