namespace ReactAdminNet;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Retail.Helpers;

[Route("")]
[ApiController]
[AllowAnonymous]
public class CatchallController : ControllerBase
{
    private readonly IWebHostEnvironment webHostEnvironment;

    public CatchallController(IWebHostEnvironment webHostEnvironment)
    {
        this.webHostEnvironment = webHostEnvironment;
    }

    [Route("{**catchAll}")]
    [HttpGet]
    [ResponseCache(Duration = 3600)]
    public IActionResult Index()
    {
        var path = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", "index.html");
        var text = System.IO.File.ReadAllText(path);
        return Content(text, "text/html");
    }
}
