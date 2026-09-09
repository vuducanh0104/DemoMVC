using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers;

public class StudentController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Student Index";
        return View();
    }

    [HttpPost]
    public IActionResult Index(
        string fullName,
        string addres,
        string university,
        string hoten)
    {
        var data =
            $"FullName: {fullName}, Address: {addres}, " +
            $"University: {university}, HoTen: {hoten}";

        ViewData["Data"] = data;
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.Title = "Create Student";
        return View();
    }
}
