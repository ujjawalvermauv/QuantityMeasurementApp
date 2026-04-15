using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/history")]
public class HistoryController : ControllerBase
{
    private readonly IQuantityMeasurementRepository _repository;

    public HistoryController(IQuantityMeasurementRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize]
    public ActionResult<IEnumerable<OperationHistory>> GetHistory()
    {
        var history = _repository
            .GetAll()
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => new OperationHistory
            {
                Id = item.Id.ToString(),
                Type = item.Type,
                Operation = item.Operation,
                Input = item.Input,
                Result = item.Result,
                CreatedAt = item.CreatedAt
            })
            .ToList();

        return Ok(history);
    }
}