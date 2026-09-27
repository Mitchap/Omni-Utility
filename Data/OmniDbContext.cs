using Microsoft.EntityFrameworkCore;
using omni_multitool.Data.Models;

namespace omni_multitool.Data;

public class OmniDbContext : DbContext
{
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    public DbSet<WaterReminderSettings> WaterReminderSettings => Set<WaterReminderSettings>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=omni.db");
    }
}