using _16noyabr.DAL;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt => 
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllerRoute(
                "default",
                "{area:exists}/{controller=home}/{action=index}/{id?}"

                );

            app.MapControllerRoute(
            "default",
            "{controller=home}/{action=index}/{id?}"
            );
            app.Run();
        }
       
    }
}