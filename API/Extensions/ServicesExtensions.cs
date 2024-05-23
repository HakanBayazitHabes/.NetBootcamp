using AspNetCoreRateLimit;

namespace API.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
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


        }
    }
}