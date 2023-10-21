using Microsoft.AspNetCore.Mvc;

namespace ActionResultExercise.Controllers
{
	public class CalculatorController : Controller
	{
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
	}
}
