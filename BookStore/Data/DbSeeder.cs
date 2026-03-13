using BookStore.Models;

namespace BookStore.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmin(AppDbContext context)
        {
            if (!context.Users.Any(u => u.Email == "hemanthsure@gmail.com"))
            {
                var admin = new User
                {
                    Name = "Admin",
                    Email = "hemanthsure@gmail.com",
                    Password = "1234567890",
                    Role = "Admin",
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}