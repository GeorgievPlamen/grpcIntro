using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzalandClient.Models;

namespace pizzalandClient.Controllers;

public class UserController(ILogger<UserController> logger, UserService.UserServiceClient userClient) : Controller
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly UserService.UserServiceClient _userClient = userClient;

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var loginRequest = new LoginRequest
        {
            Email = model.Email,
            Password = model.Password
        };

        try
        {
            var response = await _userClient.LoginAsync(loginRequest);

            _logger.LogInformation(response.User.Email);

            if (response.User?.Token != null)
            {
                // Store token in session/cookie
                HttpContext.Items.Add("JWT", response.User.Token);

                // Redirect to pizza listing page
                return RedirectToAction("ListPizzas", "Pizza");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

    }

    [HttpGet]
    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var registerRequest = new RegisterRequest
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password
        };

        try
        {
            var response = await _userClient.RegisterUserAsync(registerRequest);

            _logger.LogInformation($"Registered user: {response.User.Email}");

            // Optionally log in the user after registration
            if (response.User?.Token != null)
            {
                HttpContext.Items.Add("JWT", response.User.Token);

                // Redirect to pizza listing page
                return RedirectToAction("ListPizzas", "Pizza");
            }
            else
            {
                ModelState.AddModelError("", "Failed to register user.");
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("", "Registration failed.");
            return View(model);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
