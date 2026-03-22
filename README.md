# QuantityMeasurementApp

## 📌 Project Overview

QuantityMeasurementApp is a domain-driven C# application developed incrementally using a hybrid approach of:

- Design-Develop-Test (DDT)
- Test-Driven Development (TDD)

Each use case expands functionality in a controlled and maintainable way.

---


## 🚀 Use Case 2 (UC2) – Feet and Inches Measurement Equality

### Description

UC2 extends UC1 by introducing equality comparison for Inches measurement alongside Feet measurement.

⚠ Important:
- Feet and Inches are treated as separate entities.
- No cross-unit comparison (e.g., 1 ft ≠ 12 inches in this use case).
- Only same-unit equality comparison is supported.

---

## ✅ Features Implemented in UC2

- Immutable `Feet` value object
- Immutable `Inches` value object
- Proper override of `Equals()` and `GetHashCode()`
- Implementation of `IEquatable<T>` for both classes
- Equality methods in `QuantityMeasurementService`
- MSTest unit test coverage for both Feet and Inches
- Clean architecture separation (src and tests layers)

---

## 🚀 Use Case 3 (UC3) – QuantityLength with Unit Conversion (DRY)

### Description

UC3 refactors the duplicated Feet and Inches logic into a single QuantityLength class that uses a LengthUnit enum.
This solves the DRY problem where Feet and Inches had nearly identical constructors and equality logic.

### Problem Solved

- Eliminates code duplication between Feet and Inches classes.
- Enables cross-unit comparison (e.g., 1 ft == 12 inches) using a shared conversion path.
- Makes it easier to add new length units without creating new classes.

---

## ✅ Features Implemented in UC3

- LengthUnit enum with conversion factors
- QuantityLength value object with unit-aware equality
- Cross-unit comparison via base-unit conversion (feet)
- Updated QuantityMeasurementService to support QuantityLength
- New unit tests for cross-unit and same-unit equality

---

## 🧠 Concepts Demonstrated in UC3

- DRY Principle (single class for multiple units)
- Abstraction of conversion logic
- Polymorphism via unit enum
- Value-based equality with cross-unit support
- Scalability for adding new units

---

## 🚀 Use Case 4 (UC4) – Extended Unit Support (Yards & Centimeters)

### Description

UC4 extends UC3 by introducing Yards and Centimeters as additional length units to the QuantityLength class.
This demonstrates how the generic QuantityLength design scales effortlessly to accommodate new units without code duplication.

### Problem Solved

- Validates the DRY principle design from UC3 by adding new units with minimal code changes.
- Proves that adding units requires only enum modification, not new classes.
- Enables complex multi-unit conversions (yards ↔ feet ↔ inches ↔ centimeters).

---

## ✅ Features Implemented in UC4

- LengthUnit enum extended with Yards and Centimeters constants
- Conversion factors: 1 Yard = 3 Feet, 1 cm = 1/30.48 Feet
- QuantityLength unchanged – no modifications needed for new units
- 32 new test cases covering yard, centimeter, and multi-unit comparisons
- All UC1, UC2, UC3 functionality remains backward compatible

---

## 🧠 Concepts Demonstrated in UC4

- **Scalability of Generic Design**: Adding units requires only enum modification
- **Conversion Factor Management**: Centralized in enum for consistency
- **Unit Relationships**: Understanding conversion to common base unit (feet)
- **Enum Extensibility**: Type-safe approach to managing unit variations
- **Mathematical Accuracy**: Precise conversion factors relative to base unit
- **DRY Validation**: Proves the QuantityLength design eliminates duplication
- **Backward Compatibility**: New units don't affect existing functionality

---

## 🚀 Use Case 5 (UC5) – Unit-to-Unit Conversion (Same Measurement Type)

### Description

UC5 extends UC4 by adding explicit unit conversion APIs for length values.
Instead of only checking equality, the app now converts values between supported units (Feet, Inches, Yards, Centimeters) through a shared base-unit normalization path.

