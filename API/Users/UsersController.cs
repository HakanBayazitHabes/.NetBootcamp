using API.Products;
using API.Users.DTOs;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;

namespace API.Users;

public class UsersController(UserService userService) : CustomBaseController
{
    private readonly UserService _userService = userService;

    [HttpGet]
    public IActionResult GetAll([FromServices] AgeCalculator ageCalculator)
    {
        return Ok(_userService.GetAllWithCalculatedAge(ageCalculator));
    }

    [HttpGet("{userId}")]
    public IActionResult GetById(int userId, [FromServices] AgeCalculator ageCalculator)
    {
        return CreateActionResult(_userService.GetByIdWithCalculatedAge(userId, ageCalculator));
    }

    [HttpPost]
    public IActionResult Create(UserCreateRequestDto request)
    {
        var result = _userService.Create(request);

        return CreateActionResult(result, nameof(GetById), new { userId = result.Data });
    }

    [HttpPut("{userId}")]
    public IActionResult Update(int userId, UserUpdateRequestDto request)
    {
        return CreateActionResult(_userService.Update(userId, request));
    }

    [HttpDelete("{userId}")]
    public IActionResult Delete(int userId)
    {
        return CreateActionResult(_userService.Delete(userId));
    }

}
