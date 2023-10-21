using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CurrencyConverter.Controllers
{
    public class ConverterController : Controller
    {
        public IActionResult ChangeCurrency()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeCurrency(string currency,decimal amount)
        {
            @ViewBag.SelectedCurrency=currency;
            @ViewBag.Amount=amount;
            decimal result = 0;
            switch (currency)
            {
                case "USD": result= amount*2100; break;
                case "BAHT": result= amount * 990; break;
                case "SGD": result = amount * 1920; break;
            }
            ViewBag.Result = result;
            return View();
        }
        public IActionResult ChangeCurrencyVer2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeCurrencyVer2(string srcCurrency, string desCurrency, decimal amount)
        {
            @ViewBag.SelectedSrcCurrency = srcCurrency;
            @ViewBag.SelectedDesCurrency = desCurrency;
            @ViewBag.Amount = amount;
            ViewData["failMsg"] = "Choose appropriate currency to calculate.";
            //TempData["MyViewData"] = ViewData;
            decimal result = (decimal)(0.0);
            if(srcCurrency == "MMK")
            {
                if(desCurrency == "USD") { result = amount / 2100; }
                else if(desCurrency == "SGD") { result = amount / 1920; }
                else if(desCurrency == "BAHT") { result = amount / 990; }
                else { return View("ShowFailedMsg") ; }
            }
            if (srcCurrency == "USD")
            {
                if (desCurrency == "MMK") { result = amount * 2100; }
                else if (desCurrency == "SGD") { result = amount * (decimal)(1.35); }
                else if (desCurrency == "BAHT") { result = amount * 35; }
                else { return View("ShowFailedMsg"); }
            }
            if(srcCurrency == "SGD")
            {
                if( desCurrency == "MMK") { result = amount * 1920; }
                else if( desCurrency == "USD") { result = amount * (decimal)(0.74); }
                else if(desCurrency == "BAHT") { result = amount * (decimal)(26.12); }
                else { return View("ShowFailedMsg"); }
            }
            if(srcCurrency == "BAHT")
            {
                if(desCurrency == "USD") { result = amount*(decimal)(0.028); }
                else if(desCurrency == "SGD") { result = amount * (decimal)(0.038); }
                else if(desCurrency == "MMK") { result = amount*(decimal)(59.41); }
                else { return View("ShowFailedMsg"); }
            }
            ViewBag.Result = result;
            return View();
        }
        public IActionResult ShowFailedMsg()
        {
            //var passedViewData = TempData["MyViewData"] as ViewDataDictionary;
            // Access the ViewData values
            //var value1 = passedViewData["Key1"];
            return View();
        }
    }
}
