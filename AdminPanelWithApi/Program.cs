using Auth.DependencyInjection;
using Auth.Middlewares;
using Localization.Setting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Globalization;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
 
var _policyName = "CorsPolicy";
// Add services to the container.
builder.Services.AddHttpContextAccessor();


builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityServer(builder.Configuration);
builder.Services.AddCorsPolicy(_policyName);

builder.Services.AddLocalization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddMvc().AddDataAnnotationsLocalization(options =>
{
   
    options.DataAnnotationLocalizerProvider = (type, factory) =>
        factory.Create(typeof(JsonStringLocalizerFactory));
}).AddSessionStateTempDataProvider();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ar"),
        new CultureInfo("de")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.OperationFilter<AddAcceptLanguageHeaderParameter>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers()
     .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Register the Swagger generator, defining 1 or more Swagger documents

await builder.Services.AddSeedAsync();
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));

var app = builder.Build();




// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
var supportedCultures = new[] { "en", "ar", "de" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseSerilogRequestLogging();
app.UseSession();
app.UseAuthentication();

app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(builder.Configuration["SwaggerSettings:Endpoint"], "Auth API v1");
   

});

app.UseRouting(); 
app.UseAuthorization();

app.UseCors(_policyName);

app.Map("/Account/Login", loginApp =>
{
    loginApp.Run(async context =>
    {
        // Redirect unauthenticated users to the login page
        context.Response.Redirect("/Identity/Account/Login"); // Adjust the URL as per your Identity login route
    });
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");



app.MapRazorPages();

app.Run();
