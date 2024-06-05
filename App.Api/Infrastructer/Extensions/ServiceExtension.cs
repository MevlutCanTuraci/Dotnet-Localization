#region Imports
using App.Api.Infrastructer.Helper.Localize;
using Microsoft.AspNetCore.Localization;
#endregion


namespace App.Api.Infrastructer.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureLocalize(this IServiceCollection service)
        {
            service.AddTransient<ILocalize, LocalizeHelper>();

            service.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            service.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = LocalizeFileHelper.GetCultures();

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var languages = context.Request.Headers["Accept-Language"].ToString();
                    var currentLanguage = languages.Split(',').FirstOrDefault();
                    var defaultLanguage = string.IsNullOrEmpty(currentLanguage) ? "tr" : currentLanguage;

                    if (!supportedCultures.Contains(new System.Globalization.CultureInfo(defaultLanguage)))
                    {
                        defaultLanguage = "tr";
                    }

                    return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });

        }
    }
}