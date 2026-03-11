# QuantityMeasurementApp

## UC14: Temperature Measurement with Selective Arithmetic Support

UC14 extends the system to support temperature (`CELSIUS`, `FAHRENHEIT`, `KELVIN`) while preserving UC1-UC13 behavior for length, weight, and volume.

Unlike length/weight/volume, temperature arithmetic is intentionally restricted:

- Temperature supports equality and conversion.
- Temperature rejects arithmetic (`Add`, `Subtract`, `Divide`) with clear exceptions.

This is implemented through an `IMeasurable` capability layer with default behavior and category-specific override.

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
- `TemperatureUnit`: `CELSIUS`, `FAHRENHEIT`, `KELVIN`

Base units used for normalization:

- Length -> `FEET`
- Weight -> `KILOGRAM`
- Volume -> `LITRE`
- Temperature -> `CELSIUS`

## IMeasurable Refactor (UC14)

UC14 introduces optional arithmetic capability support through:

- `SupportsArithmetic` delegate (`() => bool`)
- `IMeasurable` default methods:
  - `SupportsArithmeticCheck => () => true`
  - `SupportsArithmetic()`
  - `ValidateOperationSupport(string operation)` (default no-op)

Implementation strategy in this C# codebase:

- Non-temperature categories use default measurable behavior (arithmetic allowed).
- Temperature uses a specialized measurable that always rejects arithmetic and throws `UnsupportedOperationException`.

## Quantity Flow

For `Add` and `Subtract`:

1. Validate operands and target unit.
2. Validate operation support through measurable capability.
3. Convert both quantities to base unit.
4. Compute via enum operation dispatch.
5. Convert result to target unit.
6. Round to two decimals and return new immutable `Quantity<U>`.

For `Divide`:

1. Validate operands.
2. Validate operation support through measurable capability.
3. Convert both quantities to base unit.
4. Compute base ratio.
5. Return raw `double` (no rounding).

## Validation and Error Handling

- Rejects null operands (`ArgumentNullException`).
- Rejects invalid enum unit values (`ArgumentException`).
- Rejects non-finite numeric values (`ArgumentException`).
- Rejects zero divisor in division (`ArithmeticException`).
- Rejects unsupported temperature arithmetic (`UnsupportedOperationException`).
- Cross-category arithmetic is prevented by generic type safety at compile time.

## Behavior Examples

Temperature Equality:

- `new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS).Equals(new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT))`
  -> `true`
- `new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN).Equals(new Quantity<TemperatureUnit>(0.0, TemperatureUnit.CELSIUS))`
  -> `true`

Temperature Conversion:

- `new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS).ConvertTo(TemperatureUnit.FAHRENHEIT)`
  -> `Quantity(212, FAHRENHEIT)`
- `new Quantity<TemperatureUnit>(32.0, TemperatureUnit.FAHRENHEIT).ConvertTo(TemperatureUnit.CELSIUS)`
  -> `Quantity(0, CELSIUS)`
- `new Quantity<TemperatureUnit>(273.15, TemperatureUnit.KELVIN).ConvertTo(TemperatureUnit.CELSIUS)`
  -> `Quantity(0, CELSIUS)`

Unsupported Temperature Arithmetic:

- `new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS).Add(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS))`
  -> throws `UnsupportedOperationException`
- `new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS).Subtract(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS))`
  -> throws `UnsupportedOperationException`
- `new Quantity<TemperatureUnit>(100.0, TemperatureUnit.CELSIUS).Divide(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.CELSIUS))`
  -> throws `UnsupportedOperationException`

Non-Temperature Arithmetic (unchanged):

- `new Quantity<LengthUnit>(1.0, LengthUnit.FEET).Add(new Quantity<LengthUnit>(12.0, LengthUnit.INCH))`
  -> `Quantity(2, FEET)`
- `new Quantity<WeightUnit>(10.0, WeightUnit.KILOGRAM).Add(new Quantity<WeightUnit>(5000.0, WeightUnit.GRAM), WeightUnit.GRAM)`
  -> `Quantity(15000, GRAM)`

## Running the Application

```bash
dotnet run --project QuantityMeasurementApp
```

## Running Unit Tests

```bash
dotnet test
```

## UC14 Outcomes

- Temperature support added with accurate conversion formulas.
- Existing length/weight/volume arithmetic behavior remains unchanged.
- Arithmetic capability is now selectively enforced by measurable category.
- Unsupported temperature arithmetic fails fast with clear error messages.
- The design is extensible for future categories with custom operation constraints.
