using Microsoft.EntityFrameworkCore;
using omni_multitool.Data.Models;

namespace omni_multitool.Data;

public class SettingsService
{
    private readonly OmniDbContext _db;

    public SettingsService()
    {
        _db = new OmniDbContext();
    }

    public SettingsService(OmniDbContext db)
    {
        _db = db;
    }

    public async Task<AppSettings> GetAppSettingsAsync()
    {
        var settings = await _db.AppSettings.FirstOrDefaultAsync();

        if (settings is not null)
            return settings;

        settings = new AppSettings
        {
            StartWithWindows = false,
            MinimizeToTray = false,
            ShowWelcomePage = true
        };

        _db.AppSettings.Add(settings);
        await _db.SaveChangesAsync();

        return settings;
    }

    public async Task SaveAppSettingsAsync(AppSettings settings)
    {
        _db.AppSettings.Update(settings);
        await _db.SaveChangesAsync();
    }

    public async Task<WaterReminderSettings> GetWaterReminderSettingsAsync()
    {
        var settings = await _db.WaterReminderSettings.FirstOrDefaultAsync();

        if (settings is not null)
            return settings;

        settings = new WaterReminderSettings
        {
            IsEnabled = false,
            IntervalInMinutes = 60,
            PlaySound = true
        };

        _db.WaterReminderSettings.Add(settings);
        await _db.SaveChangesAsync();

        return settings;
    }

    public async Task SaveWaterReminderSettingsAsync(WaterReminderSettings settings)
    {
        _db.WaterReminderSettings.Update(settings);
        await _db.SaveChangesAsync();
    }
}