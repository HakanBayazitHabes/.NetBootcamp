using System.Collections.Immutable;
using AutoMapper;
using Repository;
using Repository.Users;
using Service.SharedDTOs;
using Service.Users.DTOs;
using Service.Users.Helpers;
using Service.Users.UserCreateUseCase;

namespace Service.Users.AsyncMethod
{
    public class UserServiceAsync(IUserRepositoryAsync userRepositoryAsync, IUnitOfWork unitOfWork, IMapper mapper) : IUserServiceAsync
    {
        public async Task<ResponseModelDto<int>> Create(UserCreateRequestDto request)
        {
            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                CreatedDate = DateTime.Now
            };

            await userRepositoryAsync.Create(newUser);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<int>.Success(newUser.Id, System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int id)
        {
            await userRepositoryAsync.Delete(id);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(System.Net.HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllByPageWithCalculatedAge(AgeCalculator ageCalculator, int page, int pageSize)
        {
            var userList = await userRepositoryAsync.GetAllByPage(page, pageSize);

            var productListAsDto = mapper.Map<List<UserDto>>(userList);

            return ResponseModelDto<ImmutableList<UserDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllWithCalculatedAge(AgeCalculator ageCalculator)
        {
            var userList = await userRepositoryAsync.GetAll();

            var productListAsDto = mapper.Map<List<UserDto>>(userList);

            return ResponseModelDto<ImmutableList<UserDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<UserDto?>> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator)
        {
            var hasUser = await userRepositoryAsync.GetById(id);

            if (hasUser is null)
            {
                return ResponseModelDto<UserDto?>.Fail("User not found", System.Net.HttpStatusCode.NotFound);
            }

            var userAsDto = mapper.Map<UserDto>(hasUser);

            return ResponseModelDto<UserDto?>.Success(userAsDto);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int userId, UserUpdateRequestDto request)
        {
            var hasUser = await userRepositoryAsync.GetById(userId);

            if (hasUser is null)
            {
                return ResponseModelDto<NoContent>.Fail("User not found", System.Net.HttpStatusCode.NotFound);
            }

            hasUser.Name = request.Name;
            hasUser.Email = request.Email;
            hasUser.Age = request.Age;

            await userRepositoryAsync.Update(hasUser);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(System.Net.HttpStatusCode.NoContent);

        }

        public async Task<ResponseModelDto<NoContent>> UpdateUserName(int id, string name)
        {
            await userRepositoryAsync.UpdateUserName(name, id);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(System.Net.HttpStatusCode.NoContent);
        }
    }
}