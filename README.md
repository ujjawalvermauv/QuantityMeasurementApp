# QuantityMeasurementApp

## UC8: Refactoring Unit Enum to Standalone with Conversion Responsibility

UC8 refactors the length model by extracting `LengthUnit` into a standalone top-level enum and moving conversion responsibilities into the enum itself.

This removes conversion logic duplication inside `QuantityLength`, improves cohesion, reduces coupling, and keeps all behavior from UC1–UC7 unchanged.

## UC8 Design Goal

- `LengthUnit` handles unit-to-base and base-to-unit conversions.
- `QuantityLength` focuses on validation, equality, conversion orchestration, and arithmetic.
- Public API behavior remains backward compatible with UC1–UC7 client usage.

## Supported Units

- `FEET`
- `INCH`
- `YARD`
- `CENTIMETER`

Base unit for length remains `FEET`.

## Responsibilities After Refactor

### `LengthUnit` (Standalone Enum)

- `convertToBaseUnit(double value)`
- `convertFromBaseUnit(double baseValue)`
- Maintains conversion factors per unit

### `QuantityLength`

- Equality across same/cross units
- Unit conversion by delegating to `LengthUnit`
- Addition (UC6 and UC7 styles)
- Input validation and immutability

## Main Flow (UC8)

1. Extract `LengthUnit` as a top-level enum.
2. Implement conversion methods in `LengthUnit` for base-unit normalization.
3. Remove internal conversion formulas from `QuantityLength`.
4. Delegate conversion/equality/addition normalization through `LengthUnit`.
5. Keep all public behavior from UC1–UC7 unchanged.

## Validation Rules

- Operand values must be finite (`NaN`/`Infinity` are rejected).
- Operands must be non-null.
- Units and target units must be valid `LengthUnit` values.
- Invalid input throws `ArgumentException` or `ArgumentNullException`.

## Example Outputs

- `Quantity(1.0, FEET).convertTo(INCH)` -> `Quantity(12.0, INCH)`
- `Quantity(1.0, FEET).add(Quantity(12.0, INCH), FEET)` -> `Quantity(2.0, FEET)`
- `Quantity(36.0, INCH).equals(Quantity(1.0, YARD))` -> `true`
- `Quantity(1.0, YARD).add(Quantity(3.0, FEET), YARD)` -> `Quantity(2.0, YARD)`
- `Quantity(2.54, CENTIMETER).convertTo(INCH)` -> `Quantity(~1.0, INCH)`
- `LengthUnit.FEET.convertToBaseUnit(12.0)` -> `12.0`
- `LengthUnit.INCH.convertToBaseUnit(12.0)` -> `1.0`

## Postconditions

- `LengthUnit` is standalone and owns conversion logic.
- `QuantityLength` is simplified and conversion-aware via delegation.
- Circular dependency risk is reduced for future categories.
- SRP is reinforced (`LengthUnit` = conversion, `QuantityLength` = domain behavior).
- Equality/conversion/addition from UC1–UC7 remain functionally consistent.

## Key Concepts

- Single Responsibility Principle
- Separation of concerns
- Delegation pattern
- Encapsulation of conversion logic
- Backward-compatible refactoring
- Scalable architecture for future categories (`WeightUnit`, `VolumeUnit`, etc.)

## UC8 Test Coverage Checklist

- Standalone `LengthUnit` constants and conversion factors
- `convertToBaseUnit` / `convertFromBaseUnit` correctness for all units
- `QuantityLength` equality delegation with cross-unit comparisons
- `convertTo` delegation behavior and round-trip accuracy
- Addition (UC6 + UC7 styles) after refactor
- Null/invalid value validation
- Backward compatibility for UC1–UC7 tests
- Architectural readiness for additional measurement categories

Run tests:

`dotnet test`
