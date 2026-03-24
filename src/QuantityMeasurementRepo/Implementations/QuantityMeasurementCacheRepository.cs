using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QuantityMeasurementApp.QuantityMeasurementModel;
using QuantityMeasurementApp.QuantityMeasurementRepo.Models;
using QuantityMeasurementApp.QuantityMeasurementRepo.Interfaces;

namespace QuantityMeasurementApp.QuantityMeasurementRepo.Implementations
{
    /// <summary>
    /// QuantityMeasurementCacheRepository is a repository class for managing persistence operations related to QuantityMeasurementEntity.
    /// This class is a Singleton and implements the IQuantityMeasurementRepository interface.
    /// It provides an in-memory cache for storing QuantityMeasurementEntity objects.
    /// </summary>
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository _instance;
        private static readonly object _lock = new object();
        private readonly List<QuantityMeasurementEntity> _measurements;
        private const string FilePath = "quantity_measurements.json";

        /// <summary>
        /// Private constructor to prevent instantiation.
        /// </summary>
        private QuantityMeasurementCacheRepository()
        {
            _measurements = LoadFromDisk();
        }

        /// <summary>
        /// Gets the singleton instance of the repository.
        /// </summary>
        public static QuantityMeasurementCacheRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new QuantityMeasurementCacheRepository();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Saves a QuantityMeasurementEntity to the repository.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        public void Save(QuantityMeasurementEntity entity)
        {
            _measurements.Add(entity);
            SaveToDisk();
        }

        /// <summary>
        /// Retrieves all measurement entities from the repository.
        /// </summary>
        /// <returns>List of all entities</returns>
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return new List<QuantityMeasurementEntity>(_measurements);
        }

        /// <summary>
        /// Saves the measurements to disk as JSON.
        /// </summary>
        private void SaveToDisk()
        {
            try
            {
                string json = JsonSerializer.Serialize(_measurements, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to disk: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the measurements from disk.
        /// </summary>
        /// <returns>List of measurements</returns>
        private List<QuantityMeasurementEntity> LoadFromDisk()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    return JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(json) ?? new List<QuantityMeasurementEntity>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from disk: {ex.Message}");
            }
            return new List<QuantityMeasurementEntity>();
        }
    }
}