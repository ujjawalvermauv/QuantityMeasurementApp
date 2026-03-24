using System;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;
using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.interfaces;
using ControllerType = QuantityMeasurementApp.QuantityMeasurementController.QuantityMeasurementController;

namespace QuantityMeasurementApp.QuantityMeasurementUI
{
    /// <summary>
    /// Handles all console-based user interaction for quantity measurement operations.
    /// </summary>
    public class Menu : IApplicationUI
    {
        private readonly ControllerType _controller;

        public Menu(ControllerType controller)
        {
            _controller = controller;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== Quantity Measurement Menu ===");
                Console.WriteLine("1) Length");
                Console.WriteLine("2) Weight");
                Console.WriteLine("3) Volume");
                Console.WriteLine("4) Temperature");
                Console.WriteLine("5) View Operation History");
                Console.WriteLine("0) Exit");
                Console.Write("Select measurement type: ");

                var categoryInput = Console.ReadLine()?.Trim();
                if (categoryInput == "0")
                    break;

                if (categoryInput == "5")
                {
                    DisplayHistory();
                    continue;
                }

                string? measurementType = categoryInput switch
                {
                    "1" => "Length",
                    "2" => "Weight",
                    "3" => "Volume",
                    "4" => "Temperature",
                    _ => null
                };

                if (measurementType == null)
                {
                    Console.WriteLine("Invalid selection. Please try again.");
                    continue;
                }

                Console.WriteLine("----------------------------");
                Console.WriteLine($"Selected: {measurementType}");
                Console.WriteLine("\nOperations:");
                Console.WriteLine("1) Compare");
                Console.WriteLine("2) Convert");
                if (measurementType != "Temperature")
                {
                    Console.WriteLine("3) Add");
                    Console.WriteLine("4) Subtract");
                    Console.WriteLine("5) Divide");
                }
                Console.WriteLine("0) Back");
                Console.Write("Select operation: ");

                var opInput = Console.ReadLine()?.Trim();
                if (opInput == "0")
                    continue;

                bool isComparison = opInput == "1";
                bool isConversion = opInput == "2";
                bool isAdd = opInput == "3";
                bool isSubtract = opInput == "4";
                bool isDivide = opInput == "5";

                if (!isComparison && !isConversion && !isAdd && !isSubtract && !isDivide)
                {
                    Console.WriteLine("Invalid operation selection.");
                    continue;
                }

                var unit1 = PromptForUnit(measurementType, "Enter first unit");
                var value1 = PromptForDouble("Enter first value");

                var dto1 = new QuantityDTO
                {
                    MeasurementType = measurementType,
                    UnitName = unit1,
                    Value = value1
                };

                if (isConversion)
                {
                    var targetUnit = PromptForUnit(measurementType, "Enter target unit");
                    try
                    {
                        var result = _controller.Convert(dto1, targetUnit);
                        Console.WriteLine($"\nSuccess: {dto1.Value} {dto1.UnitName} is converted to {result.Value} {targetUnit}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Conversion failed: {ex.Message}");
                    }
                    continue;
                }

                var unit2 = PromptForUnit(measurementType, "Enter second unit");
                var value2 = PromptForDouble("Enter second value");
                var dto2 = new QuantityDTO
                {
                    MeasurementType = measurementType,
                    UnitName = unit2,
                    Value = value2
                };

                if (isComparison)
                {
                    try
                    {
                        bool isEqual = _controller.Compare(dto1, dto2);
                        if (isEqual)
                        {
                            Console.WriteLine($"\nSuccess: {dto1.Value} {dto1.UnitName} is equal to {dto2.Value} {dto2.UnitName}");
                        }
                        else
                        {
                            double val1InBaseUnit = ConvertToBaseUnit(measurementType, dto1.Value, dto1.UnitName);
                            double val2InBaseUnit = ConvertToBaseUnit(measurementType, dto2.Value, dto2.UnitName);
                            if (val1InBaseUnit > val2InBaseUnit)
                            {
                                Console.WriteLine($"\nSuccess: {dto1.Value} {dto1.UnitName} is higher than {dto2.Value} {dto2.UnitName}");
                            }
                            else
                            {
                                Console.WriteLine($"\nSuccess: {dto2.Value} {dto2.UnitName} is higher than {dto1.Value} {dto1.UnitName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Comparison failed: {ex.Message}");
                    }
                }
                else if (isAdd)
                {
                    try
                    {
                        var result = _controller.Add(dto1, dto2);
                        Console.WriteLine($"\nSuccess: Addition of {dto1.Value} {dto1.UnitName} and {dto2.Value} {dto2.UnitName} is {result.Value} {result.UnitName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Addition failed: {ex.Message}");
                    }
                }
                else if (isSubtract)
                {
                    try
                    {
                        var result = _controller.Subtract(dto1, dto2);
                        double absoluteResult = Math.Abs(result.Value);
                        Console.WriteLine($"\nSuccess: Subtraction of {dto1.Value} {dto1.UnitName} and {dto2.Value} {dto2.UnitName} is {absoluteResult} {result.UnitName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Subtraction failed: {ex.Message}");
                    }
                }
                else if (isDivide)
                {
                    try
                    {
                        var result = _controller.Divide(dto1, dto2);
                        double absoluteResult = Math.Abs(result.Value);
                        Console.WriteLine($"\nSuccess: Division of {dto1.Value} {dto1.UnitName} and {dto2.Value} {dto2.UnitName} is {absoluteResult}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: Division failed: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Goodbye.");
        }

        private void DisplayHistory()
        {
            try
            {
                var history = _controller.GetOperationHistory();
                if (history.Count == 0)
                {
                    Console.WriteLine("\nNo operation history found.");
                    return;
                }

                Console.WriteLine("\n=== Operation History ===");

                const int colTimestamp = 19;
                const int colType = 13;
                const int colOperation = 10;
                const int colOperand1 = 22;
                const int colOperand2 = 22;
                const int colResult = 22;
                const int colError = 30;

                string header =
                    PadCell("Timestamp", colTimestamp) + " | " +
                    PadCell("Type", colType) + " | " +
                    PadCell("Operation", colOperation) + " | " +
                    PadCell("Operand1", colOperand1) + " | " +
                    PadCell("Operand2", colOperand2) + " | " +
                    PadCell("Result", colResult) + " | " +
                    PadCell("ErrorMessage", colError);

                string separator = new string('-', header.Length);

                Console.WriteLine(separator);
                Console.WriteLine(header);
                Console.WriteLine(separator);

                foreach (var item in history)
                {
                    string measurementType = !string.IsNullOrWhiteSpace(item.Operand1.MeasurementType)
                        ? item.Operand1.MeasurementType
                        : item.Result?.MeasurementType ?? "Unknown";

                    string row =
                        PadCell(item.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), colTimestamp) + " | " +
                        PadCell(measurementType, colType) + " | " +
                        PadCell(item.Operation, colOperation) + " | " +
                        PadCell(FormatQuantity(item.Operand1), colOperand1) + " | " +
                        PadCell(FormatQuantity(item.Operand2), colOperand2) + " | " +
                        PadCell(FormatQuantity(item.Result), colResult) + " | " +
                        PadCell(item.ErrorMessage ?? "-", colError);

                    Console.WriteLine(row);
                }

                Console.WriteLine(separator);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: Unable to fetch history: {ex.Message}");
            }
        }

        private static string FormatQuantity(QuantityDTO? dto)
        {
            if (dto == null)
            {
                return "-";
            }

            return $"{dto.Value:0.####} {dto.UnitName}";
        }

        private static string PadCell(string value, int width)
        {
            if (value.Length <= width)
            {
                return value.PadRight(width);
            }

            if (width <= 3)
            {
                return value.Substring(0, width);
            }

            return value.Substring(0, width - 3) + "...";
        }

        private static string PromptForUnit(string measurementType, string prompt)
        {
            while (true)
            {
                Console.WriteLine($"\n{prompt} for {measurementType}:");
                var units = GetUnitsForMeasurementType(measurementType);
                for (int i = 0; i < units.Length; i++)
                    Console.WriteLine($"{i + 1}) {units[i]}");

                Console.Write("Select option: ");
                var input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out var index) && index >= 1 && index <= units.Length)
                    return units[index - 1];

                Console.WriteLine("Invalid unit selection, try again.");
            }
        }

        private static double PromptForDouble(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine()?.Trim();
                if (double.TryParse(input, out var value))
                    return value;
                Console.WriteLine("Invalid number, please enter a numeric value.");
            }
        }

