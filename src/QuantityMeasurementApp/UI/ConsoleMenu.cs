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
    /// <summary>
    /// ConsoleMenu - User Interface Layer
    /// 
    /// What it does:
    /// - Presents interactive console-based menu to users
    /// - Guides users through measurement operations (compare, convert, add, subtract, divide)
    /// - Handles user input validation and error handling
    /// - Displays available units, operation history, and results
    /// 
    /// How it works:
    /// 1. Shows main menu with measurement categories (Length, Weight, Volume, Temperature)
    /// 2. User selects category
    /// 3. Shows category-specific operations menu
    /// 4. Prompts user for quantities and units
    /// 5. Calls controller to perform operation
    /// 6. Displays result to user
    /// 
    /// Separation of Concerns:
    /// - UI layer: This ConsoleMenu class (presentation logic)
    /// - Controller layer: QuantityMeasurementController (orchestration)
    /// - Service layer: IQuantityMeasurementService (business logic)
    /// - Repository layer: IQuantityMeasurementRepository (data persistence)
    /// </summary>
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

        /// <summary>
        /// Displays main menu and handles user interactions in infinite loop
        /// 
        /// User flow:
        /// 1. Shows available measurement categories
        /// 2. Processes user choice
        /// 3. Routes to category menu or utility functions
        /// 4. Continues until user selects "Exit" (0)
        /// 
        /// Default Behavior:
        /// - Shows menu infinitely until explicit exit command
        /// - Displays friendly "Invalid option" message for bad input
        /// </summary>
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

        /// <summary>
        /// Displays operation menu for selected measurement category
        /// 
        /// Parameters:
        /// - controller: Controller instance to execute operations
        /// - category: Selected measurement category (Length, Weight, Volume, Temperature)
        /// 
        /// How it works:
        /// 1. Shows category-appropriate operations menu
        /// 2. Temperature has limited operations (no addition/subtraction)
        /// 3. Other categories support all operations
        /// 4. User selects operation or returns to main menu
        /// 
        /// Conditional Menus:
        /// - Temperature: Compare, Convert, Show units (4 operations)
        /// - Other categories: Compare, Convert, Add, Subtract, Divide, Show units (6 operations)
        /// </summary>
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

        /// <summary>
        /// Routes operation request to appropriate controller method
        /// 
        /// Parameters:
        /// - controller: Controller instance
        /// - category: Selected measurement category
        /// - operation: User's operation choice (as string "1"-"6")
        /// 
        /// Operations:
        /// - "1": Compare two quantities
        /// - "2": Convert quantity to different unit
        /// - "3": Add quantities (arithmetic-supporting categories) or Show units (Temperature)
        /// - "4": Subtract quantities (arithmetic-supporting categories only)
        /// - "5": Divide quantities (arithmetic-supporting categories only)
        /// - "6": Show units (arithmetic-supporting categories only)
        /// 
        /// Error Handling:
        /// - Displays "Invalid operation option" for unrecognized choice
        /// - Caller handles exceptions and displays error messages
        /// </summary>
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

        /// <summary>
        /// Prompts user to enter a quantity (value and unit)
        /// 
        /// Parameters:
        /// - category: Measurement category for context
        /// - label: Display label for the quantity (e.g., "first", "second", "source")
        /// 
        /// Returns:
        /// - QuantityDTO with user-entered value, unit, and category
        /// 
        /// User Flow:
        /// 1. Prompts for numeric value
        /// 2. Prompts for unit selection (shows available units)
        /// 3. Returns complete QuantityDTO
        /// </summary>
        private static QuantityDTO PromptQuantity(MeasurementCategory category, string label)
        {
            var value = PromptDouble($"Enter {label} value: ");
            var unit = PromptUnit(category, $"Enter {label} unit");
            return new QuantityDTO(value, unit, category);
        }

        /// <summary>
        /// Prompts user to enter a numeric value with validation
        /// 
        /// Parameters:
        /// - prompt: Display prompt message
        /// 
        /// Returns:
        /// - Valid double value entered by user
        /// 
        /// Validation:
        /// - Loops until valid numeric input received
        /// - Shows "Invalid number" message for non-numeric input
        /// - Accepts integers and decimal numbers
        /// </summary>
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

        /// <summary>
        /// Prompts user to select a unit from available options for category
        /// 
        /// Parameters:
        /// - category: Measurement category
        /// - prompt: Display message
        /// 
        /// Returns:
        /// - Valid unit name (case-insensitive match)
        /// 
        /// Features:
        /// - Shows all available units for category
        /// - Case-insensitive comparison (accepts "meter", "METER", "Meter")
        /// - Loops until valid unit entered
        /// - Uses dictionary lookup for efficient matching
        /// </summary>
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

        /// <summary>
        /// Optional prompt for target unit in arithmetic operations
        /// 
        /// Returns:
        /// - Target unit name if user selects 'y', null if selects 'n' or leaves blank
        /// 
        /// Default Behavior:
        /// - Returns null (first quantity's unit will be used for result)
        /// - Only called for arithmetic operations (Add, Subtract)
        /// </summary>
        private static string? PromptOptionalTargetUnit(MeasurementCategory category)
        {
            Console.Write("Use specific target unit for result? (y/N): ");
            var choice = Console.ReadLine()?.Trim();
            if (!string.Equals(choice, "y", StringComparison.OrdinalIgnoreCase))
                return null;

            return PromptUnit(category, "Enter target unit for result");
        }

        /// <summary>
        /// Determines if category supports arithmetic operations
        /// 
        /// Parameters:
        /// - category: Measurement category
        /// 
        /// Returns:
        /// - true for Length, Weight, Volume (support all operations)
        /// - false for Temperature (only supports compare/convert)
        /// 
        /// Why Temperature is different:
        /// - Absolute temperature addition is not meaningful (40°C + 10°C ≠ 50°C)
        /// - Temperature differences can be subtracted (not supported yet, but could be)
        /// - This restriction enforces domain rules
        /// </summary>
        private static bool SupportsArithmetic(MeasurementCategory category)
        {
            return category != MeasurementCategory.Temperature;
        }

        /// <summary>
        /// Gets list of all unit names for specified category
        /// 
        /// Parameters:
        /// - category: Measurement category
        /// 
        /// Returns:
        /// - Readonly list of unit enum names (e.g., ["METER", "CENTIMETER", "KILOMETER"])
        /// 
        /// Implementation:
        /// - Uses reflection to get enum names
        /// - Returns empty array for unknown categories
        /// </summary>
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

        /// <summary>
        /// Prints all supported units for specified category
        /// 
        /// Used for:
        /// - Operation menu option "Show supported units for this category"
        /// - Displays units in comma-separated list format
        /// </summary>
        private static void PrintUnitsForCategory(MeasurementCategory category)
        {
            Console.WriteLine();
            Console.WriteLine(
                $"Supported {category} units: {string.Join(", ", GetUnits(category))}"
            );
        }

        /// <summary>
        /// Prints all available units across all measurement categories
        /// 
        /// Used for:
        /// - Main menu option "Show all supported units"
        /// - Shows complete reference of system capabilities
        /// </summary>
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

        /// <summary>
        /// Displays operation history persisted in repository
        /// 
        /// Used for:
        /// - Main menu option "Show persisted history"
        /// - Shows all past operations (successful and failed)
        /// 
        /// Display Format:
        /// - [DateTime] STATUS | Description | Error message (if any)
        /// - STATUS: "OK" for successful operations, "ERROR" for failed operations
        /// 
        /// Why Useful:
        /// - Audit trail for debugging
        /// - User can verify past results
        /// - Error tracking for support/troubleshooting
        /// </summary>
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
