using Microsoft.AspNetCore.Mvc;

public class TestController : Controller
{
    [Route("/api/[controller]")]
    public ActionResult Get()
    {
        return Ok("controller!");
    }
}