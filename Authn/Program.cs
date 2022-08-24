using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GoogleOpenID";
    })
    .AddCookie(
    options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/denied";
    })
    .AddOpenIdConnect("GoogleOpenID", options =>
    {
        options.Authority = "https://accounts.google.com";
        options.ClientId = "435073184014-3059uis6u3511jecqsp5atvne5k523n3.apps.googleusercontent.com";
        options.ClientSecret = "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";
        options.CallbackPath = "/auth";
        options.SaveTokens = true;
    });
    //.AddGoogle(options =>
    //{
    //    options.ClientId = "435073184014-3059uis6u3511jecqsp5atvne5k523n3.apps.googleusercontent.com";
    //    options.ClientSecret = "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";
    //    options.CallbackPath = "/auth";
    //    //Prompt to consent, to select from list of account after logout, when trying to log back in has been taking cared of in latest .Net
    //    //So the AuthorizationEndpoint is not necessary needed, but take note when dealing with other versions.
    //    options.AuthorizationEndpoint += "?prompt=consent";
    //}
    //);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
