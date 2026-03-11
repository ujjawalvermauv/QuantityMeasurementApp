# QuantityMeasurementApp

## UC13: Centralized Arithmetic Logic (DRY Refactor)

UC13 refactors arithmetic operations introduced in UC12 to remove duplication while preserving behavior and public APIs.

The core design now routes `Add`, `Subtract`, and `Divide` through centralized private helpers inside `Quantity<U>`:

- `ValidateArithmeticOperands(...)`
- `PerformBaseArithmetic(...)`
- enum-based operation dispatch via `ArithmeticOperation`

This keeps all validation, base-unit normalization, and operation dispatch in one place.

## Public API (Unchanged)

- `Quantity<U> Add(Quantity<U> other)`
- `Quantity<U> Add(Quantity<U> other, U targetUnit)`
- `Quantity<U> Subtract(Quantity<U> other)`
- `Quantity<U> Subtract(Quantity<U> other, U targetUnit)`
- `double Divide(Quantity<U> other)`
- `Quantity<U> ConvertTo(U targetUnit)`

## Supported Unit Categories

- `LengthUnit`: `FEET`, `INCH`, `YARD`, `CENTIMETER`
- `WeightUnit`: `KILOGRAM`, `GRAM`
- `VolumeUnit`: `LITRE`, `MILLILITRE`

Base units used for normalization:

- Length -> `FEET`
- Weight -> `KILOGRAM`
- Volume -> `LITRE`

## UC13 Internal Flow

For `Add` and `Subtract`:

1. Validate operands and target unit.
2. Convert both quantities to base unit.
3. Compute via enum operation dispatch.
4. Convert result to target unit.
5. Round to two decimals and return new immutable `Quantity<U>`.

For `Divide`:

1. Validate operands.
2. Convert both quantities to base unit.
3. Compute base ratio.
4. Return raw `double` (no rounding).

## Validation and Error Handling

- Rejects null operands (`ArgumentNullException`).
- Rejects invalid enum unit values (`ArgumentException`).
- Rejects non-finite numeric values (`ArgumentException`).
- Rejects zero divisor in division (`ArithmeticException`).
- Cross-category arithmetic is prevented by generic type safety at compile time.

## Behavior Examples

Addition:

- `new Quantity<LengthUnit>(1.0, LengthUnit.FEET).Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH))`
  -> `Quantity(2, FEET)`
- `new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM).Add(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM), WeightUnit.GRAM)`
  -> `Quantity(15000, GRAM)`

Subtraction:

- `new Quantity<LengthUnit>(10.0, LengthUnit.FEET).Subtract(new Quantity<LengthUnit>(6.0, LengthUnit.INCH))`
  -> `Quantity(9.5, FEET)`
- `new Quantity<VolumeUnit>(5.0, VolumeUnit.LITRE).Subtract(new Quantity<VolumeUnit>(2.0, VolumeUnit.LITRE), VolumeUnit.MILLILITRE)`
  -> `Quantity(3000, MILLILITRE)`

Division:

- `new Quantity<LengthUnit>(24.0, LengthUnit.INCH).Divide(new Quantity<LengthUnit>(2.0, LengthUnit.FEET))`
  -> `1.0`

## Running the Application

```bash
dotnet run --project QuantityMeasurementApp
```

## Running Unit Tests

```bash
dotnet test
```

## UC13 Outcomes

- DRY principle enforced for arithmetic paths.
- Public behavior remains backward compatible with UC12.
- Validation and conversion logic are maintained in one place.
- Future operations can follow the same operation-dispatch pattern.
