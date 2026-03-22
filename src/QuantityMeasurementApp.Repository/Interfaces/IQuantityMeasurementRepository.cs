using System.Collections.Generic;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        IEnumerable<QuantityMeasurementEntity> GetAll();
    }
}
