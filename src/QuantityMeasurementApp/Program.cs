using System;
using QuantityMeasurementApp.Startup;
using QuantityMeasurementApp.UI;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Console application entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main application entry point.
        /// </summary>
        private static void Main()
        {
            try
            {
                var factory = new ServiceFactory();
                var service = factory.CreateService();
                var repository = factory.CreateRepository();

                IConsoleMenu menuApp = new ConsoleMenu(service, repository);
                menuApp.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
            }
        }
    }
}
