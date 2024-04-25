using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Service;
using HotelBookingWeb.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


builder.Services.AddHttpClient<IRoomTypeService, RoomTypeService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

SD.RoomTypeAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
