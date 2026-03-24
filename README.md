# QuantityMeasurementApp

Small .NET sample: length, weight, and volume quantities with multi-unit arithmetic and conversions, now supporting extensible measurement categories via generics.

**Features**
- `Quantity<LengthUnit>` supports addition, subtraction, and division across different units (FEET, INCHES, YARDS, CENTIMETERS).
- `Quantity<WeightUnit>` supports addition, subtraction, and division across different units (KILOGRAMS, GRAMS, POUNDS).
- `Quantity<VolumeUnit>` supports addition, subtraction, and division across different units (LITRE, MILLILITRE, GALLON).
- Generic `Quantity<U>` class for any measurement category implementing `IMeasurable`.
- Comprehensive arithmetic operations: addition (UC6-UC7), subtraction (UC12), division (UC12).
- Centralized arithmetic logic (UC13) enforcing DRY principle with unified validation and conversion.
- Automatic unit conversion for arithmetic; result in first operand's unit (or explicitly specified target unit).
- Unit conversion API: static `Convert()` and instance `ConvertTo()` methods.
- Tolerance-based equality and normalized `GetHashCode()`.
- Type-safe cross-category prevention.
- Division returns dimensionless scalar ratios.

**Getting started**
- Build: `dotnet build QuantityMeasurementApp`
- Run demo: `dotnet run --project QuantityMeasurementApp`
- Run tests: `dotnet test QuantityMeasurementApp.Tests`

**Implemented (UC1) — Feet equality**
- Class: `QuantityMeasurementApp/Feet.cs`
- Behavior: Tolerance-based equality (0.0001) and normalized `GetHashCode()`.
- Tests: `QuantityMeasurementApp.Tests/FeetTests.cs` — verifies equality, hash consistency.

**Implemented (UC2) — Inches equality**
- Class: `QuantityMeasurementApp/Inches.cs`
- Behavior: Tolerance-based equality (0.0001) and normalized `GetHashCode()`.
- Tests: `QuantityMeasurementApp.Tests/InchesTests.cs` — verifies equality, hash consistency.

**Implemented (UC3) — Generic Length**
- Files: `QuantityMeasurementApp/Length.cs`, `QuantityMeasurementApp/LengthUnit.cs`
- Generic length class with unit conversion and tolerance-based equality.

**Implemented (UC4) — Extended units**
- Files: `QuantityMeasurementApp/Length.cs`, `QuantityMeasurementApp/LengthUnit.cs`
- Added YARDS and CENTIMETERS with correct conversion factors.

**Implemented (UC5) — Unit-to-unit conversion API**
- Files: `QuantityMeasurementApp/Length.cs`
- Static `Length.Convert(value, source, target)` and instance `ConvertTo(targetUnit)` methods.
- Tests: `QuantityMeasurementApp.Tests/LengthTests.cs` (`LengthConversionTests`) — conversion accuracy, round-trip, edge cases.

**Implemented (UC6) — Addition of quantities (consolidated)**
- Files: `QuantityMeasurementApp/QuantityLength.cs`, `QuantityMeasurementApp/Program.cs`
- Adds `Add()` method for cross-unit addition returning result in the caller's unit; supports same/cross-unit addition and basic validations.

**Implemented (UC7) — Addition with explicit target-unit specification**
- Files: `QuantityMeasurementApp/QuantityLength.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp/QuantityLengthAdditionTests.cs`
- Adds `Add(other, targetUnit)` overload allowing callers to specify the desired result unit (e.g., `a.Add(b, LengthUnit.CENTIMETERS)`).
- Tests: `QuantityMeasurementApp.Tests/QuantityLengthAdditionTests.cs` / `QuantityLengthExplicitTargetTests` — verifies explicit-target addition, commutativity, invalid-target handling, and scale/precision scenarios.

**Implemented (UC8) — Refactor QuantityLength for cleaner responsibilities**
- Files: `QuantityMeasurementApp/QuantityLength.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityLengthAdditionTests.cs`
- Simplifies `QuantityLength` by delegating all unit conversions to `LengthUnit` and consolidating equality, conversion, and addition logic.
- Updated tests (`QuantityLengthRefactoredTests`) ensure correct behavior after refactor: equality across units, `ConvertTo`, and `Add` with explicit target unit.

**Implemented (UC9) — Replicate Length pattern for Weight**
- Files: `QuantityMeasurementApp/QuantityWeight.cs`, `QuantityMeasurementApp/WeightUnit.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityWeightTests.cs`
- Implements weight quantities with multi-unit arithmetic and conversions, replicating the length pattern. Supports addition across different units (KILOGRAMS, GRAMS, POUNDS), automatic unit conversion for arithmetic, unit conversion API with static `Convert()` and instance `ConvertTo()` methods, tolerance-based equality, and addition with explicit target unit specification.

