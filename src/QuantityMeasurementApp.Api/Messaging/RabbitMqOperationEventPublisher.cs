using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using QuantityMeasurementApp.Api.Contracts;
using RabbitMQ.Client;

namespace QuantityMeasurementApp.Api.Messaging;

public sealed class RabbitMqOperationEventPublisher : IOperationEventPublisher, IDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqOperationEventPublisher> _logger;

    private IConnection? _connection;
    private IModel? _channel;

    public RabbitMqOperationEventPublisher(
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqOperationEventPublisher> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task PublishAsync(OperationEventDto message, CancellationToken cancellationToken = default)
    {
        try
        {
            EnsureChannel();
            if (_channel == null)
            {
                return Task.CompletedTask;
            }

            var payload = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(payload);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(
                exchange: _options.Exchange,
                routingKey: _options.RoutingKey,
                basicProperties: properties,
                body: body);
        }
        catch (Exception ex)
        {
            // Queue publishing is best-effort so core business flow does not fail if broker is down.
            _logger.LogWarning(ex, "Could not publish RabbitMQ message for operation {Operation}", message.Operation);
        }

        return Task.CompletedTask;
    }

    private void EnsureChannel()
    {
        if (_channel?.IsOpen == true)
        {
            return;
        }

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };

            _connection?.Dispose();
            _channel?.Dispose();

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: _options.Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ connection is unavailable.");
            _channel = null;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
