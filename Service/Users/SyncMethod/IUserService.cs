using System.Collections.Immutable;
using Service.SharedDTOs;
using Service.Users.DTOs;
using Service.Users.Helpers;
using Service.Users.UserCreateUseCase;

namespace Service.Users.SyncMethod;

public interface IUserService
{
    ResponseModelDto<ImmutableList<UserDto>> GetAllWithCalculatedAge(AgeCalculator ageCalculator);

    ResponseModelDto<UserDto> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator);

    ResponseModelDto<int> Create(UserCreateRequestDto request);

    ResponseModelDto<NoContent> Update(int userId, UserUpdateRequestDto request);

    ResponseModelDto<NoContent> Delete(int id);
}
