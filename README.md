# QuantityMeasurementApp

## UC9: Weight Measurement Equality, Conversion, and Addition

UC9 extends the application to support weight measurements alongside length measurements. This use case demonstrates that the generic patterns from UC1–UC8 scale seamlessly to multiple measurement categories.

Weight supports three units:

- `KILOGRAM` (kg) – base unit
- `GRAM` (g) – 1 kg = 1000 g
- `POUND` (lb) – 1 lb ≈ 0.453592 kg

## Design Consistency

- `WeightUnit` enum parallels `LengthUnit` with standalone conversion responsibility.
- `QuantityWeight` class parallels `QuantityLength` for weight measurements.
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
