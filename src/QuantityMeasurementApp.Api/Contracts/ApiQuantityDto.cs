using System.ComponentModel.DataAnnotations;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Api.Contracts;

public class ApiQuantityDto
{
    [Required]
    public double? Value { get; set; }

    [Required]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(MeasurementCategory))]
    public MeasurementCategory Category { get; set; }
}
