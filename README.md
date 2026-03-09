# QuantityMeasurementApp

## UC12: Subtraction and Division Operations on Quantity Measurements

This use case introduces subtraction and division into the generic quantity model while preserving previous functionality (equality, conversion, and addition).

The project now uses a generic immutable type:

- `Quantity<U>` where `U : struct, Enum`

Supported categories:

- `LengthUnit` (`FEET`, `INCH`, `YARD`, `CENTIMETER`)
- `WeightUnit` (`KILOGRAM`, `GRAM`)
- `VolumeUnit` (`LITRE`, `MILLILITRE`)

Conversions are normalized via a category-specific base unit:

- Length base: `FEET`
- Weight base: `KILOGRAM`
- Volume base: `LITRE`

## UC12 APIs

- `Quantity<U> Add(Quantity<U> other)`
- `Quantity<U> Add(Quantity<U> other, U targetUnit)`
- `Quantity<U> Subtract(Quantity<U> other)`
- `Quantity<U> Subtract(Quantity<U> other, U targetUnit)`
- `double Divide(Quantity<U> other)`
- `Quantity<U> ConvertTo(U targetUnit)`

## Arithmetic Behavior

Subtraction:

- Supports same-unit and cross-unit arithmetic within a category.
- Implicit result unit is the first operand unit.
- Explicit target unit can be specified.
- Result is rounded to 2 decimal places.
- Returns a new immutable `Quantity<U>`.

Division:

- Supports same-unit and cross-unit division within a category.
- Returns a dimensionless `double` ratio.
- Does not round the ratio result.
- Throws `ArithmeticException` when divisor is zero.

## Validation Rules

- Values must be finite (`NaN` and `Infinity` are rejected).
- Operand quantities must be non-null.
- Target units must be valid enum values.
- Cross-category arithmetic is prevented by generic type safety (`Quantity<LengthUnit>` cannot operate with `Quantity<WeightUnit>`).

## Example Operations

Subtraction:

- `new Quantity<LengthUnit>(10.0, LengthUnit.FEET).Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH))`
  returns `Quantity(9.5, FEET)`
- `new Quantity<LengthUnit>(10.0, LengthUnit.FEET).Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH), LengthUnit.INCH)`
  returns `Quantity(114, INCH)`
- `new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM).Subtract(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM))`
  returns `Quantity(5, KILOGRAM)`

Division:

- `new Quantity<LengthUnit>(24.0, LengthUnit.INCH).Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET))`
  returns `1.0`
- `new Quantity<WeightUnit>(2000.0, WeightUnit.GRAM).Divide(new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM))`
  returns `2.0`
- `new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE).Divide(new Quantity<VolumeUnit>(10.0, VolumeUnit.LITRE))`
  returns `0.5`

## Running the App

```bash
dotnet run --project QuantityMeasurementApp
```

## Running Tests

```bash
dotnet test
```

## SOLID / Object Calisthenics Notes

- `Quantity<U>` follows SRP for quantity state and arithmetic behavior.
- `UnitConverter` centralizes conversion logic to avoid duplication.
- Immutability is preserved in all operations.
- Current conversion dispatch uses type checks in `UnitConverter`; for future extensibility this can be refactored toward a strategy/provider model per unit category.
