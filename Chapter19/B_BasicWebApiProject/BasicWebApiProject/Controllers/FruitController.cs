using Microsoft.AspNetCore.Mvc;

namespace BasicWebApiProject.Controllers;

[ApiController]
public class FruitController : ControllerBase
{
    private readonly FruitService _fruitService;
    public FruitController(FruitService fruitService)
    {
        _fruitService = fruitService;
    }

    [HttpGet("fruit")]
    public IEnumerable<string> Index()
    {
        return _fruitService.Fruit;
    }

    [HttpGet("fruit/{id}")]
    public ActionResult<string> View(int id)
    {
        if (id >= 0 && id < _fruitService.Fruit.Count)
        {
            return _fruitService.Fruit[id];
        }

        return NotFound();
    }
}
