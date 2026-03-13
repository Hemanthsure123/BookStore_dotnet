using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    public IActionResult UserDashboard()
    {
        return View();
    }
}