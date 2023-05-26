
using JokesMVC2023.Models.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<JokeDBContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("JokesDBSQL")));

        builder.Services.AddSession();
        builder.Services.AddDistributedMemoryCache();


        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM www.youtube.com");
            await next(context);
        });

        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}