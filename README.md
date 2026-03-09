# QuantityMeasurementApp

## UC11: Volume Measurement Equality, Conversion, and Addition (Litre, Millilitre, Gallon)

UC11 extends the application with a third independent measurement category: **Volume**.
The implementation follows the UC10 generic pattern and supports:

- Equality across volume units
- Unit conversion between volume units
- Addition with implicit or explicit target unit

Supported units:

- `LITRE` (base unit): `1.0`
- `MILLILITRE`: `0.001` litre
- `GALLON`: `3.78541` litre

### Preconditions

- Generic `Quantity<U>` implementation is available and stable.
- Existing `LengthUnit` and `WeightUnit` behaviors remain unchanged.
- Volume is treated as a separate non-interoperable category.

### UC11 Implementation Summary

- Added `VolumeUnit` enum and `VolumeUnitExtensions` with:
  - `GetConversionFactor()`
  - `ConvertToBaseUnit(double value)`
  - `ConvertFromBaseUnit(double baseValue)`
  - `GetUnitName()`
- Enabled `Quantity<VolumeUnit>` conversion dispatch in the generic `Quantity<U>` conversion helper.
- Added UC11 examples in `Program.cs` (equality, conversion, addition).
- Added UC11 tests to `UnitTest1.cs`.

### Main Flow Coverage

#### 1) Equality Comparison

All compared values are normalized to litres, then checked using epsilon tolerance.

Examples:

- `Quantity<VolumeUnit>(1.0, LITRE)` equals `Quantity<VolumeUnit>(1000.0, MILLILITRE)`
- `Quantity<VolumeUnit>(1.0, GALLON)` equals `Quantity<VolumeUnit>(3.78541, LITRE)`

#### 2) Unit Conversion

`ConvertTo(targetUnit)` converts through base unit (`LITRE`) and returns a new immutable quantity.

Examples:

- `1.0 LITRE -> 1000.0 MILLILITRE`
- `1.0 GALLON -> 3.78541 LITRE`
- `1000.0 MILLILITRE -> ~0.264172 GALLON`

#### 3) Addition

Both operands are converted to base unit, summed, then returned in:

- first operand’s unit (implicit target), or
- caller-specified unit (explicit target)

Examples:

- `1.0 LITRE + 1000.0 MILLILITRE = 2.0 LITRE`
- `1.0 LITRE + 1.0 GALLON = 4785.41 MILLILITRE` (explicit target)

#### 4) Cross-Category Type Safety

- `Quantity<VolumeUnit>` is not equal to `QuantityLength` or `QuantityWeight`.
- Generic typing prevents accidental arithmetic between different measurement categories.

### Postconditions

- Volume equality, conversion, and addition are available with immutable behavior.
- UC1–UC10 features continue to work unchanged.
- Architecture remains scalable: adding a new category follows the same pattern.

### UC11 Usage Examples

```csharp
var volume1 = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITRE);
var volume2 = new Quantity<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

var areEqual = volume1.Equals(volume2); // true
var inGallons = volume1.ConvertTo(VolumeUnit.GALLON); // ~0.264172
var sum = Quantity<VolumeUnit>.Add(volume1, new Quantity<VolumeUnit>(1.0, VolumeUnit.GALLON), VolumeUnit.LITRE); // ~4.78541
```

### UC11 Test Case Results

UC11 test categories include:

- `VolumeUnit` conversion factor and base conversion validation
- Equality tests (same-unit and cross-unit)
- Conversion tests (all key pairs + round-trip)
- Addition tests (same-unit, cross-unit, explicit target)
- Cross-category compatibility checks (Volume vs Length/Weight)

Latest run result:

- **Total tests:** 74
- **Passed:** 74
- **Failed:** 0

## UC10: Generic Quantity<U> Class with IMeasurable Interface

UC10 refactors the application to use a **generic, category-agnostic `Quantity<U>` class**, eliminating code duplication and establishing a foundation for unlimited measurement categories. This use case demonstrates advanced C# generics, reflection-based polymorphism, and interface design for scalability.

### Architecture Evolution

**UC9 Problem:**