**Implemented (UC10) — Generic Quantity Class with Unit Interface for Multi-Category Support**
- Files: `QuantityMeasurementApp/Interfaces/IMeasurable.cs`, `QuantityMeasurementApp/Quantities/Quantity.cs`, `QuantityMeasurementApp/Units/LengthUnit.cs`, `QuantityMeasurementApp/Units/WeightUnit.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityLengthTests.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityWeightTests.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityConstructorTests.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityCrossCategoryTests.cs`
- Refactors the app to use a single generic `Quantity<U>` class where `U` implements `IMeasurable`, eliminating code duplication from UC9.
- Introduces `IMeasurable` interface for unit conversions, implemented via extension methods on enums.
- Updates `LengthUnit` and `WeightUnit` to use extension methods for interface implementation.
- Simplifies `Program.cs` with generic demonstration methods for equality, conversion, and addition.
- Maintains backward compatibility with updated tests ensuring type safety and cross-category prevention.

**Implemented (UC11) — Volume Measurements**
- Files: `QuantityMeasurementApp/Units/VolumeUnit.cs`, `QuantityMeasurementApp/Quantities/Quantity.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityVolumeTests.cs`
- Adds volume measurement support with LITRE, MILLILITRE, GALLON units.
- Updates generic `Quantity<U>` to handle volume conversions and operations.
- Adds volume demonstrations in `Program.cs`.
- Includes volume-specific tests for equality, conversion, and addition.