### Problem Solved

- Provides direct conversion for same measurement type (length) using one consistent formula.
- Keeps conversion logic centralized and reusable.
- Preserves immutability by returning converted values/new instances without mutating existing objects.

---

## ✅ Features Implemented in UC5

- Non-static conversion API in `QuantityLength`: `ConvertTo(LengthUnit targetUnit)`
- Non-static conversion API in `QuantityMeasurementService`: `Convert(double value, LengthUnit sourceUnit, LengthUnit targetUnit)`
- Conversion formula: `result = value × (sourceFactor / targetFactor)`
- Input validation for finite values and supported enum units
- Separate focused conversion tests in `UnitConversionTests.cs`

---

## 🧠 Concepts Demonstrated in UC5

- Base-unit normalization for cross-unit conversion
- Enum-driven conversion factor management
- Value object immutability and conversion without side effects
- Precision handling with epsilon-based assertions in tests
- Clear API design for conversion and equality responsibilities

---

## 🚀 Use Case 6 (UC6) – Addition of Two Length Units (Same Category)

### Description

UC6 extends UC5 by adding arithmetic support for length values.
Two measurements can be added even when units differ, and by default the result is returned in the unit of the first operand.

### Problem Solved

- Supports same-unit and cross-unit addition through the existing conversion pipeline.
- Preserves immutability by returning new `QuantityLength` objects.
- Enables clear API usage for domain arithmetic on value objects.

---

## ✅ Features Implemented in UC6

- `QuantityLength.Add(QuantityLength other)`
- `QuantityLength.Add(QuantityLength other, LengthUnit targetUnit)`
- `QuantityLength.Add(double value, LengthUnit unit)`
- `QuantityLength.Add(double value, LengthUnit unit, LengthUnit targetUnit)`
- Service overloads for object and raw-value addition in `QuantityMeasurementService`
- Validation for null operands and invalid/unsupported units
- Dedicated UC6 test suite in `UnitAdditionTests.cs`

---

## 🧠 Concepts Demonstrated in UC6

- Same-unit and cross-unit addition
- Commutativity checks in a shared target unit
- Identity behavior with zero
- Floating-point precision handling with epsilon tolerance
- Input validation and defensive error handling

---

## 🚀 Use Case 7 (UC7) – Addition with Target Unit Specification

### Description

UC7 extends UC6 by allowing the caller to explicitly provide a target unit for the addition result.
The operation still adds values through a common base-unit path, but the final output unit is always the requested target unit.

### Problem Solved

- Decouples result representation from operand units.
- Supports flexible output in FEET, INCHES, YARDS, or CENTIMETERS for the same arithmetic operation.
- Preserves immutability by returning a new `QuantityLength` instance.

---

## ✅ Features Implemented in UC7

- `QuantityLength.Add(QuantityLength other, LengthUnit targetUnit)`
- `QuantityLength.Add(double value, LengthUnit unit, LengthUnit targetUnit)`
- Service overloads for explicit target unit in `QuantityMeasurementService`
- Validation for invalid/unsupported target unit values
- UC7-focused tests added to `UnitAdditionTests.cs`

---

## 🧠 Concepts Demonstrated in UC7

- Explicit parameterized result unit
- Commutativity with a fixed target unit
- Mathematical equivalence across different target representations
- Edge-case support (zero, negative, cross-scale values)

---

## 🚀 Use Case 8 (UC8) – Standalone Unit with Conversion Responsibility

### Description

UC8 refactors unit conversion ownership by keeping `LengthUnit` as a standalone top-level enum and assigning conversion responsibility directly to it.
`QuantityLength` delegates conversion behavior to the unit, improving cohesion and supporting scalable patterns for future measurement categories.

### Problem Solved

- Centralizes conversion logic in the unit abstraction.
- Reduces coupling in `QuantityLength` by removing dedicated converter-object dependency.
- Preserves full backward compatibility for UC1–UC7 public APIs.

