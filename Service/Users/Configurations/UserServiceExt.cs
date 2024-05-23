using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Repository.Users;
using Service.Users.AsyncMethod;
using Service.Users.Filters;
using Service.Users.Helpers;
using Service.Users.SyncMethod;
using Service.Users.UserCreateUseCase;

namespace Service.Users.Configurations;

public static class UserServiceExt
{
    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUserRepositoryAsync, UserRepositoryAsync>();
        services.AddScoped<IUserServiceAsync, UserServiceAsync>();

        services.AddValidatorsFromAssemblyContaining<UserCreateRequestValidator>();

        services.AddScoped<NotFoundFilter>();

        services.AddSingleton<AgeCalculator>();
    }
}
