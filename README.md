# QuantityMeasurementApp

## UC6: Addition of Two Length Units (Same Category)

UC6 adds arithmetic support to the `QuantityLength` value object.
Two length quantities (same measurement category) can be added even when units differ.
The result is returned in the unit of the first operand by default, or in an explicit target unit when provided.

## Supported Units

- `FEET`
- `INCH`
- `YARD`
- `CENTIMETER`

All conversions are normalized to a base unit (`FEET`) before arithmetic.

## UC6 API

- `QuantityLength Add(QuantityLength other)`
- `static QuantityLength Add(QuantityLength first, QuantityLength second)`
- `static QuantityLength Add(QuantityLength first, QuantityLength second, LengthUnit targetUnit)`
- `static QuantityLength Add(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit, LengthUnit targetUnit)`

## Validation Rules

- Values must be finite (`NaN` and `Infinity` are rejected).
- Operands must be non-null.
- Units must be valid `LengthUnit` enum values.
- Invalid input throws `ArgumentException` or `ArgumentNullException`.

## Example Outputs

- `add(Quantity(1.0, FEET), Quantity(2.0, FEET))` -> `Quantity(3.0, FEET)`
- `add(Quantity(1.0, FEET), Quantity(12.0, INCH))` -> `Quantity(2.0, FEET)`
- `add(Quantity(12.0, INCH), Quantity(1.0, FEET))` -> `Quantity(24.0, INCH)`
- `add(Quantity(1.0, YARD), Quantity(3.0, FEET))` -> `Quantity(2.0, YARD)`
- `add(Quantity(2.54, CENTIMETER), Quantity(1.0, INCH))` -> `Quantity(~5.08, CENTIMETER)`
- `add(Quantity(5.0, FEET), Quantity(0.0, INCH))` -> `Quantity(5.0, FEET)`
- `add(Quantity(5.0, FEET), Quantity(-2.0, FEET))` -> `Quantity(3.0, FEET)`

## UC6 Test Coverage

Implemented tests include:

- Same-unit addition
- Cross-unit addition
- Commutativity using explicit target unit
- Identity element (`+ 0`)
- Negative values
- Null operand handling
- Large and small values

Run tests:

`dotnet test`