---

## ✅ Features Implemented in UC8

- `LengthUnit` conversion responsibilities:
   - `GetConversionFactor()`
   - `ConvertToBaseUnit(double value)`
   - `ConvertFromBaseUnit(double baseValue)`
- `QuantityLength` now delegates conversions to `LengthUnit` conversion methods
- Existing equality, conversion, and addition APIs remain unchanged
- Dedicated UC8 test suite added in `LengthUnitTests.cs`

---

## 🧠 Concepts Demonstrated in UC8

- Single Responsibility Principle for unit conversion logic
- Delegation of conversion behavior from quantity object to unit abstraction
- Improved architectural scalability for additional categories (weight/volume/etc.)
- Backward-compatible refactoring with no client API breakage

---

## 🧠 Concepts Demonstrated

- Equality Contract:
  - Reflexive
  - Symmetric
  - Transitive
  - Consistent
  - Null Handling
- Type Safety
- Value-Based Equality
- Floating-point comparison using `double.Compare()`
- Encapsulation and Immutability
- SOLID Principles
- Clean project layering
- Professional Git branching workflow

---


## 🚀 Use Case 9 (UC9) – Weight Unit Support

### Description

UC9 extends the application to a second measurement category: **weight**.
This proves the conversion/addition/equality flow can work beyond length.

### ✅ Features Implemented in UC9

- `WeightUnit` with conversion factors (base unit: kilogram)
- Weight conversion support via service APIs
- Weight addition support (same-unit and cross-unit)
- Weight-focused tests:
   - `WeightUnitTests.cs`
   - `WeightUnitConversionTests.cs`
   - `WeightUnitAdditionTests.cs`
   - `QuantityWeightTests.cs`

---

## 🚀 Use Case 10 (UC10) – Generic Quantity with Common Unit Contract

### Description

UC10 removes quantity-class duplication by introducing a single generic model:

- `Quantity<U>` for value + unit operations
- `IMeasurable` as common unit behavior contract

In C#, enums cannot implement interfaces directly, so both `LengthUnit` and `WeightUnit`
are adapted to `IMeasurable` using extension-based wrappers (`AsMeasurable()`).

### ✅ Features Implemented in UC10

- Generic immutable `Quantity<U>` replaces category-specific quantity models
- Common conversion contract through `IMeasurable`
- Unified generic service operations:
   - `AreEqual<U>(...)`
   - `Convert<U>(...)`
   - `Add<U>(...)`
- Runtime category-safety check in equality (length vs weight returns `false`)
- Backward compatibility retained for legacy `Feet` / `Inches` UCs

### 📌 UC10 Behavior Notes

- Input validation rejects non-finite values and invalid enum values
- Equality compares normalized base-unit values
- `ConvertTo` and `Add` return values rounded to **2 decimal places**
- Existing unit tests continue to pass with the generic architecture

---

## 🚀 Use Case 11 (UC11) – Volume Measurement Equality, Conversion, and Addition

### Description

UC11 introduces a third measurement category: **volume**.
It validates that the generic architecture from UC10 scales without changing core generic logic.

### ✅ Features Implemented in UC11

- New `VolumeUnit` enum with base unit **Litre**
- Supported units:
   - `Litre` → `1.0`
   - `Millilitre` → `0.001`
   - `Gallon` → `3.78541`
- Volume conversion support via existing generic APIs
- Volume addition support with:
   - implicit target unit (first operand unit)
   - explicit target unit
- Cross-category isolation retained (`Volume` vs `Length`/`Weight` returns incompatible equality)

### 📌 UC11 Behavior Notes

- Equality is based on normalized base-unit values
- `ConvertTo` and `Add` keep the shared UC10 rounding behavior (**2 decimal places**)
- Existing UC1–UC10 functionality remains unaffected

### 🧪 UC11 Example Operations