**Implemented (UC12) — Subtraction and Division Operations**
- Files: `QuantityMeasurementApp/Quantities/Quantity.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantitySubtractionTests.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityDivisionTests.cs`
- Adds comprehensive subtraction operations: `Subtract(Quantity<U> other)` and `Subtract(Quantity<U> other, U targetUnit)`.
- Adds division operation: `Divide(Quantity<U> other)` returning dimensionless scalar ratio.
- Supports both implicit (first operand's unit) and explicit target unit specification for subtraction.
- Maintains immutability: all operations return new objects; originals unchanged.
- Provides full validation: null checks, cross-category prevention, division-by-zero detection.
- Demonstrates non-commutative properties: A - B ≠ B - A, A ÷ B ≠ B ÷ A.
- Includes comprehensive unit tests (50+ test cases) covering same-unit, cross-unit, negative results, zero results, explicit target units, and edge cases.
- Demonstrates subtraction and division across all measurement categories (length, weight, volume).

**Implemented (UC13) — Centralized Arithmetic Logic to Enforce DRY in Quantity Operations**
- Files: `QuantityMeasurementApp/Quantities/Quantity.cs`
- Refactors arithmetic operations (addition, subtraction, division) to eliminate code duplication and enforce DRY principle.
- Introduces `ArithmeticOperation` enum for type-safe operation dispatch.
- Creates centralized `ValidateArithmeticOperands()` helper method for consistent validation across all operations.
- Creates centralized `PerformBaseArithmetic()` helper method for unified conversion and computation logic.
- Eliminates repetitive validation, conversion, and error handling code from individual arithmetic methods.
- Maintains identical public API and behavior from UC12; all existing tests pass without modification.
- Improves maintainability: validation and conversion changes affect all operations uniformly.
- Enables future extensibility: adding new operations (multiplication, modulo) requires minimal code changes.
- Demonstrates clean separation of concerns: public methods handle API consistency, private helpers handle implementation details.
- Validates consistent error handling: same exceptions and messages across all operations.
- Preserves immutability, rounding behavior, and mathematical properties from UC12.

**Implemented (UC14) — Temperature Measurement with Selective Arithmetic Support and IMeasurable Refactoring**
- Files: `QuantityMeasurementApp/Interfaces/IMeasurable.cs`, `QuantityMeasurementApp/Units/TemperatureUnit.cs`, `QuantityMeasurementApp/Quantities/Quantity.cs`, `QuantityMeasurementApp/Program.cs`, `QuantityMeasurementApp.Tests/QuantityTests/QuantityTemperatureTests.cs`
- Refactors `IMeasurable` interface to support optional arithmetic operations through default methods, enabling selective arithmetic constraints.
- Introduces `SupportsArithmetic` functional interface (delegate) for indicating operation capability.
- Adds `SupportsArithmeticOperations()` default method returning true for backward compatibility.
- Adds `ValidateOperationSupport()` default method for pre-arithmetic validation.
- Creates `TemperatureUnit` enum with CELSIUS, FAHRENHEIT, KELVIN units supporting non-linear conversions (Celsius as base unit).
- Implements temperature conversions using lambda expressions: °F = (°C × 9/5) + 32, K = °C + 273.15.
- Selectively disables arithmetic operations for temperature units by overriding `SupportsArithmeticOperations()` to return false.
- Throws `NotSupportedException` with descriptive messages for temperature arithmetic operations (addition, subtraction, division).
- Updates `Quantity<U>` class with pattern matching in `ConvertToBase()` and `ConvertFromBase()` methods for temperature support.
- Enhances `PerformBaseArithmetic()` with operation validation before arithmetic execution.
- Updates `GetUnitName()` method to support temperature units in string representations.
- Adds comprehensive temperature demonstrations in `Program.cs` showing equality (0°C = 32°F = 273.15K), conversions, and unsupported operations.
- Includes 17 comprehensive temperature tests covering equality across units, conversion accuracy, round-trip conversions, arithmetic rejection, cross-category prevention, and edge cases.
- Maintains full backward compatibility: all existing UC1-UC13 functionality preserved, 104 total tests pass.
- Demonstrates physical accuracy: temperature arithmetic disabled as it lacks meaningful physical interpretation in most contexts.
- Validates selective arithmetic support: length/weight/volume support arithmetic, temperature supports only equality and conversion.

**Implemented (UC15) — N-Tier Architecture Refactoring**
- Refactors project into clean 5-layer N-Tier architecture ensuring separation of concerns and maintainability.
- **Presentation Layer** (`QuantityMeasurementApp`): Handles user interaction via console menu; contains UI interface `IApplicationUI` and interactive `Menu` class; decoupled from business logic.
- **Controller Layer** (`QuantityMeasurementController`): Thin orchestration layer; routes user requests to service layer; maintains request/response flow without business logic.
- **Business Layer** (`QuantityMeasurementBusiness`): Core domain logic; includes `Quantity<U>` generic class, measurement units (Length, Weight, Volume, Temperature), unit conversion logic, arithmetic operations, and business exceptions.
  - Units are defined as enum-only files in `Units/` folder (e.g., `LengthUnit.cs` contains only enum definition).
  - Unit conversion implementations moved to separate `UnitExtensions/` folder with dedicated extension classes for each unit type.
  - Service layer (`QuantityMeasurementServiceImpl`) implements `IQuantityMeasurementService` interface, providing measurement operations and coordinating with repository.
- **Model/DTO Layer** (`QuantityMeasurementModel`): Data transfer objects only; contains `QuantityDTO` (user input/output), `QuantityModel` (internal domain model), and simple data holders; no persistence knowledge.
- **Repository Layer** (`QuantityMeasurementRepo`): Data access abstraction; singleton `QuantityMeasurementCacheRepository` manages persistence; owns `QuantityMeasurementEntity` (moved from model layer) as persistence audit record in `QuantityMeasurementRepo/Models/` namespace.
- **Cross-layer Benefits:**
  - Enums belong to business layer as they represent domain concepts, not transport objects.
  - Persistence entities (`QuantityMeasurementEntity`) live in repository layer, not model layer, keeping model focused on DTOs.
  - Extension methods grouped by responsibility in `UnitExtensions/` improves discoverability and maintainability.
  - Interfaces (`IApplicationUI`, `IQuantityMeasurementService`, `IQuantityMeasurementRepository`) enforce abstraction boundaries.
- **Code Quality Improvements:**
  - Removed unused interface `IMeasurable` that was declared but never directly implemented (units use extension methods instead).
  - Removed unused helper method parameters that served no purpose.
  - Eliminated unused nested DTO interface and stale documentation.
  - Removed emoji symbols (✓, ✗) from console output, replacing with plain text prefixes (Success, Error) for better compatibility.
  - Dead code cleanup: no ceremonial or temporary code remains.
- **Structure Overview:**
  ```
  Presentation → Controller → Business → Model/DTO → Repository
  (Menu)      (Orchestrator) (Domain  (Contracts)  (Persistence)
                            Logic)
  ```
- **Validation:** All 104 tests pass; full backward compatibility maintained; architecture enforces layering discipline while enabling future feature additions without cross-layer coupling.
- Architecture follows industry best practices for scalable, maintainable .NET applications with clear separation of concerns, consistent naming conventions, and logical folder organization.
