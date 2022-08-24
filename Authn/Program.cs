using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
        options.Events = new OpenIdConnectEvents()
        {
            OnTokenValidated = async context =>
            {
                if(context.Principal.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier).Value== "101977722387594596976")
                {
                    var claim = new Claim(ClaimTypes.Role, "Admin");
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity.AddClaim(claim);
                }
                var claims = context.Principal.Claims;
            }
        };
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