- Equality:
   - `Quantity(1.0, Litre)` == `Quantity(1000.0, Millilitre)` → `true`
   - `Quantity(1.0, Gallon)` == `Quantity(3.78541, Litre)` → `true`

- Conversion:
   - `Quantity(1.0, Litre).ConvertTo(Millilitre)` → `Quantity(1000.0, Millilitre)`
   - `Quantity(2.0, Gallon).ConvertTo(Litre)` → `Quantity(7.57, Litre)`
   - `Quantity(500.0, Millilitre).ConvertTo(Gallon)` → `Quantity(0.13, Gallon)`

- Addition:
   - `Quantity(1.0, Litre).Add(Quantity(1000.0, Millilitre))` → `Quantity(2.0, Litre)`
   - `Quantity(1.0, Litre).Add(Quantity(1000.0, Millilitre), Millilitre)` → `Quantity(2000.0, Millilitre)`
   - `Quantity(500.0, Millilitre).Add(Quantity(1.0, Litre), Gallon)` → `Quantity(0.4, Gallon)`

---

   ## 🚀 Use Case 12 (UC12) – Subtraction and Division Operations

   ### Description

   UC12 extends the generic arithmetic model by adding:

   - **Subtraction** between same-category quantities (`Quantity<U>`) with:
      - implicit target unit (first operand unit)
      - explicit target unit overload
   - **Division** between same-category quantities returning a **dimensionless scalar** (`double`)

   Both operations reuse the UC10–UC11 conversion pipeline (convert to base unit first), preserve immutability,
   and keep category safety through generic typing.

   ### ✅ Features Implemented in UC12

   - `Quantity<U>.Subtract(Quantity<U> other)`
   - `Quantity<U>.Subtract(Quantity<U> other, U targetUnit)`
   - `Quantity<U>.Divide(Quantity<U> other)`
   - Service facade overloads for subtraction and division in `QuantityMeasurementService`
   - Program demonstrations for subtraction and division across:
      - Length
      - Weight
      - Volume
   - New UC12 unit test coverage for:
      - same-unit and cross-unit subtraction/division
      - explicit target unit subtraction
      - negative and zero subtraction results
      - non-commutativity checks
      - division-by-zero handling
      - null argument handling

   ### 📌 UC12 Behavior Notes

   - Subtraction result is rounded to **2 decimal places** and returned as a new `Quantity<U>`.
   - Division returns raw ratio as `double` (no unit).
   - Division by zero throws `ArithmeticException`.
   - Cross-category arithmetic is prevented by compile-time generic constraints and runtime method signatures.

   ### 🧪 UC12 Example Operations

   - Subtraction (implicit target):
      - `Quantity(10.0, Feet).Subtract(Quantity(6.0, Inches))` → `Quantity(9.5, Feet)`
      - `Quantity(10.0, Kilogram).Subtract(Quantity(5000.0, Gram))` → `Quantity(5.0, Kilogram)`
   - Subtraction (explicit target):
      - `Quantity(10.0, Feet).Subtract(Quantity(6.0, Inches), Inches)` → `Quantity(114.0, Inches)`
      - `Quantity(5.0, Litre).Subtract(Quantity(2.0, Litre), Millilitre)` → `Quantity(3000.0, Millilitre)`
   - Division:
      - `Quantity(24.0, Inches).Divide(Quantity(2.0, Feet))` → `1.0`
      - `Quantity(2000.0, Gram).Divide(Quantity(1.0, Kilogram))` → `2.0`
      - `Quantity(5.0, Litre).Divide(Quantity(10.0, Litre))` → `0.5`

   ---

## 🚀 Use Case 13 (UC13) – Centralized Arithmetic Logic (DRY Refactor)

### Description

UC13 refactors internal arithmetic implementation in `Quantity<U>` to remove duplication across:

- `Add(...)`
- `Subtract(...)`
- `Divide(...)`

The public API and behavior remain unchanged from UC12. The refactor introduces a centralized arithmetic flow
for validation, base-unit conversion, operation dispatch, and result projection.

