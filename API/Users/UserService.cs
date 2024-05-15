using System.Collections.Immutable;
using System.Net;
using API.SharedDTOs;
using API.Users.DTOs;

namespace API.Users;

public class UserService(IUserRepository userRepository) : IUserService
{
    public ResponseModelDto<int> Create(UserCreateRequestDto request)
    {
        var newUser = new User
        {
            Id = userRepository.GetAll().Count + 1,
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Age = request.Age,
            Created = DateTime.Now
        };

        userRepository.Create(newUser);

        return ResponseModelDto<int>.Success(newUser.Id, HttpStatusCode.Created);
    }

    public ResponseModelDto<NoContent> Delete(int id)
    {
        var user = userRepository.GetById(id);

        if (user is null)
        {
            return ResponseModelDto<NoContent>.Fail("User not found",
                    HttpStatusCode.NotFound);
        }
        userRepository.Delete(id);

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);

    }

    public ResponseModelDto<ImmutableList<UserDto>> GetAllWithCalculatedAge(AgeCalculator ageCalculator)
    {
        var userList = userRepository.GetAll().Select(user => new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Password,
            ageCalculator.CalculateAge(user.Age),
            user.Created.ToShortDateString()
        )).ToImmutableList();

        return ResponseModelDto<ImmutableList<UserDto>>.Success(userList);
    }

    public ResponseModelDto<UserDto> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator)
    {
        var user = userRepository.GetById(id);

        if (user is null)
        {
            return ResponseModelDto<UserDto>.Fail("User Not Found", HttpStatusCode.NotFound);
        }

        var newDto = new UserDto(
            user.Id,
            user.Name,
            user.Email,
            user.Password,
            ageCalculator.CalculateAge(user.Age),
            user.Created.ToShortDateString()
        );

        return ResponseModelDto<UserDto>.Success(newDto);
    }

    public ResponseModelDto<NoContent> Update(int userId, UserUpdateRequestDto request)
    {
        var hasUser = userRepository.GetById(userId);

        if (hasUser is null)
        {
            return ResponseModelDto<NoContent>.Fail("User not found",
                    HttpStatusCode.NotFound);
        }

        var updatedUser = new User
        {
            Id = userId,
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Age = request.Age,
            Created = hasUser.Created
        };

        userRepository.Update(updatedUser);

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }
}
