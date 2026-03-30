using System;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Controller
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementController(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository
        )
        {
            _service = service;
            _repository = repository;
        }

        public void DisplayResult(string message)
        {
            Console.WriteLine(message);
        }

        public void PerformCompare(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                var result = _service.Compare(a, b);
                DisplayResult($"Comparison result: {result}");
                SaveAudit($"Compare: {a.Value} {a.Unit} vs {b.Value} {b.Unit} => {result}");
            }
            catch (Business.Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
                SaveAuditError(
                    $"Compare failed for {a.Value} {a.Unit} and {b.Value} {b.Unit}",
                    ex.Message
                );
            }
        }

        public void PerformConvert(QuantityDTO source, string targetUnit)
        {
            try
            {
                var result = _service.Convert(source, targetUnit);
                DisplayResult($"Converted result: {result.Value} {result.Unit}");
                SaveAudit(
                    $"Convert: {source.Value} {source.Unit} to {targetUnit} => {result.Value} {result.Unit}"
                );
            }
            catch (Business.Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
                SaveAuditError(
                    $"Convert failed for {source.Value} {source.Unit} to {targetUnit}",
                    ex.Message
                );
            }
        }

        public void PerformAdd(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                var result = _service.Add(a, b, targetUnit);
                DisplayResult($"Addition result: {result.Value} {result.Unit}");
                SaveAudit(
                    $"Add: {a.Value} {a.Unit} + {b.Value} {b.Unit} => {result.Value} {result.Unit}"
                );
            }
            catch (Business.Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
                SaveAuditError(
                    $"Add failed for {a.Value} {a.Unit} and {b.Value} {b.Unit}",
                    ex.Message
                );
            }
        }

        public void PerformSubtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                var result = _service.Subtract(a, b, targetUnit);
                DisplayResult($"Subtraction result: {result.Value} {result.Unit}");
                SaveAudit(
                    $"Subtract: {a.Value} {a.Unit} - {b.Value} {b.Unit} => {result.Value} {result.Unit}"
                );
            }
            catch (Business.Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
                SaveAuditError(
                    $"Subtract failed for {a.Value} {a.Unit} and {b.Value} {b.Unit}",
                    ex.Message
                );
            }
        }

        public void PerformDivide(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                var result = _service.Divide(a, b);
                DisplayResult($"Division result: {result}");
                SaveAudit($"Divide: {a.Value} {a.Unit} / {b.Value} {b.Unit} => {result}");
            }
            catch (Business.Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
                SaveAuditError(
                    $"Divide failed for {a.Value} {a.Unit} and {b.Value} {b.Unit}",
                    ex.Message
                );
            }
        }

        private void SaveAudit(string description)
        {
            _repository.Save(new QuantityMeasurementEntity(description));
        }

        private void SaveAuditError(string description, string error)
        {
            _repository.Save(new QuantityMeasurementEntity(description, error));
        }
    }
}
