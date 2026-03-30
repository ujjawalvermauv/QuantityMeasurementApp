using System;
using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Controller;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.UI
{
    public class ConsoleMenu : IConsoleMenu
    {
        private readonly IQuantityMeasurementService _service;
        private readonly IQuantityMeasurementRepository _repository;

        public ConsoleMenu(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository
        )
        {
            _service = service;
            _repository = repository;
        }

        public void Run()
        {
            var controller = new QuantityMeasurementController(_service, _repository);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Quantity Measurement - Select Category");
                Console.WriteLine("1) Length");
                Console.WriteLine("2) Weight");
                Console.WriteLine("3) Volume");
                Console.WriteLine("4) Temperature");
                Console.WriteLine("5) Show all supported units");
                Console.WriteLine("6) Show persisted history");
                Console.WriteLine("0) Exit");
                Console.Write("Select a category: ");

                var choice = Console.ReadLine()?.Trim();
                switch (choice)
                {
                    case "1":
                        RunCategoryMenu(controller, MeasurementCategory.Length);
                        break;
                    case "2":
                        RunCategoryMenu(controller, MeasurementCategory.Weight);
                        break;
                    case "3":
                        RunCategoryMenu(controller, MeasurementCategory.Volume);
                        break;
                    case "4":
                        RunCategoryMenu(controller, MeasurementCategory.Temperature);
                        break;
                    case "5":
                        PrintAvailableUnits();
                        break;
                    case "6":
                        PrintHistory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid category option.");
                        break;
                }
            }
        }

        private static void RunCategoryMenu(
            QuantityMeasurementController controller,
            MeasurementCategory category
        )
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"{category} - Select Operation");
                Console.WriteLine("1) Compare two quantities");
                Console.WriteLine("2) Convert a quantity");

                if (SupportsArithmetic(category))
                {
                    Console.WriteLine("3) Add two quantities");
                    Console.WriteLine("4) Subtract two quantities");
                    Console.WriteLine("5) Divide two quantities");
                    Console.WriteLine("6) Show supported units for this category");
                    Console.WriteLine("0) Back to categories");
                }
                else
                {
                    Console.WriteLine("3) Show supported units for this category");
                    Console.WriteLine("0) Back to categories");
                }

                Console.Write("Select an operation: ");
                var operation = Console.ReadLine()?.Trim();

                if (operation == "0")
                    return;

                try
                {
                    HandleOperation(controller, category, operation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void HandleOperation(
            QuantityMeasurementController controller,
            MeasurementCategory category,
            string? operation
        )
        {
            if (operation == "1")
            {
                var first = PromptQuantity(category, "first");
                var second = PromptQuantity(category, "second");
                controller.PerformCompare(first, second);
                return;
            }

            if (operation == "2")
            {
                var source = PromptQuantity(category, "source");
                var targetUnit = PromptUnit(category, "Enter target unit");
                controller.PerformConvert(source, targetUnit);
                return;
            }

            if (SupportsArithmetic(category))
            {
                if (operation == "3")
                {
                    var first = PromptQuantity(category, "first");
                    var second = PromptQuantity(category, "second");
                    var targetUnit = PromptOptionalTargetUnit(category);
                    controller.PerformAdd(first, second, targetUnit);
                    return;
                }

                if (operation == "4")
                {
                    var first = PromptQuantity(category, "first");
                    var second = PromptQuantity(category, "second");
                    var targetUnit = PromptOptionalTargetUnit(category);
                    controller.PerformSubtract(first, second, targetUnit);
                    return;
                }

                if (operation == "5")
                {
                    var first = PromptQuantity(category, "first");
                    var second = PromptQuantity(category, "second");
                    controller.PerformDivide(first, second);
                    return;
                }

                if (operation == "6")
                {
                    PrintUnitsForCategory(category);
                    return;
                }

                Console.WriteLine("Invalid operation option.");
                return;
            }

            if (operation == "3")
            {
                PrintUnitsForCategory(category);
                return;
            }

            Console.WriteLine("Invalid operation option.");
        }

        private static QuantityDTO PromptQuantity(MeasurementCategory category, string label)
        {
            var value = PromptDouble($"Enter {label} value: ");
            var unit = PromptUnit(category, $"Enter {label} unit");
            return new QuantityDTO(value, unit, category);
        }

        private static double PromptDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (double.TryParse(input, out var value))
                    return value;

                Console.WriteLine("Invalid number. Please enter a valid numeric value.");
            }
        }

        private static string PromptUnit(MeasurementCategory category, string prompt)
        {
            var units = GetUnits(category);
            var lookup = units.ToDictionary(
                unit => unit,
                unit => unit,
                StringComparer.OrdinalIgnoreCase
            );

            while (true)
            {
                Console.WriteLine($"Available units: {string.Join(", ", units)}");
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(input) && lookup.TryGetValue(input, out var matched))
                    return matched;

                Console.WriteLine("Invalid unit for selected category.");
            }
        }

        private static string? PromptOptionalTargetUnit(MeasurementCategory category)
        {
            Console.Write("Use specific target unit for result? (y/N): ");
            var choice = Console.ReadLine()?.Trim();
            if (!string.Equals(choice, "y", StringComparison.OrdinalIgnoreCase))
                return null;

            return PromptUnit(category, "Enter target unit for result");
        }

        private static bool SupportsArithmetic(MeasurementCategory category)
        {
            return category != MeasurementCategory.Temperature;
        }

        private static IReadOnlyList<string> GetUnits(MeasurementCategory category)
        {
            return category switch
            {
                MeasurementCategory.Length => Enum.GetNames(typeof(LengthUnit)),
                MeasurementCategory.Weight => Enum.GetNames(typeof(WeightUnit)),
                MeasurementCategory.Volume => Enum.GetNames(typeof(VolumeUnit)),
                MeasurementCategory.Temperature => Enum.GetNames(typeof(TemperatureUnit)),
                _ => Array.Empty<string>(),
            };
        }

        private static void PrintUnitsForCategory(MeasurementCategory category)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Supported {category} units: {string.Join(", ", GetUnits(category))}"
            );
        }

        private static void PrintAvailableUnits()
        {
            Console.WriteLine();
            Console.WriteLine("Supported categories and units:");
            Console.WriteLine($"Length: {string.Join(", ", Enum.GetNames(typeof(LengthUnit)))}");
            Console.WriteLine($"Weight: {string.Join(", ", Enum.GetNames(typeof(WeightUnit)))}");
            Console.WriteLine($"Volume: {string.Join(", ", Enum.GetNames(typeof(VolumeUnit)))}");
            Console.WriteLine(
                $"Temperature: {string.Join(", ", Enum.GetNames(typeof(TemperatureUnit)))}"
            );
        }

        private void PrintHistory()
        {
            Console.WriteLine();
            Console.WriteLine("Persisted operation history:");

            foreach (var entry in _repository.GetAll())
            {
                var status = entry.IsError ? "ERROR" : "OK";
                var errorPart = entry.IsError ? $" | {entry.ErrorMessage}" : string.Empty;
                Console.WriteLine(
                    $"[{entry.CreatedAt:yyyy-MM-dd HH:mm:ss}] {status} | {entry.Description}{errorPart}"
                );
            }
        }
    }
}
