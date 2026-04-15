using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Globalization;
using QuantityMeasurementApp.Api.Contracts;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/v1/quantities")]
public class QuantityMeasurementsController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;
    private readonly IQuantityMeasurementRepository _repository;

    public QuantityMeasurementsController(
        IQuantityMeasurementService service,
        IQuantityMeasurementRepository repository)
    {
        _service = service;
        _repository = repository;
    }

    [HttpPost("compare")]
    [AllowAnonymous]
    public ActionResult<OperationResultDto> Compare([FromBody] BinaryOperationRequestDto request)
    {
        var userScope = GetCurrentUserScope();
        var first = ToDomain(request.First);
        var second = ToDomain(request.Second);

        try
        {
            var result = _service.Compare(first, second);

            var response = new OperationResultDto
            {
                Operation = "COMPARE",
                Description = "Compares two quantities for equality after unit normalization.",
                First = request.First,
                Second = request.Second,
                BooleanResult = result,
                TimestampUtc = DateTime.UtcNow,
                Message = result
                    ? "Comparison completed. The two quantities are equal."
                    : "Comparison completed. The two quantities are not equal."
            };

            SaveAuditSuccess(
                userScope,
                first.Category.ToString(),
                "Compare",
                BuildCompareInput(first, second),
                result.ToString()
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                userScope,
                first.Category.ToString(),
                "Compare",
                BuildCompareInput(first, second),
                ex.Message
            );
            throw;
        }
    }

    [HttpPost("convert")]
    [AllowAnonymous]
    public ActionResult<OperationResultDto> Convert([FromBody] ConvertOperationRequestDto request)
    {
        var userScope = GetCurrentUserScope();
        var source = ToDomain(request.Source);

        try
        {
            var result = _service.Convert(source, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "CONVERT",
                Description = "Converts a quantity value from one unit to another within the same category.",
                First = request.Source,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow,
                Message = $"Conversion completed: {request.Source.Value} {request.Source.Unit} -> {result.Value} {result.Unit}."
            };

            SaveAuditSuccess(
                userScope,
                source.Category.ToString(),
                "Convert",
                BuildConvertInput(source, request.TargetUnit),
                BuildQuantityResult(result)
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                userScope,
                source.Category.ToString(),
                "Convert",
                BuildConvertInput(source, request.TargetUnit),
                ex.Message
            );
            throw;
        }
    }

    [HttpPost("add")]
    [AllowAnonymous]
    public ActionResult<OperationResultDto> Add([FromBody] BinaryOperationRequestDto request)
    {
        var userScope = GetCurrentUserScope();
        var first = ToDomain(request.First);
        var second = ToDomain(request.Second);

        try
        {
            var result = _service.Add(first, second, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "ADD",
                Description = "Adds two quantities and returns the result in the requested target unit.",
                First = request.First,
                Second = request.Second,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow,
                Message = $"Addition completed: {result.Value} {result.Unit}."
            };

            SaveAuditSuccess(
                userScope,
                first.Category.ToString(),
                "Add",
                BuildAddInput(first, second),
                BuildQuantityResult(result)
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                userScope,
                first.Category.ToString(),
                "Add",
                BuildAddInput(first, second),
                ex.Message
            );
            throw;
        }
    }

    [HttpPost("subtract")]
    [AllowAnonymous]
    public ActionResult<OperationResultDto> Subtract([FromBody] BinaryOperationRequestDto request)
    {
        var userScope = GetCurrentUserScope();
        var first = ToDomain(request.First);
        var second = ToDomain(request.Second);

        try
        {
            var result = _service.Subtract(first, second, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "SUBTRACT",
                Description = "Subtracts the second quantity from the first and returns the result in target unit.",
                First = request.First,
                Second = request.Second,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow,
                Message = $"Subtraction completed: {result.Value} {result.Unit}."
            };

            SaveAuditSuccess(
                userScope,
                first.Category.ToString(),
                "Subtract",
                BuildSubtractInput(first, second),
                BuildQuantityResult(result)
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                userScope,
                first.Category.ToString(),
                "Subtract",
                BuildSubtractInput(first, second),
                ex.Message
            );
            throw;
        }
    }

    [HttpPost("divide")]
    [AllowAnonymous]
    public ActionResult<OperationResultDto> Divide([FromBody] BinaryOperationRequestDto request)
    {
        var userScope = GetCurrentUserScope();
        var first = ToDomain(request.First);
        var second = ToDomain(request.Second);

        try
        {
            var result = _service.Divide(first, second);

            var response = new OperationResultDto
            {
                Operation = "DIVIDE",
                Description = "Divides the first quantity by the second quantity and returns a scalar value.",
                First = request.First,
                Second = request.Second,
                ScalarResult = result,
                TimestampUtc = DateTime.UtcNow,
                Message = $"Division completed. Quotient: {result.ToString(CultureInfo.InvariantCulture)}."
            };

            SaveAuditSuccess(
                userScope,
                first.Category.ToString(),
                "Divide",
                BuildDivideInput(first, second),
                result.ToString(CultureInfo.InvariantCulture)
            );
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                userScope,
                first.Category.ToString(),
                "Divide",
                BuildDivideInput(first, second),
                ex.Message
            );
            throw;
        }
    }

    [HttpGet("history")]
    [Authorize]
    public ActionResult<IEnumerable<OperationHistoryDto>> GetHistory()
    {
        var userScope = GetCurrentUserScope();
        var userHistory = _repository
            .GetAll()
            .Where(x => string.Equals(x.UserScope, userScope, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new OperationHistoryDto
            {
                Id = x.Id,
                Type = x.Type,
                Operation = x.Operation,
                Input = x.Input,
                Result = x.Result,
                CreatedAt = x.CreatedAt,
            });

        return Ok(userHistory);
    }

    private void SaveAuditSuccess(
        string userScope,
        string type,
        string operation,
        string input,
        string result
    )
    {
        _repository.Save(new QuantityMeasurementEntity(type, operation, input, result, userScope));
    }

    private void SaveAuditError(
        string userScope,
        string type,
        string operation,
        string input,
        string error
    )
    {
        _repository.Save(new QuantityMeasurementEntity(type, operation, input, "-", error, userScope));
    }

    private static QuantityDTO ToDomain(ApiQuantityDto dto)
    {
        return new QuantityDTO(dto.Value ?? 0, dto.Unit, dto.Category);
    }

    private static string BuildConvertInput(QuantityDTO source, string targetUnit)
    {
        return $"{source.Value} {source.Unit} to {targetUnit}";
    }

    private static string BuildCompareInput(QuantityDTO first, QuantityDTO second)
    {
        return $"{first.Value} {first.Unit} vs {second.Value} {second.Unit}";
    }

    private static string BuildAddInput(QuantityDTO first, QuantityDTO second)
    {
        return $"{first.Value} {first.Unit} + {second.Value} {second.Unit}";
    }

    private static string BuildSubtractInput(QuantityDTO first, QuantityDTO second)
    {
        return $"{first.Value} {first.Unit} - {second.Value} {second.Unit}";
    }

    private static string BuildDivideInput(QuantityDTO first, QuantityDTO second)
    {
        return $"{first.Value} {first.Unit} / {second.Value} {second.Unit}";
    }

    private static string BuildQuantityResult(QuantityDTO dto)
    {
        return $"{dto.Value} {dto.Unit}";
    }

    private string GetCurrentUserScope()
    {
        // Support both raw JWT claims and framework-mapped claim types.
        var subClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst("sub")?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("nameid")?.Value;

        if (!string.IsNullOrWhiteSpace(subClaim))
        {
            return subClaim;
        }

        return "GUEST";
    }

}
