using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pizzalandClient.Interfaces;
using pizzalandClient.Models;
using pizzalandClient.Services;

namespace pizzalandClient.Controllers;

public class UserController(ILogger<UserController> logger, UserService.UserServiceClient userClient, ITokenProvider tokenProvider) : Controller
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly UserService.UserServiceClient _userClient = userClient;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

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
                tokenProvider.SetToken(response.User.Token);
                tokenProvider.SetUserId(response.User.Id);

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

            if (response.User?.Token != null)
            {
                HttpContext.Items.Add("JWT", response.User.Token);

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

    public override async void OnActionExecuting(ActionExecutingContext context)
    {
        var token = _tokenProvider.GetToken(CancellationToken.None);
        if (!string.IsNullOrEmpty(token))
        {
            string userName = GetUserNameFromToken(token);
            ViewBag.IsLoggedIn = true;
            ViewBag.UserName = userName;
        }
        else
        {
            ViewBag.IsLoggedIn = false;
        }

        base.OnActionExecuting(context);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        _tokenProvider.ClearToken();

        return RedirectToAction("Index", "Home");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private string GetUserNameFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        return userName;
    }
}
