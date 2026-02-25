using Logger.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using NLog;

namespace Logger.Controllers
{
    public class HomeController : Controller
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();
        public IActionResult Index()
        {
            _logger.Info("Home page visited");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.Info("Privacy page visited");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.Error("Error page triggered");

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}