using System;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace devicetwins
{
    class Program
    {

		static string IoTHubConnectionString = "";
		static string DeviceConnectionString = "";
		static string DeviceID="";
		static DeviceClient Client = null;

		static int tempLevel = 30;
		static int coolerOn = 0;

        static async Task Main(string[] args)
        {
            Console.WriteLine("***********************************************");
			Console.WriteLine("Welcome to the Azure IoT Hub Device Twin Tester");
			Console.WriteLine();
			Console.WriteLine("Author: Pete Gallagher");
			Console.WriteLine("Twitter: @pete_codes");
			Console.WriteLine("Date: 16th November 2020");
			Console.WriteLine();
			Console.WriteLine("***********************************************");
			Console.WriteLine();
			
			try
			{

				Console.WriteLine("Enter the IoT Hub Connection String");
				IoTHubConnectionString = Console.ReadLine();

				Console.WriteLine("Enter the Device Connection String");
				DeviceConnectionString = Console.ReadLine();
				
				Console.WriteLine("Enter the Device ID");
				DeviceID = Console.ReadLine();
				
				InitClient();

				GetDeviceTwinAsync().Wait();
                Client.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertyChanged, null).Wait();

				var coolerOnSetting = "";

				while (true)
				{
					Console.WriteLine("Set Cooler Control (0=off, 1=on) (Enter to exit)");
					coolerOnSetting = Console.ReadLine();

					if (coolerOnSetting != null && coolerOnSetting != "")
					{
						coolerOn = Int32.Parse(coolerOnSetting);
						await ReportCoolerControl();
					}
					else
					{
						return;
					}
				}
				
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("Error in sample: {0}", ex.Message);
			}
			
        }

		private static async Task GetDeviceTwinAsync()
        {
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(IoTHubConnectionString);

            try
            {                
                var twin = await registryManager.GetTwinAsync(DeviceID);
				Console.WriteLine(twin.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : {0}", ex);
            }

        }

		public static async void InitClient()
		{
			try
			{
				Console.WriteLine("Connecting to hub");
				Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
				Console.WriteLine("Retrieving twin");
				await Client.GetTwinAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("Error in sample: {0}", ex.Message);
			}
		}

		public static async Task ReportCoolerControl()
		{
			try
			{
				Console.WriteLine("Sending Cooler Control as reported property");

				TwinCollection reportedProperties;
				reportedProperties = new TwinCollection();
				reportedProperties["coolerOn"] = coolerOn;
				await Client.UpdateReportedPropertiesAsync(reportedProperties);
			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("Errors: {0}", ex.Message);
			}
		}

		private static async Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            try
            {
                Console.WriteLine("Desired property change:");
                Console.WriteLine(JsonConvert.SerializeObject(desiredProperties));
				tempLevel = desiredProperties["tempLevel"];
				Console.WriteLine($"Temperature Level is now: {tempLevel}");
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error in sample: {0}", exception);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
        }

    }
}
