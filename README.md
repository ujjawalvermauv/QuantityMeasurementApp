# QuantityMeasurementApp

## UC7: Addition with Target Unit Specification

UC7 extends UC6 by allowing the caller to explicitly choose the result unit for addition.
Instead of always returning the sum in the first operand's unit, the API supports:

- implicit target unit (UC6 behavior)
- explicit target unit (UC7 behavior)

All operations are immutable and return a new `QuantityLength` instance.

## Supported Units

- `FEET`
- `INCH`
- `YARD`
- `CENTIMETER`

All conversions are normalized through the base unit `FEET`.

## Addition API

- `QuantityLength Add(QuantityLength other)`
- `static QuantityLength Add(QuantityLength first, QuantityLength second)`
- `static QuantityLength Add(QuantityLength first, QuantityLength second, LengthUnit targetUnit)`
- `static QuantityLength Add(double firstValue, LengthUnit firstUnit, double secondValue, LengthUnit secondUnit, LengthUnit targetUnit)`

## UC7 Main Flow

1. Validate both operands and target unit.
2. Convert both operands to base unit (`FEET`).
3. Add normalized values.
4. Convert sum to explicitly specified `targetUnit`.
5. Return new immutable `QuantityLength`.

## Validation Rules

- Operand values must be finite (`NaN`/`Infinity` are rejected).
- Operands must be non-null.
- Target unit must be a valid `LengthUnit`.
- Invalid input throws `ArgumentException` or `ArgumentNullException`.

## UC7 Example Outputs

- `add(Quantity(1.0, FEET), Quantity(12.0, INCH), FEET)` -> `Quantity(2.0, FEET)`
- `add(Quantity(1.0, FEET), Quantity(12.0, INCH), INCH)` -> `Quantity(24.0, INCH)`
- `add(Quantity(1.0, FEET), Quantity(12.0, INCH), YARD)` -> `Quantity(~0.667, YARD)`
- `add(Quantity(1.0, YARD), Quantity(3.0, FEET), YARD)` -> `Quantity(2.0, YARD)`
- `add(Quantity(36.0, INCH), Quantity(1.0, YARD), FEET)` -> `Quantity(6.0, FEET)`
- `add(Quantity(2.54, CENTIMETER), Quantity(1.0, INCH), CENTIMETER)` -> `Quantity(~5.08, CENTIMETER)`
- `add(Quantity(5.0, FEET), Quantity(0.0, INCH), YARD)` -> `Quantity(~1.667, YARD)`
- `add(Quantity(5.0, FEET), Quantity(-2.0, FEET), INCH)` -> `Quantity(36.0, INCH)`

## Key UC7 Concepts

- Method overloading for implicit and explicit target unit behavior.
- Reuse of conversion infrastructure from UC5/UC6.
- DRY design through common base-unit normalization.
- Commutativity preserved with fixed target unit.
- Precision handled via epsilon-based assertions in tests.

## UC7 Test Coverage

- Explicit target = first operand unit
- Explicit target = second operand unit
- Explicit target different from both operands
- Commutativity with explicit target unit
- Zero and negative values with target conversion
- Large-to-small and small-to-large scale conversion
- Null/invalid input validation

Run tests:

`dotnet test`
