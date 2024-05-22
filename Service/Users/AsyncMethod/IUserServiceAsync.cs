using System.Collections.Immutable;
using Service.SharedDTOs;
using Service.Users.DTOs;
using Service.Users.Helpers;
using Service.Users.UserCreateUseCase;

namespace Service.Users.AsyncMethod
{
    public interface IUserServiceAsync
    {
        Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllWithCalculatedAge(AgeCalculator ageCalculator);

        Task<ResponseModelDto<UserDto?>> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator);

        Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllByPageWithCalculatedAge(
            AgeCalculator ageCalculator, int page, int pageSize);

        Task<ResponseModelDto<int>> Create(UserCreateRequestDto request);

        Task<ResponseModelDto<NoContent>> Update(int userId, UserUpdateRequestDto request);

        Task<ResponseModelDto<NoContent>> UpdateUserName(int id, string name);

        Task<ResponseModelDto<NoContent>> Delete(int id);
    }
}