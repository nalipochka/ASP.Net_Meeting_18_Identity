using ASP.Net_Meeting_18_Identity.AutoMapperProfiles;
using ASP.Net_Meeting_18_Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ShopDbContext>();

builder.Services.AddAutoMapper(typeof(UserProfiles));

string connStr = builder.Configuration.GetConnectionString("testShopDb");

builder.Services.AddSession();

builder.Services.AddDbContext<ShopDbContext>(options =>
options.UseSqlServer(connStr));

builder.Services.AddAuthentication().AddGoogle(options =>
{
    IConfigurationSection googleSection = configuration.GetSection("Authentication:Google");
    options.ClientId = googleSection.GetValue<string>("ClientId");
    options.ClientSecret = googleSection["ClientSecret"];
}).AddFacebook(fbOptions =>
{
    IConfigurationSection fbSection = configuration.GetSection("Authentication:Facebook");
    fbOptions.AppId = fbSection.GetSection("AppId").Value;
    fbOptions.AppSecret = fbSection.GetSection("AppSecret").Value;
});

//builder.Services.AddDistributedMemoryCache();


builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("FrameworkPolicy", policy =>
    {
        policy.RequireClaim("PrefferedFramework", new[] { "ASP.NET Core" });
        policy.RequireRole("admin", "manager");
    });
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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