### ✅ Features Implemented in UC13

- Private `ArithmeticOperation` enum for operation dispatch (`Add`, `Subtract`, `Divide`)
- Centralized validation helper:
   - null operand validation
   - category/type compatibility guard
   - finite numeric validation
   - target unit validation (for add/subtract)
- Centralized base arithmetic helper to compute add/subtract/divide in base-unit space
- Shared conversion helper for add/subtract result projection to target unit
- Dedicated divide-by-zero guard reused by division flow

### 📌 UC13 Behavior Notes

- Public method signatures are unchanged.
- `Add` and `Subtract` still return rounded `Quantity<U>` values (2 decimals).
- `Divide` still returns a raw dimensionless `double`.
- Existing UC12 behavior is preserved; implementation is now DRY and easier to extend.

### 🧪 UC13 Validation Coverage

- Existing UC12 tests continue to pass without API-level changes.
- Additional UC13 regression tests in `QuantityUc13RefactorTests.cs` verify:
   - unchanged arithmetic results
   - consistent null-operand handling across operations
   - division-by-zero behavior remains fail-fast

---

## 🚀 Use Case 14 (UC14) – Temperature Measurement with Selective Arithmetic Support

### Description

UC14 adds temperature support (`Celsius`, `Fahrenheit`, `Kelvin`) and refactors `IMeasurable` so arithmetic support is optional.
Unlike length, weight, and volume, absolute temperatures do not support arithmetic operations in this model.

### ✅ Features Implemented in UC14

- New `TemperatureUnit` enum and measurable adapter with non-linear conversion formulas.
- `IMeasurable` now includes optional operation-support defaults:
   - `SupportsArithmetic()`
   - `ValidateOperationSupport(string operation)`
- `Quantity<U>` arithmetic flow validates operation support before add/subtract/divide execution.
- Temperature conversions and equality work through base-unit normalization.
- Temperature arithmetic (`Add`, `Subtract`, `Divide`) throws `NotSupportedException` with clear messages.

### 📌 UC14 Behavior Notes

- Existing UC1–UC13 behavior remains unchanged for length, weight, and volume.
- Temperature supports equality and conversion only.
- Cross-category safety remains intact through generic typing and runtime unit-category checks.

### 🧪 UC14 Validation Coverage

- `TemperatureUnitTests.cs` validates equality, conversion formulas, absolute-zero, and edge points.
- `TemperatureUnsupportedOperationsTests.cs` validates unsupported arithmetic and capability flags.

---

## 🛠 Tech Stack

- .NET 8
- C#
- MSTest
- Git

---

## 📂 Project Structure

```text
QuantityMeasurementApp.sln

src/
└── QuantityMeasurementApp
    ├── Program.cs
    ├── Models/
    │   ├── Feet.cs
    │   ├── IMeasurable.cs
    │   ├── Inches.cs
    │   ├── LengthUnit.cs
    │   ├── Quantity.cs
   │   ├── TemperatureUnit.cs
    │   ├── VolumeUnit.cs
    │   └── WeightUnit.cs
    └── Services/
        └── QuantityMeasurementService.cs

tests/
└── QuantityMeasurementApp.Tests
    ├── FeetTests.cs
    ├── IMeasurableTests.cs
    ├── InchesTests.cs
    ├── LengthUnitTests.cs
    ├── QuantityTests.cs
   ├── QuantityUc13RefactorTests.cs
   ├── TemperatureUnitTests.cs
   ├── TemperatureUnsupportedOperationsTests.cs
    ├── QuantityVolumeTests.cs
    ├── QuantityWeightTests.cs
    ├── UnitConversionTests.cs
    ├── UnitAdditionTests.cs
   ├── UnitSubtractionTests.cs
   ├── UnitDivisionTests.cs
   ├── VolumeUnitTests.cs
   ├── VolumeUnitConversionTests.cs
   ├── VolumeUnitAdditionTests.cs
   ├── VolumeUnitSubtractionTests.cs
   ├── VolumeUnitDivisionTests.cs
   ├── WeightUnitTests.cs
   ├── WeightUnitConversionTests.cs
   ├── WeightUnitAdditionTests.cs
   ├── WeightUnitSubtractionTests.cs
   └── WeightUnitDivisionTests.cs
```

