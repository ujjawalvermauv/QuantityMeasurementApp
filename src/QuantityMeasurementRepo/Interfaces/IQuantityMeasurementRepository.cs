using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.QuantityMeasurementRepo.Models;

namespace QuantityMeasurementApp.QuantityMeasurementRepo.Interfaces
{
    /// <summary>
    /// The IQuantityMeasurementRepository serves as the data access layer for the application,
    /// abstracting the implementation details either in-memory caching or database interactions,
    /// and providing a clean interface for managing quantity measurement data.
    /// </summary>
    public interface IQuantityMeasurementRepository
    {
        /// <summary>
        /// Saves a QuantityMeasurementEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        void Save(QuantityMeasurementEntity entity);

        /// <summary>
        /// Retrieves all measurement entities from the repository.
        /// </summary>
        /// <returns>List of all entities</returns>
        List<QuantityMeasurementEntity> GetAllMeasurements();
    }
}