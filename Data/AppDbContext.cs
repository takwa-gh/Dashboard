using Microsoft.EntityFrameworkCore;
using Dashboard.Models;

namespace Dashboard.Data
{
    public class AppDbContext : DbContext
    {
        //Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //DbSet for Users
        public DbSet<User> Users { get; set; }
        //DbSet for Stations
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationAWT> StationAWTs { get; set; }
        public DbSet<StationGUM> StationGUMs { get; set; }
        //DbSet for Lines
        public DbSet<LineParam> LineParams { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }

}
