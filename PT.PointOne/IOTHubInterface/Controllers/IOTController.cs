using IOTHubInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IOTHubInterface.Controllers
{
    public class IOTController : ApiController
    {
        public class IOTParams
        {
            public string customerId { get; set; }
            public bool tappedCompleted { get; set; } 
        }
        private RegistryManager registryManager;
        private string connectionString = "HostName=helgesmebyiot.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=JysiEI6uUIoYgypD4B/XbbZe3S7skmq7adSKhELNCAk=";        
        private DeviceClient deviceClient;
        public Device device;
        private string iotHubUri = "helgesmebyiot.azure-devices.net";
        private string DEVICE_ID = "ESP-12-BEERTAPPER";
        private string deviceKey; 

        [HttpPost]
        public async Task<IOTInfo> PostStatus(IOTParams par)   
        {
            if(!VerifyDeviceKey().Result)
                return new IOTInfo { Text = "ERR-KEY" };

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("ESP-12-BEERTAPPER", deviceKey));
            await SendDeviceToCloudMessagesAsync(par.customerId, par.tappedCompleted);            
            return new IOTInfo { Text = "OK" };
        }

        [HttpGet]
        public async Task<IOTInfo> GetStatus()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            var devices = await registryManager.GetDevicesAsync(5);
            
            await registryManager.RemoveDeviceAsync(devices.First());
           
            

            if (!VerifyDeviceKey().Result)
                return new IOTInfo { Text = "ERR-KEY" };

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("ESP-12-BEERTAPPER", deviceKey));            
            var message = await deviceClient.ReceiveAsync();


            return new IOTInfo { Text = "TEST" };
        }

        private async Task<bool> VerifyDeviceKey()
        {
            if (deviceKey == null)
            {
                await GetDeviceKey();
                deviceKey = device.Authentication.SymmetricKey.PrimaryKey;
                return true;
            }
            if (deviceKey != null || deviceKey.Length > 0)
                return true;

            return false;                 
        }

        private async Task SendDeviceToCloudMessagesAsync(string customerId, bool tappedCompleted)
        {
            var telemetryData = new
            {
                deviceId = DEVICE_ID,
                CustomerId = customerId,
                TappedCompleted = tappedCompleted
            };
            var messageString = JsonConvert.SerializeObject(telemetryData);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));
            await deviceClient.SendEventAsync(message); 
                       
        }

        private async Task GetDeviceKey()
        {
            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                device = await registryManager.AddDeviceAsync(new Device(DEVICE_ID));
            }catch(DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(DEVICE_ID);
            }     catch(Exception ex)
            {
                var y = ex.Message; 
            }       
        }
    }
}