- `QuantityLength` and `QuantityWeight` are nearly identical (300+ lines of duplicated code)
- Adding a third category (e.g., `Volume`, `Temperature`) requires copying the entire Quantity class
- Violates DRY (Don't Repeat Yourself) and SRP (Single Responsibility Principle)
- High risk of inconsistency when updating comparison/arithmetic logic

**UC10 Solution:**

- Single generic `Quantity<U>` class where `U` is constrained to enum types (`where U : struct, Enum`)
- Reflection-based dispatch to unit-specific extension methods
- Linear code growth instead of exponential growth with new categories
- Type-safe operations via generic constraints

### Core Components

#### 1. Generic Quantity Class

```csharp
public class Quantity<U> where U : struct, Enum
{
    public Quantity(double value, U unit) { ... }
    public Quantity<U> ConvertTo(U targetUnit) { ... }
    public Quantity<U> Add(Quantity<U> other) { ... }
    public static Quantity<U> Add(Quantity<U> first, Quantity<U> second, U targetUnit) { ... }
    public static double Convert(double value, U source, U target) { ... }
}
```

#### 2. Unit Enums with Extension Methods

Each unit enum (e.g., `LengthUnit`, `WeightUnit`) has a corresponding extensions class:

```csharp
public static class LengthUnitExtensions
{
    public static double GetConversionFactor(this LengthUnit unit) { ... }
    public static double ConvertToBaseUnit(this LengthUnit unit, double value) { ... }
    public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue) { ... }
    public static string GetUnitName(this LengthUnit unit) { ... }
}
```

#### 3. Reflection-Based Dispatch

The `Quantity<U>` class uses reflection to dynamically invoke extension methods based on the unit type:

```csharp
private static double ConvertToBase<T>(double value, T unit) where T : struct, Enum
{
    var method = typeof(T).Name switch
    {
        "LengthUnit" => typeof(LengthUnitExtensions).GetMethod("ConvertToBaseUnit"),
        "WeightUnit" => typeof(WeightUnitExtensions).GetMethod("ConvertToBaseUnit"),
        _ => throw new NotSupportedException(...)
    };
    return (double)method.Invoke(null, new object[] { unit, value })!;
}
```

#### 4. Backward Compatibility Wrappers

For seamless migration, `QuantityLength` and `QuantityWeight` are now thin wrapper classes:

```csharp
public class QuantityLength : Quantity<LengthUnit>
{
    public QuantityLength(double value, LengthUnit unit) : base(value, unit) { }
}

public class QuantityWeight : Quantity<WeightUnit>
{
    public QuantityWeight(double value, WeightUnit unit) : base(value, unit) { }
}
```

### Benefits of UC10 Design

| Aspect                  | UC9                        | UC10                           |
| ----------------------- | -------------------------- | ------------------------------ |
| **Code per category**   | ~150 lines                 | ~0 lines (generic)             |
| **Adding new category** | Copy entire Quantity class | Add 1 enum + 1 extension class |
| **Logic consistency**   | Risk of divergence         | Single source                  |
| **Type safety**         | Category-specific          | Generic with constraints       |
| **Scalability**         | O(n) growth                | O(1) growth                    |

### Usage Examples

**Creating Quantities:**

```csharp
// Both direct generic syntax and backward-compatible syntax work
var length1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
var length2 = new QuantityLength(5.0, LengthUnit.FEET);  // Backward compatible

var weight1 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
var weight2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);  // Backward compatible
```

**Equality Testing:**

```csharp
var length1 = new Quantity<LengthUnit>(5.0, LengthUnit.FEET);
var length2 = new Quantity<LengthUnit>(60.0, LengthUnit.INCH);
Console.WriteLine(length1.Equals(length2));  // true

var weight1 = new Quantity<WeightUnit>(2.0, WeightUnit.KILOGRAM);
var weight2 = new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM);
Console.WriteLine(weight1.Equals(weight2));  // true
```

**Conversion:**

```csharp
var quantity = new Quantity<LengthUnit>(36.0, LengthUnit.INCH);
var converted = quantity.ConvertTo(LengthUnit.YARD);
Console.WriteLine(converted);  // Quantity(1, YARD)
```

**Addition:**

```csharp
var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCH);
var sum = Quantity<LengthUnit>.Add(q1, q2, LengthUnit.FEET);
Console.WriteLine(sum);  // Quantity(2, FEET)
```

### Adding a New Measurement Category

To add support for a new category (e.g., `Volume`), only two files are required:

1. **VolumeUnit.cs** – Define the enum and extension methods
2. **No changes to Quantity.cs** – It already supports any enum type

Example for `VolumeUnit`:

```csharp
public enum VolumeUnit { LITER, MILLILITER, GALLON }

public static class VolumeUnitExtensions
{
    public static double GetConversionFactor(this VolumeUnit unit) { ... }
    public static double ConvertToBaseUnit(this VolumeUnit unit, double value) { ... }
    public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue) { ... }
    public static string GetUnitName(this VolumeUnit unit) { ... }
}
```

Then use it immediately:

```csharp
var volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.LITER);
var converted = volume.ConvertTo(VolumeUnit.MILLILITER);  // Works automatically!
```

### Test Coverage

UC10 validates:

- Generic `Quantity<U>` works correctly for multiple unit types
- Backward-compatible wrapper classes function identically
- Reflection dispatch correctly invokes extension methods
- Cross-category type safety (prevents Quantity<LengthUnit> from equaling Quantity<WeightUnit>)
- All UC1–UC9 tests pass unchanged (56 passing tests)

### Key UC10 Concepts

- **Generics** – Single implementation for unlimited categories
- **Reflection** – Dynamic method dispatch based on type information
- **Type Constraints** – Enforce enum-only type parameters
- **Extension Methods** – Add behavior to enums without subclassing
- **Backward Compatibility** – Wrapper classes bridge old and new designs
- **Scalability** – O(1) growth vs. O(n) in UC9

---

## UC9: Weight Measurement Equality, Conversion, and Addition

UC9 extends the application to support weight measurements alongside length measurements. This use case demonstrates that the generic patterns from UC1–UC8 scale seamlessly to multiple measurement categories.

Weight supports three units:

- `KILOGRAM` (kg) – base unit
- `GRAM` (g) – 1 kg = 1000 g
- `POUND` (lb) – 1 lb ≈ 0.453592 kg

## Design Consistency

- `WeightUnit` enum parallels `LengthUnit` with standalone conversion responsibility.
- `QuantityWeight` class (now derived from `Quantity<WeightUnit>` via UC10) parallels `QuantityLength` for weight measurements.
- Weight and length are separate, type-safe categories that cannot be directly compared.
- All UC1–UC8 length functionality remains unchanged.

## Supported Measurement Categories

### Length (UC1–UC8)

- Units: `FEET`, `INCH`, `YARD`, `CENTIMETER`
- Base unit: `FEET`

### Weight (UC9)

- Units: `KILOGRAM`, `GRAM`, `POUND`
- Base unit: `KILOGRAM`

## Weight API

- `QuantityWeight(double value, WeightUnit unit)`
- `static double Convert(double value, WeightUnit source, WeightUnit target)`
- `QuantityWeight ConvertTo(WeightUnit targetUnit)`
- `QuantityWeight Add(QuantityWeight other)`
- `static QuantityWeight Add(QuantityWeight first, QuantityWeight second)`
- `static QuantityWeight Add(QuantityWeight first, QuantityWeight second, WeightUnit targetUnit)`

## Validation Rules

- Values must be finite (`NaN`/`Infinity` rejected).
- Units must be valid `WeightUnit` values.
- Operands and target units must be non-null.
- Category type mismatch (weight vs. length) returns false in equals().

## Example Outputs

**Equality Comparisons:**

- `Quantity(1.0, KILOGRAM).equals(Quantity(1.0, KILOGRAM))` → `true`
- `Quantity(1.0, KILOGRAM).equals(Quantity(1000.0, GRAM))` → `true`
- `Quantity(2.20462, POUND).equals(Quantity(1.0, KILOGRAM))` → `true`
- `Quantity(1.0, KILOGRAM).equals(Quantity(1.0, FOOT))` → `false` (incompatible categories)

**Unit Conversions:**

- `Quantity(1.0, KILOGRAM).convertTo(GRAM)` → `Quantity(1000.0, GRAM)`
- `Quantity(2.20462, POUND).convertTo(KILOGRAM)` → `Quantity(~1.0, KILOGRAM)`
- `Quantity(500.0, GRAM).convertTo(POUND)` → `Quantity(~1.10231, POUND)`

**Addition (Implicit Target Unit):**

- `Quantity(1.0, KILOGRAM).add(Quantity(2.0, KILOGRAM))` → `Quantity(3.0, KILOGRAM)`
- `Quantity(1.0, KILOGRAM).add(Quantity(1000.0, GRAM))` → `Quantity(2.0, KILOGRAM)`

**Addition (Explicit Target Unit):**

- `Quantity(1.0, KILOGRAM).add(Quantity(1000.0, GRAM), GRAM)` → `Quantity(2000.0, GRAM)`
- `Quantity(1.0, POUND).add(Quantity(453.592, GRAM), POUND)` → `Quantity(~2.0, POUND)`

## Key UC9 Concepts

- Multiple independent measurement categories
- Scalable generic design patterns
- Category type safety (weight vs. length incompatibility)
- Base unit normalization per category
- Conversion factor precision
- Enum-based responsibility assignment
- Immutability across categories

## UC9 Test Coverage

- Same-unit weight equality and inequality
- Cross-unit weight equality (kg ↔ g, kg ↔ lb, g ↔ lb)
- Weight vs. length incompatibility
- Unit conversions between all weight unit pairs
- Round-trip conversion accuracy
- Addition with implicit target unit
- Addition with explicit target unit
- Zero, negative, and large magnitude values
- Null and invalid input handling

## Backward Compatibility

- All UC1–UC8 length tests pass unchanged
- `QuantityLength` and `QuantityWeight` coexist without conflict
- No modifications to existing length code required

Run tests:

`dotnet test`
