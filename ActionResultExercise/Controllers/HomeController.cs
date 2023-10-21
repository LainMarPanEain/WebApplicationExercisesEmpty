using Microsoft.AspNetCore.Mvc;

namespace ActionResultExercise.Controllers
{
	public class HomeController : Controller
	{
		private readonly IWebHostEnvironment _webHostEnvironment;//declare to know the files under the prj

		//Constructor injection for _webHostEnvironment obj
		public HomeController(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CalculateIndex()
		{
			return View();
		}
		public ViewResult Calculate(int num1, int num2, string method)
		{
			int result;
			switch (method)
			{
				case "add":
					result = num1 + num2;
					ViewBag.result = result;
					break;
				case "subtract":
					result = num1 - num2;
					ViewBag.result = result;
					break;
				case "multiply":
					result = num1 * num2;
					ViewBag.result = result;
					break;
				case "divide":
					result = num1 / num2;
					ViewBag.result = result;
					break;
				default:
					break;
			}
			return View("CalculateIndex");
		}
		public IActionResult FullNameIndex()
		{
			return View();
		}
		public ViewResult FullName(string fname, string lname)
		{
			string fullName = fname + lname;
			ViewBag.fullName = fullName;
			return View("FullNameIndex");
		}

		[ActionName("DownloadingFile")]//change action name, route name that output at website
		public FileResult FileDownload()
		{
			string fileName = "06-ASP.NETCoreMVC_Chapter-3.pdf";
			string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "DownloadFile\\") + fileName;
			byte[] fileInByte = System.IO.File.ReadAllBytes(filePath);
			return File(fileInByte, "application/pdf", fileName);
		}
		[NonAction]//tell the browser not to do this action, like change public to private
		public IActionResult Multiply()
		{
			int result = 2 * 3;
			ViewBag.Result = result;
			return View();
		}
		[HttpGet]//action verb get method(work without it also cus default is get)
		public int Sum(int n1, int n2)
		{
			return n1+n2;
		}

		[HttpPost]
		public int AddTwoNum(int a,  int b)
		{
			return a+b;
		}

		public IActionResult GetRemainder(int n1, int n2)
		{
			ViewBag.result = n1%n2;
			return View("Remainder");//This method's view name is not same with action name. So, Add the view name in the View()
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(string Username, string Password)
		{
			TempData["username"] = Username;
			TempData["password"] = Password;
			ViewData["successMsg"] = $"Hello {Username}, you have successfully logged in.";
			ViewData["failMsg"] = "Login failed.";
			if (!(Username.Equals("admin") && Password.Equals("admin123")))
			{
				return View();
			}
			return View("HomePage");
		}
		public IActionResult HomePage()
		{
			return View();
		}
		public IActionResult About()
		{
			return View();
		}
	}
}
