using API.Products;
using API.Users.DTOs;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;

namespace API.Users;

public class UsersController(IUserService userService) : CustomBaseController
{

    [HttpGet]
    public IActionResult GetAll([FromServices] AgeCalculator ageCalculator)
    {
        return Ok(userService.GetAllWithCalculatedAge(ageCalculator));
    }

    [HttpGet("{userId}")]
    public IActionResult GetById(int userId, [FromServices] AgeCalculator ageCalculator)
    {
        return CreateActionResult(userService.GetByIdWithCalculatedAge(userId, ageCalculator));
    }

    [HttpPost]
    public IActionResult Create(UserCreateRequestDto request)
    {
        var result = userService.Create(request);

        return CreateActionResult(result, nameof(GetById), new { userId = result.Data });
    }

    [HttpPut("{userId}")]
    public IActionResult Update(int userId, UserUpdateRequestDto request)
    {
        return CreateActionResult(userService.Update(userId, request));
    }

    [HttpDelete("{userId}")]
    public IActionResult Delete(int userId)
    {
        return CreateActionResult(userService.Delete(userId));
    }

}
