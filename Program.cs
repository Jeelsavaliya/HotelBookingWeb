using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Service;
using HotelBookingWeb.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Stripe;
using HotelBookingWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddTransient<IMailService, MailService>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IRoomTypeService, RoomTypeService>();
builder.Services.AddHttpClient<IRoomService, RoomService>();
builder.Services.AddHttpClient<IBookingRoomService, BookingRoomService>();
builder.Services.AddHttpClient<ICheckAvailabilityService, CheckAvailabilityService>();
builder.Services.AddHttpClient<IMailService, MailService>();
builder.Services.AddHttpClient<IUserService, UserService>();

SD.RoomTypeAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.RoomAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.AuthAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.BookingRoomAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.CheckAvailabilityAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.EmialAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];
SD.UserAPIBase = builder.Configuration["ServiceUrls:HotelBookingAPI"];

builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingRoomService, BookingRoomService>();
builder.Services.AddScoped<ICheckAvailabilityService, CheckAvailabilityService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();

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
