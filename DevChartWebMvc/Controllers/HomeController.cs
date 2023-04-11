using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DevChartWebMvc.Models;

namespace DevChartWebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    public IActionResult SelectByFilmAndDev(string filmName,string devName)
    {
        filmName = WebUtility.HtmlDecode(filmName);
        devName = WebUtility.HtmlDecode(devName);
        if (!string.IsNullOrWhiteSpace(filmName)&&!string.IsNullOrWhiteSpace(devName))
        {
            return Ok(PubConst.DevChart.SelectByFilmAndDev(filmName, devName));
        }
        else if(!string.IsNullOrWhiteSpace(devName))
        {
            return Ok(PubConst.DevChart.SelectByDev(devName));
        }
        else
        {
            return Ok(PubConst.DevChart.SelectByFilm(filmName));
        }
    }
}