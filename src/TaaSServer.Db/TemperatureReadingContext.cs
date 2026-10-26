using Microsoft.EntityFrameworkCore;
using TaaSServer.Db.Entities;

namespace TaaSServer.Db;

public class TemperatureReadingContext(DbContextOptions<TemperatureReadingContext> options) : DbContext(options)
{
    public DbSet<TemperatureReading> TemperatureReadings { get; set; }
}