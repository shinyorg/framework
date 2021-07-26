using System;


namespace Shiny
{
    public static class ServiceCollectionExtensions
    {
        public static bool UseUserDialogs(this IServiceCollection services)
        {
            return false;
        }
    }
}
