using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kysect.Configuin.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOptionsWithValidation<T>(this IServiceCollection serviceCollection, string name, bool validateOnStart = true) where T : class
        {
            OptionsBuilder<T> builder = serviceCollection
                .AddOptions<T>()
                .BindConfiguration(name)
                .ValidateDataAnnotations();

            if (validateOnStart)
                builder = builder.ValidateOnStart();

            return builder.Services;
        }
    }
}