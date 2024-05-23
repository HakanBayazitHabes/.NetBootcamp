using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using Service.Users.AsyncMethod;
using Service.Users.DTOs;
using Service.Users.Filters;
using Service.Users.Helpers;
using Service.Users.UserCreateUseCase;

namespace API.Users;

public class UsersController(IUserServiceAsync userServiceAsync) : CustomBaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] AgeCalculator ageCalculator)
    {
        return Ok(await userServiceAsync.GetAllWithCalculatedAge(ageCalculator));
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetById(int userId, [FromServices] AgeCalculator ageCalculator)
    {
        return CreateActionResult(await userServiceAsync.GetByIdWithCalculatedAge(userId, ageCalculator));
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateRequestDto request)
    {
        var result = await userServiceAsync.Create(request);

        return CreateActionResult(result, nameof(GetById), new { userId = result.Data });
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpPut("{userId:int}")]
    public async Task<IActionResult> Update(int userId, UserUpdateRequestDto request)
    {
        return CreateActionResult(await userServiceAsync.Update(userId, request));
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> Delete(int userId)
    {
        return CreateActionResult(await userServiceAsync.Delete(userId));
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpPut("UpdateUserName")]
    public async Task<IActionResult> UpdateUserName(UserNameUpdateRequestDto request)
    {
        return CreateActionResult(await userServiceAsync.UpdateUserName(request.Id, request.Name));
    }

}
