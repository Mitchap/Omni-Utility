using Microsoft.EntityFrameworkCore;
using omni_multitool.Data.Models;
using System.IO;

namespace omni_multitool.Data;

public class OmniDbContext : DbContext
{
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();
    public DbSet<WaterReminderSettings> WaterReminderSettings => Set<WaterReminderSettings>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //%LOCALAPPDATA%/OMNI Utility Suite/omni.db


        string appDataPath = Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData);

        string omniPath = Path.Combine(
            appDataPath,
            "OMNI Utility Suite");

        Directory.CreateDirectory(omniPath);

        string databasePath = Path.Combine(
            omniPath,
            "omni.db");

        optionsBuilder.UseSqlite($"Data Source={databasePath}");
    }
}