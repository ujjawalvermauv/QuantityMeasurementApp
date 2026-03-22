IF OBJECT_ID('dbo.QuantityMeasurementOperations', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuantityMeasurementOperations
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Description NVARCHAR(1000) NOT NULL,
        IsError BIT NOT NULL,
        ErrorMessage NVARCHAR(1000) NOT NULL,
        CreatedAt DATETIME2 NOT NULL
    );

    CREATE INDEX IX_QuantityMeasurementOperations_CreatedAt
        ON dbo.QuantityMeasurementOperations(CreatedAt DESC);
END;
