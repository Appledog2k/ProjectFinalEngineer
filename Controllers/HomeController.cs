﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectFinalEngineer.Models.AggregateExtensions;

namespace ProjectFinalEngineer.Controllers;

public class HomeController : Controller
{

    public HomeController()
    {
    }

    public IActionResult Home()
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
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