## ▶ How to Run the Application

1. Clone the repository:

   ```bash
   git clone <repository-url>
   ```

2. Navigate to the project folder:

   ```bash
   cd QuantityMeasurementApp
   ```

3. Build the project:

   ```bash
   dotnet build
   ```

4. Run the application:

   ```bash
   cd src/QuantityMeasurementApp
   dotnet run
   ```

### Expected Output (Sample from `dotnet run`)

```text
=== Length Operations ===
Input: Quantity(1, Feet) and Quantity(12, Inches) -> Output: True
Input: Quantity(1, Feet).ConvertTo(Inches) -> Output: Quantity(12, Inches)
Input: Quantity(1, Feet).Add(Quantity(12, Inches), Feet) -> Output: Quantity(2, Feet)
Input: Quantity(10, Feet).Subtract(Quantity(6, Inches)) -> Output: Quantity(9.5, Feet)
Input: Quantity(10, Feet).Subtract(Quantity(6, Inches), Inches) -> Output: Quantity(114, Inches)
Input: Quantity(24, Inches).Divide(Quantity(2, Feet)) -> Output: 1

=== Weight Operations ===
Input: Quantity(1, Kilogram) and Quantity(1000, Gram) -> Output: True
Input: Quantity(1, Kilogram).ConvertTo(Gram) -> Output: Quantity(1000, Gram)
Input: Quantity(1, Kilogram).Add(Quantity(1000, Gram), Kilogram) -> Output: Quantity(2, Kilogram)
Input: Quantity(10, Kilogram).Subtract(Quantity(5000, Gram)) -> Output: Quantity(5, Kilogram)
Input: Quantity(10, Kilogram).Subtract(Quantity(5000, Gram), Gram) -> Output: Quantity(5000, Gram)
Input: Quantity(10, Kilogram).Divide(Quantity(5, Kilogram)) -> Output: 2

=== Volume Operations ===
Input: Quantity(1, Litre) and Quantity(1000, Millilitre) -> Output: True
Input: Quantity(1, Gallon).ConvertTo(Litre) -> Output: Quantity(3.79, Litre)
Input: Quantity(1, Litre).Add(Quantity(1, Gallon), Millilitre) -> Output: Quantity(4785.41, Millilitre)
Input: Quantity(5, Litre).Subtract(Quantity(500, Millilitre)) -> Output: Quantity(4.5, Litre)
Input: Quantity(5, Litre).Subtract(Quantity(2, Litre), Millilitre) -> Output: Quantity(3000, Millilitre)
Input: Quantity(1000, Millilitre).Divide(Quantity(1, Litre)) -> Output: 1

=== Temperature Operations ===
Input: Quantity(0, Celsius) and Quantity(32, Fahrenheit) -> Output: True
Input: Quantity(273.15, Kelvin) and Quantity(0, Celsius) -> Output: True
Input: Quantity(100, Celsius).ConvertTo(Fahrenheit) -> Output: Quantity(212, Fahrenheit)
Input: Quantity(273.15, Kelvin).ConvertTo(Celsius) -> Output: Quantity(0, Celsius)
Input: Quantity(100, Celsius).Add(Quantity(50, Celsius)) -> Error: Temperature does not support Add operation for absolute values.
Input: Quantity(100, Celsius).Divide(Quantity(50, Celsius)) -> Error: Temperature does not support Divide operation for absolute values.
```

---

## 🧪 How to Run Unit Tests

From solution root directory:

```bash
dotnet test
```

All test cases must pass before merging into develop or main branch.

---