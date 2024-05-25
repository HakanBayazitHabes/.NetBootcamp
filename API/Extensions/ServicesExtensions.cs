using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Products.Configurations;
using Service.Users.Configurations;
using Service.Roles.Configurations;
using Service.Logs.Configurations;
using API.Filters;
using Service.Products.ProductCreateUseCase;
using FluentValidation;
using Service.Roles.RoleCreateUseCase;
using Service.Users.UserCreateUseCase;
using NLog;


namespace API.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            LogManager.LoadConfiguration(String.Concat(AppDomain.CurrentDomain.BaseDirectory, "nlog.config"));

            var rateLimistRules = new List<RateLimitRule>(){
                new RateLimitRule{
                    Endpoint = "*",
                    Limit = 1,
                    Period = "5s"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimistRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddMemoryCache();
            services.AddSingleton<LogFilterAttribute>();


        }

        public static void AddServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(x => { x.SuppressModelStateInvalidFilter = true; });
            services.AddAutoMapper(typeof(ServiceAssembly).Assembly);
            services.AddFluentValidationAutoValidation();

            //RuleFor Entity
            services.AddValidatorsFromAssemblyContaining<ProductCreateRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleCreateRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<UserCreateRequestValidator>();

            services.AddProductService();
            services.AddUserService();
            services.AddRoleService();
            services.AddLogService();
        }
    }
}