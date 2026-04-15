using System;

namespace QuantityMeasurementApp.Models.Entities;

public class OperationHistory
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}