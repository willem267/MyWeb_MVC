using Microsoft.EntityFrameworkCore;
using Web.DataAccess.Data;
using Web.DataAccess.Repository;
using Web.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Web.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Web.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options
    =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    

});
//facebook authentication
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = builder.Configuration["Facebook:AppId"];
    options.AppSecret = builder.Configuration["Facebook:AppSecret"];

});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientID"];
    options.ClientSecret = builder.Configuration["Google:ClientSecret"];

});
//add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
seedDatabase(); //seed database
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

//DbInitializer
void seedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
       var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
