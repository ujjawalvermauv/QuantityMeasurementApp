using QuantityMeasurementApp.QuantityMeasurementBusiness.Services;
using QuantityMeasurementApp.QuantityMeasurementRepo.Implementations;
using QuantityMeasurementApp.QuantityMeasurementUI;
using QuantityMeasurementApp.interfaces;
using ControllerType = QuantityMeasurementApp.QuantityMeasurementController.QuantityMeasurementController;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Entry point. Wires dependencies and starts the console menu.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var repository = QuantityMeasurementCacheRepository.Instance;
            var service = new QuantityMeasurementServiceImpl(repository);
            var controller = new ControllerType(service);
            IApplicationUI applicationUI = new Menu(controller);

            applicationUI.Run();
        }
    }
}
