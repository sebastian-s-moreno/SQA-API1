using System.Reflection;

namespace Forte.Location.Api;
public static class OpenApiInstaller
{
    public static void AddOpenApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        var title = configuration.GetSection("OpenApi")["Title"];
        var xmlDocumentation = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentation);

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = title,
                Version = "v1",
                Description = "Forte Academy building weather api"
            });

            options.IncludeXmlComments(xmlPath);

        });
    }

    public static void UseOpenApi(this IApplicationBuilder app)
    {
        app.UseStaticFiles();
        app.UseSwagger(options => options.RouteTemplate = "docs/{documentName}/docs.json");
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "docs";
            options.DocumentTitle = "Weather api";
            options.InjectStylesheet("/swagger-ui/custom.css");
            options.SwaggerEndpoint("/docs/v1/docs.json", "Forte.Location.Api v1");
        });
    }
}