        private static string[] GetUnitsForMeasurementType(string measurementType)
        {
            return measurementType switch
            {
                "Length" => Enum.GetNames(typeof(LengthUnit)),
                "Weight" => Enum.GetNames(typeof(WeightUnit)),
                "Volume" => Enum.GetNames(typeof(VolumeUnit)),
                "Temperature" => Enum.GetNames(typeof(TemperatureUnit)),
                _ => Array.Empty<string>()
            };
        }

        /// <summary>
        /// Converts a value to base unit for comparison purposes.
        /// </summary>
        private static double ConvertToBaseUnit(string measurementType, double value, string unitName)
        {
            return measurementType switch
            {
                "Length" => ConvertLengthToBase(value, unitName),
                "Weight" => ConvertWeightToBase(value, unitName),
                "Volume" => ConvertVolumeToBase(value, unitName),
                _ => value
            };
        }

        private static double ConvertLengthToBase(double value, string unit)
        {
            return unit switch
            {
                nameof(LengthUnit.FEET) => value * 12,
                nameof(LengthUnit.INCHES) => value,
                nameof(LengthUnit.YARDS) => value * 36,
                nameof(LengthUnit.CENTIMETERS) => value * 0.393701,
                nameof(LengthUnit.MILLIMETER) => value * 0.0393701,
                _ => value
            };
        }

        private static double ConvertWeightToBase(double value, string unit)
        {
            return unit switch
            {
                nameof(WeightUnit.KILOGRAM) => value * 1000,
                nameof(WeightUnit.GRAM) => value,
                nameof(WeightUnit.TONNE) => value * 1000000,
                _ => value
            };
        }

        private static double ConvertVolumeToBase(double value, string unit)
        {
            return unit switch
            {
                nameof(VolumeUnit.LITRE) => value,
                nameof(VolumeUnit.MILLILITRE) => value * 0.001,
                nameof(VolumeUnit.GALLON) => value * 3.78541,
                _ => value
            };
        }
    }
}
