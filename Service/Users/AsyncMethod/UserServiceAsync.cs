using System.Collections.Immutable;
using System.Net;
using System.Text.Json;
using AutoMapper;
using Repository;
using Repository.Redis;
using Repository.Users;
using Service.SharedDTOs;
using Service.Users.DTOs;
using Service.Users.Helpers;
using Service.Users.UserCreateUseCase;

namespace Service.Users.AsyncMethod
{
    public class UserServiceAsync(IUserRepositoryAsync userRepositoryAsync, RedisService redisService, IUnitOfWork unitOfWork, IMapper mapper) : IUserServiceAsync
    {
        private const string UserCacheKey = "users";
        private const string UserCacheKeyAsList = "users-list";

        public async Task<ResponseModelDto<int>> Create(UserCreateRequestDto request)
        {
            redisService.Database.KeyDelete(UserCacheKey);

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                CreatedDate = DateTime.Now
            };

            await userRepositoryAsync.Create(newUser);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<int>.Success(newUser.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int id)
        {
            redisService.Database.KeyDelete(UserCacheKey);
            await userRepositoryAsync.Delete(id);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllByPageWithCalculatedAge(AgeCalculator ageCalculator, int page, int pageSize)
        {
            var userList = await userRepositoryAsync.GetAllByPage(page, pageSize);

            var productListAsDto = mapper.Map<List<UserDto>>(userList);

            return ResponseModelDto<ImmutableList<UserDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ImmutableList<UserDto>>> GetAllWithCalculatedAge(AgeCalculator ageCalculator)
        {
            if (redisService.Database.KeyExists(UserCacheKey))
            {
                var users = redisService.Database.StringGet(UserCacheKey);
                var usersListFromCache = JsonSerializer.Deserialize<ImmutableList<UserDto>>(users);
                return ResponseModelDto<ImmutableList<UserDto>>.Success(usersListFromCache.ToImmutableList());
            }

            var userList = await userRepositoryAsync.GetAll();

            var productListAsDto = mapper.Map<List<UserDto>>(userList);

            var usersListAsJson = JsonSerializer.Serialize(productListAsDto);
            redisService.Database.StringSet(UserCacheKey, usersListAsJson);

            productListAsDto.ForEach(user =>
            {
                redisService.Database.ListLeftPush($"{UserCacheKeyAsList}:{user.Id}", JsonSerializer.Serialize(user));
            });

            return ResponseModelDto<ImmutableList<UserDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<UserDto?>> GetByIdWithCalculatedAge(int id, AgeCalculator ageCalculator)
        {
            var customKey = $"{UserCacheKeyAsList}:{id}";

            if (redisService.Database.KeyExists(customKey))
            {
                var userAsJsonFromCache = redisService.Database.ListGetByIndex(customKey, 0);
                var userFromCache = JsonSerializer.Deserialize<UserDto>(userAsJsonFromCache);
                return ResponseModelDto<UserDto?>.Success(userFromCache);
            }

            var hasUser = await userRepositoryAsync.GetById(id);

            // if (hasUser is null)
            // {
            //     return ResponseModelDto<UserDto?>.Fail("User not found", HttpStatusCode.NotFound);
            // }

            redisService.Database.ListLeftPush($"{UserCacheKeyAsList}:{hasUser.Id}", JsonSerializer.Serialize(hasUser));

            var userAsDto = mapper.Map<UserDto>(hasUser);

            return ResponseModelDto<UserDto?>.Success(userAsDto);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int userId, UserUpdateRequestDto request)
        {
            redisService.Database.KeyDelete(UserCacheKey);

            var hasUser = await userRepositoryAsync.GetById(userId);

            // if (hasUser is null)
            // {
            //     return ResponseModelDto<NoContent>.Fail("User not found", HttpStatusCode.NotFound);
            // }

            hasUser.Name = request.Name;
            hasUser.Email = request.Email;
            hasUser.Age = request.Age;

            await userRepositoryAsync.Update(hasUser);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);

        }

        public async Task<ResponseModelDto<NoContent>> UpdateUserName(int id, string name)
        {
            redisService.Database.KeyDelete(UserCacheKey);

            await userRepositoryAsync.UpdateUserName(name, id);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}