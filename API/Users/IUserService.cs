using System.Collections.Immutable;
using API.SharedDTOs;
using API.Users.DTOs;

namespace API.Users;

public interface IUserService
{
    ResponseModelDto<ImmutableList<UserDto>> GetAllWithCalculatedAge(AgeCalculator ageCalculator);

    ResponseModelDto<UserDto> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator);

    ResponseModelDto<int> Create(UserCreateRequestDto request);

    ResponseModelDto<NoContent> Update(int userId, UserUpdateRequestDto request);

    ResponseModelDto<NoContent> Delete(int id);
}
