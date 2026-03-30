using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuantityMeasurementApp.Api.Contracts;
using QuantityMeasurementApp.Api.Messaging;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/quantities")]
public class QuantityMeasurementsController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;
    private readonly IQuantityMeasurementRepository _repository;
    private readonly IOperationEventPublisher _publisher;

    public QuantityMeasurementsController(
        IQuantityMeasurementService service,
        IQuantityMeasurementRepository repository,
        IOperationEventPublisher publisher)
    {
        _service = service;
        _repository = repository;
        _publisher = publisher;
    }

    [HttpPost("compare")]
    public async Task<ActionResult<OperationResultDto>> Compare([FromBody] BinaryOperationRequestDto request)
    {
        try
        {
            var first = ToDomain(request.First);
            var second = ToDomain(request.Second);
            var result = _service.Compare(first, second);

            var response = new OperationResultDto
            {
                Operation = "COMPARE",
                First = request.First,
                Second = request.Second,
                BooleanResult = result,
                TimestampUtc = DateTime.UtcNow
            };

            SaveAuditSuccess($"COMPARE|{BuildBinaryDescription(first, second)}|RESULT={result}");
            await PublishAsync("COMPARE", true, null, first.Category.ToString(), first.Unit);
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError($"COMPARE|{BuildBinaryDescription(ToDomain(request.First), ToDomain(request.Second))}", ex.Message);
            await PublishAsync("COMPARE", false, ex.Message, request.First.Category.ToString(), request.First.Unit);
            throw;
        }
    }

    [HttpPost("convert")]
    public async Task<ActionResult<OperationResultDto>> Convert([FromBody] ConvertOperationRequestDto request)
    {
        try
        {
            var source = ToDomain(request.Source);
            var result = _service.Convert(source, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "CONVERT",
                First = request.Source,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow
            };

            SaveAuditSuccess($"CONVERT|SRC={Describe(source)}|TARGET={request.TargetUnit}|RESULT={Describe(result)}");
            await PublishAsync("CONVERT", true, null, result.Category.ToString(), result.Unit);
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError($"CONVERT|SRC={Describe(ToDomain(request.Source))}|TARGET={request.TargetUnit}", ex.Message);
            await PublishAsync("CONVERT", false, ex.Message, request.Source.Category.ToString(), request.Source.Unit);
            throw;
        }
    }

    [HttpPost("add")]
    public async Task<ActionResult<OperationResultDto>> Add([FromBody] BinaryOperationRequestDto request)
    {
        try
        {
            var first = ToDomain(request.First);
            var second = ToDomain(request.Second);
            var result = _service.Add(first, second, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "ADD",
                First = request.First,
                Second = request.Second,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow
            };

            SaveAuditSuccess($"ADD|{BuildBinaryDescription(first, second)}|TARGET={request.TargetUnit}|RESULT={Describe(result)}");
            await PublishAsync("ADD", true, null, result.Category.ToString(), result.Unit);
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError($"ADD|{BuildBinaryDescription(ToDomain(request.First), ToDomain(request.Second))}", ex.Message);
            await PublishAsync("ADD", false, ex.Message, request.First.Category.ToString(), request.First.Unit);
            throw;
        }
    }

    [HttpPost("subtract")]
    public async Task<ActionResult<OperationResultDto>> Subtract([FromBody] BinaryOperationRequestDto request)
    {
        try
        {
            var first = ToDomain(request.First);
            var second = ToDomain(request.Second);
            var result = _service.Subtract(first, second, request.TargetUnit);

            var response = new OperationResultDto
            {
                Operation = "SUBTRACT",
                First = request.First,
                Second = request.Second,
                QuantityResult = result,
                TimestampUtc = DateTime.UtcNow
            };

            SaveAuditSuccess($"SUBTRACT|{BuildBinaryDescription(first, second)}|TARGET={request.TargetUnit}|RESULT={Describe(result)}");
            await PublishAsync("SUBTRACT", true, null, result.Category.ToString(), result.Unit);
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError($"SUBTRACT|{BuildBinaryDescription(ToDomain(request.First), ToDomain(request.Second))}", ex.Message);
            await PublishAsync("SUBTRACT", false, ex.Message, request.First.Category.ToString(), request.First.Unit);
            throw;
        }
    }

    [HttpPost("divide")]
    public async Task<ActionResult<OperationResultDto>> Divide([FromBody] BinaryOperationRequestDto request)
    {
        try
        {
            var first = ToDomain(request.First);
            var second = ToDomain(request.Second);
            var result = _service.Divide(first, second);

            var response = new OperationResultDto
            {
                Operation = "DIVIDE",
                First = request.First,
                Second = request.Second,
                ScalarResult = result,
                TimestampUtc = DateTime.UtcNow
            };

            SaveAuditSuccess($"DIVIDE|{BuildBinaryDescription(first, second)}|RESULT={result}");
            await PublishAsync("DIVIDE", true, null, first.Category.ToString(), first.Unit);
            return Ok(response);
        }
        catch (Exception ex)
        {
            SaveAuditError($"DIVIDE|{BuildBinaryDescription(ToDomain(request.First), ToDomain(request.Second))}", ex.Message);
            await PublishAsync("DIVIDE", false, ex.Message, request.First.Category.ToString(), request.First.Unit);
            throw;
        }
    }

    [HttpGet("history")]
    public ActionResult<IEnumerable<QuantityMeasurementEntity>> GetHistory()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("history/operation/{operation}")]
    public ActionResult<IEnumerable<QuantityMeasurementEntity>> GetHistoryByOperation(string operation)
    {
        var marker = operation.Trim().ToUpperInvariant() + "|";
        var history = _repository.GetAll()
            .Where(x => x.Description.StartsWith(marker, StringComparison.OrdinalIgnoreCase));

        return Ok(history);
    }

    [HttpGet("history/type/{category}")]
    public ActionResult<IEnumerable<QuantityMeasurementEntity>> GetHistoryByCategory(string category)
    {
        var history = _repository.GetAll()
            .Where(x => x.Description.Contains($"CAT={category}", StringComparison.OrdinalIgnoreCase));

        return Ok(history);
    }

    [HttpGet("history/errored")]
    public ActionResult<IEnumerable<QuantityMeasurementEntity>> GetErroredHistory()
    {
        return Ok(_repository.GetAll().Where(x => x.IsError));
    }

    [HttpGet("count/{operation}")]
    public ActionResult<object> GetCountByOperation(string operation)
    {
        var marker = operation.Trim().ToUpperInvariant() + "|";
        var count = _repository.GetAll()
            .Count(x => x.Description.StartsWith(marker, StringComparison.OrdinalIgnoreCase) && !x.IsError);

        return Ok(new { operation = operation.ToUpperInvariant(), count });
    }

    private void SaveAuditSuccess(string description)
    {
        _repository.Save(new QuantityMeasurementEntity(description));
    }

    private void SaveAuditError(string description, string error)
    {
        _repository.Save(new QuantityMeasurementEntity(description, error));
    }

    private async Task PublishAsync(string operation, bool success, string? error, string? category, string? unit)
    {
        await _publisher.PublishAsync(new OperationEventDto
        {
            Operation = operation,
            IsSuccess = success,
            ErrorMessage = error,
            Category = category,
            Unit = unit,
            TimestampUtc = DateTime.UtcNow
        });
    }

    private static QuantityDTO ToDomain(ApiQuantityDto dto)
    {
        return new QuantityDTO(dto.Value ?? 0, dto.Unit, dto.Category);
    }

    private static string BuildBinaryDescription(QuantityDTO first, QuantityDTO second)
    {
        return $"FIRST={Describe(first)}|SECOND={Describe(second)}";
    }

    private static string Describe(QuantityDTO dto)
    {
        return $"VAL={dto.Value},UNIT={dto.Unit},CAT={dto.Category}";
    }
}
