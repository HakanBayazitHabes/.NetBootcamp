using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Repository.Roles;
using Service.Roles.AsyncMethod;
using Service.Roles.Filters;
using Service.Roles.RoleCreateUseCase;

namespace Service.Roles.Configurations;

public static class RoleServiceExt
{
    public static void AddRoleService(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepositoryAsync, RoleRepositoryAsync>();
        services.AddScoped<IRoleServiceAsync, RoleServiceAsync>();

        services.AddScoped<NotFoundFilter>();

        services.AddValidatorsFromAssemblyContaining<RoleCreateRequestValidator>();
    }
}
