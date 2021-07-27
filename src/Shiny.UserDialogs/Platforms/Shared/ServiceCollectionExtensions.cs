using Microsoft.Extensions.DependencyInjection;
using Shiny.UserDialogs;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static bool UseUserDialogs(this IServiceCollection services)
        {
#if __ANDROID__ || __IOS__
            services.AddSingleton<IUserDialogs, UserDialogsImpl>();
            return true;
#else
            return false;
#endif
        }
    }
}
