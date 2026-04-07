using System.Collections.Generic;
using System.Linq;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static readonly QuantityMeasurementCacheRepository _instance = new();
        private readonly List<QuantityMeasurementEntity> _cache = new();
        private int _nextId = 1001;

        public static QuantityMeasurementCacheRepository Instance => _instance;

        private QuantityMeasurementCacheRepository() { }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity.Id <= 0)
            {
                entity.AssignId(_nextId++);
            }

            _cache.Add(entity);
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll() => _cache
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }
}
