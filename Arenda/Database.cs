using System.Linq;
using Arenda.Data;
using Arenda.Models;
using Microsoft.EntityFrameworkCore;

namespace Arenda
{
    public static class Database
    {
        public static ResidentialProperty GetRealEstateById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.ResidentialProperties
                    .Include(r => r.Photos)
                    .Include(r => r.Category)
                    .Include(r => r.City)
                    .Include(r => r.Owner)
                    .FirstOrDefault(r => r.Id == id);
            }
        }

        public static User GetUserById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Id == id);
            }
        }
    }
}