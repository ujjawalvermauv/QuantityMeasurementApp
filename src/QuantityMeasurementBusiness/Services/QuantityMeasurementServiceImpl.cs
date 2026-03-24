using System;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Interfaces;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Units;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Quantities;
using QuantityMeasurementApp.QuantityMeasurementBusiness.Exceptions;
using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.QuantityMeasurementRepo.Interfaces;
using QuantityMeasurementApp.QuantityMeasurementRepo.Models;

namespace QuantityMeasurementApp.QuantityMeasurementBusiness.Services
{
    /// <summary>
    /// QuantityMeasurementServiceImpl implements IQuantityMeasurementService that provides functionality for quantity measurement operations.
    /// </summary>
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        /// <summary>
        /// Initializes a new instance of QuantityMeasurementServiceImpl.
        /// </summary>
        /// <param name="repository">The repository for persistence</param>
        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Compares two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with comparison result</returns>
        public QuantityDTO Compare(QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateInputs(dto1, dto2);

                var model1 = ConvertToModel(dto1);
                var model2 = ConvertToModel(dto2);

                var quantity1 = CreateQuantity(model1);
                var quantity2 = CreateQuantity(model2);

                bool result = quantity1.Equals(quantity2);

                var resultDto = new QuantityDTO
                {
                    Value = result ? 1 : 0, // 1 for equal, 0 for not equal
                    UnitName = "BOOLEAN",
                    MeasurementType = "Comparison"
                };

                SaveOperation(dto1, dto2, "COMPARE", resultDto);

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorEntity = new QuantityMeasurementEntity(dto1, dto2, "COMPARE", ex.Message);
                _repository.Save(errorEntity);
                throw new QuantityMeasurementException("Comparison failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Converts a quantity to another unit.
        /// </summary>
        /// <param name="dto">Quantity DTO to convert</param>
        /// <param name="targetUnitName">Target unit name</param>
        /// <returns>Result DTO with converted quantity</returns>
        public QuantityDTO Convert(QuantityDTO dto, string targetUnitName)
        {
            try
            {
                ValidateInput(dto);

                var model = ConvertToModel(dto);
                var quantity = CreateQuantity(model);
                var targetUnit = GetUnitFromName(dto.MeasurementType, targetUnitName);

                dynamic converted;
                if (dto.MeasurementType == "Length")
                {
                    converted = ((Quantity<LengthUnit>)quantity).ConvertTo((LengthUnit)targetUnit);
                }
                else if (dto.MeasurementType == "Weight")
                {
                    converted = ((Quantity<WeightUnit>)quantity).ConvertTo((WeightUnit)targetUnit);
                }
                else if (dto.MeasurementType == "Volume")
                {
                    converted = ((Quantity<VolumeUnit>)quantity).ConvertTo((VolumeUnit)targetUnit);
                }
                else if (dto.MeasurementType == "Temperature")
                {
                    converted = ((Quantity<TemperatureUnit>)quantity).ConvertTo((TemperatureUnit)targetUnit);
                }
                else
                {
                    throw new ArgumentException("Unsupported measurement type");
                }

                var resultDto = new QuantityDTO
                {
                    Value = converted.Value,
                    UnitName = targetUnitName,
                    MeasurementType = dto.MeasurementType
                };

                SaveOperation(dto, "CONVERT", resultDto);

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorEntity = new QuantityMeasurementEntity(dto, "CONVERT", ex.Message);
                _repository.Save(errorEntity);
                throw new QuantityMeasurementException("Conversion failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds two quantities.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with sum</returns>
        public QuantityDTO Add(QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateInputs(dto1, dto2);

                var model1 = ConvertToModel(dto1);
                var model2 = ConvertToModel(dto2);

                var quantity1 = CreateQuantity(model1);
                var quantity2 = CreateQuantity(model2);

                dynamic sum = quantity1.Add(quantity2);

                var resultDto = new QuantityDTO
                {
                    Value = sum.Value,
                    UnitName = dto1.UnitName, // Use first unit as result unit
                    MeasurementType = dto1.MeasurementType
                };

                SaveOperation(dto1, dto2, "ADD", resultDto);

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorEntity = new QuantityMeasurementEntity(dto1, dto2, "ADD", ex.Message);
                _repository.Save(errorEntity);
                throw new QuantityMeasurementException("Addition failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Subtracts the second quantity from the first.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with difference</returns>
        public QuantityDTO Subtract(QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateInputs(dto1, dto2);

                var model1 = ConvertToModel(dto1);
                var model2 = ConvertToModel(dto2);

                var quantity1 = CreateQuantity(model1);
                var quantity2 = CreateQuantity(model2);

                var difference = quantity1.Subtract(quantity2);

                var resultDto = new QuantityDTO
                {
                    Value = difference.Value,
                    UnitName = dto1.UnitName,
                    MeasurementType = dto1.MeasurementType
                };

                SaveOperation(dto1, dto2, "SUBTRACT", resultDto);

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorEntity = new QuantityMeasurementEntity(dto1, dto2, "SUBTRACT", ex.Message);
                _repository.Save(errorEntity);
                throw new QuantityMeasurementException("Subtraction failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Divides the first quantity by the second.
        /// </summary>
        /// <param name="dto1">First quantity DTO</param>
        /// <param name="dto2">Second quantity DTO</param>
        /// <returns>Result DTO with quotient</returns>
        public QuantityDTO Divide(QuantityDTO dto1, QuantityDTO dto2)
        {
            try
            {
                ValidateInputs(dto1, dto2);

                var model1 = ConvertToModel(dto1);
                var model2 = ConvertToModel(dto2);

                var quantity1 = CreateQuantity(model1);
                var quantity2 = CreateQuantity(model2);

                double quotient = quantity1.Divide(quantity2);

                var resultDto = new QuantityDTO
                {
                    Value = quotient,
                    UnitName = "DIMENSIONLESS",
                    MeasurementType = "Scalar"
                };

                SaveOperation(dto1, dto2, "DIVIDE", resultDto);

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorEntity = new QuantityMeasurementEntity(dto1, dto2, "DIVIDE", ex.Message);
                _repository.Save(errorEntity);
                throw new QuantityMeasurementException("Division failed: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Validates a single input DTO.
        /// </summary>
        /// <param name="dto">The DTO to validate</param>
        private void ValidateInput(QuantityDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrEmpty(dto.UnitName)) throw new ArgumentException("Unit name is required");
            if (string.IsNullOrEmpty(dto.MeasurementType)) throw new ArgumentException("Measurement type is required");
        }

        /// <summary>
        /// Validates two input DTOs.
        /// </summary>
        /// <param name="dto1">First DTO</param>
        /// <param name="dto2">Second DTO</param>
        private void ValidateInputs(QuantityDTO dto1, QuantityDTO dto2)
        {
            ValidateInput(dto1);
            ValidateInput(dto2);
            if (dto1.MeasurementType != dto2.MeasurementType)
                throw new QuantityMeasurementException("Cannot perform operation on different measurement types");
        }

        /// <summary>
        /// Converts DTO to QuantityModel.
        /// </summary>
        /// <param name="dto">The DTO</param>
        /// <returns>QuantityModel</returns>
        private QuantityModel ConvertToModel(QuantityDTO dto)
        {
            var unit = GetUnitFromName(dto.MeasurementType, dto.UnitName);
            return new QuantityModel { Value = dto.Value, Unit = (Enum)unit };
        }

        /// <summary>
        /// Creates a Quantity from QuantityModel.
        /// </summary>
        /// <param name="model">The model</param>
        /// <returns>Quantity</returns>
        private dynamic CreateQuantity(QuantityModel model)
        {
            // Since Quantity is generic, we need to create the appropriate type
            // This is a simplified version; in practice, we might need type-specific handling
            if (model.Unit is LengthUnit lengthUnit)
                return new Quantity<LengthUnit>(model.Value, lengthUnit);
            if (model.Unit is WeightUnit weightUnit)
                return new Quantity<WeightUnit>(model.Value, weightUnit);
            if (model.Unit is VolumeUnit volumeUnit)
                return new Quantity<VolumeUnit>(model.Value, volumeUnit);
            if (model.Unit is TemperatureUnit temperatureUnit)
                return new Quantity<TemperatureUnit>(model.Value, temperatureUnit);
            throw new ArgumentException("Unsupported unit type");
        }

        /// <summary>
        /// Gets unit enum from name and type.
        /// </summary>
        /// <param name="measurementType">The measurement type</param>
        /// <param name="unitName">The unit name</param>
        /// <returns>The unit enum</returns>
        private object GetUnitFromName(string measurementType, string unitName)
        {
            return measurementType switch
            {
                "Length" => (LengthUnit)Enum.Parse(typeof(LengthUnit), unitName, true),
                "Weight" => (WeightUnit)Enum.Parse(typeof(WeightUnit), unitName, true),
                "Volume" => (VolumeUnit)Enum.Parse(typeof(VolumeUnit), unitName, true),
                "Temperature" => (TemperatureUnit)Enum.Parse(typeof(TemperatureUnit), unitName, true),
                _ => throw new ArgumentException("Unsupported measurement type")
            };
        }

        /// <summary>
        /// Saves the operation to repository.
        /// </summary>
        /// <param name="dto1">First DTO</param>
        /// <param name="dto2">Second DTO</param>
        /// <param name="operation">Operation name</param>
        /// <param name="result">Result DTO</param>
        private void SaveOperation(QuantityDTO dto1, QuantityDTO dto2, string operation, QuantityDTO result)
        {
            var entity = new QuantityMeasurementEntity(dto1, dto2, operation, result);
            _repository.Save(entity);
        }

        /// <summary>
        /// Saves the single operand operation to repository.
        /// </summary>
        /// <param name="dto">DTO</param>
        /// <param name="operation">Operation name</param>
        /// <param name="result">Result DTO</param>
        private void SaveOperation(QuantityDTO dto, string operation, QuantityDTO result)
        {
            var entity = new QuantityMeasurementEntity(dto, operation, result);
            _repository.Save(entity);
        }
    }
}