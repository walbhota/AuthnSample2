using Authn.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AuthDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(
    options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/denied";
        options.Events = new CookieAuthenticationEvents()
        {
            OnSigningIn = async context =>
            {
                var scheme = context.Properties.Items.Where(k => k.Key == ".AuthScheme").FirstOrDefault();
                var claim = new Claim(scheme.Key, scheme.Value);
                var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                claimsIdentity.AddClaim(claim);
            }
        };
    })
    .AddOpenIdConnect("google", options =>
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
                if (context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == "101977722387594596976")
                {
                    var claim = new Claim(ClaimTypes.Role, "Admin");
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    claimsIdentity.AddClaim(claim);
                }
                var claims = context.Principal.Claims;
            }
        };
    }).AddOpenIdConnect("okta", options =>
    {
        options.Authority = "https://dev-29932433.okta.com/oauth2/default";
        options.ClientId = "0oa6a4kkrx9A86Eb25d7";
        options.ClientSecret = "Qrwxuhz24BtL5luFbt5wPWz-gGcKvQvayeZXXuqb";
        options.CallbackPath = "/authorization-code/callback";
        options.SignedOutCallbackPath = "/Account/PostLogout";
        options.ResponseType = "code";
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("offline_access");
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
