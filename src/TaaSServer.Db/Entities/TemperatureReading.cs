using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaaSServer.Db.Entities;

[Index(nameof(DeviceId), nameof(Timestamp))]
public class TemperatureReading
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    [MaxLength(64)]
    public required string DeviceId { get; set; }
    public float Temperature { get; set; }
}