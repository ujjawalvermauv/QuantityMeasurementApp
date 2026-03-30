using System.Collections.Generic;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly QuantityMeasurementCacheRepository _instance = new();
        private readonly List<QuantityMeasurementEntity> _cache = new();

        public static QuantityMeasurementCacheRepository Instance => _instance;

        private QuantityMeasurementCacheRepository() { }

        public void Save(QuantityMeasurementEntity entity)
        {
            _cache.Add(entity);
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll() => _cache.AsReadOnly();
    }
}
