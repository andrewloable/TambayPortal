using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace TambayPortal.Data.Models
{
    public static class Config
    {
        /// <summary>
        /// Hash password using https://www.xorbin.com/tools/sha256-hash-calculator
        /// </summary>
        public static string AdminUserPassword { get; set; }
        /// <summary>
        /// GUID of the device for identification
        /// </summary>
        public static string DeviceSerial { get; set; }
        /// <summary>
        /// Device used to connect client network devices. (example /dev/wlan0)
        /// </summary>
        public static string PublicInterface { get; set; }
        /// <summary>
        /// Network address used in device facing network interface. (default: 10.0.0.0/8)
        /// </summary>
        public static string PublicNetworkAddress { get; set; }
        /// <summary>
        /// Device used to connect to Internet/ISP. (example /dev/eth0)
        /// </summary>
        public static string ISPInterface { get; set; }
        /// <summary>
        /// The network address of the device running the server. (default: 10.1.1.1)
        /// </summary>
        public static string DeviceNetworkAddress { get; set; }
        private static string bootConfigFile = "/boot/tambay-portal.json";
        private static string configFile = "config.json";
        /// <summary>
        /// Read configuration file from /boot/tambay-portal.json or in the app folder config.json
        /// </summary>
        public static async void ReadConfig()
        {
            string json = string.Empty;
            if (File.Exists(bootConfigFile))
            {
                json = await File.ReadAllTextAsync(bootConfigFile);
            } 
            else if (File.Exists(configFile))
            {
                json = await File.ReadAllTextAsync(configFile);
            } 
            else
            {
                throw new FileNotFoundException("Config file was not found.");                
            }
            
            if (string.IsNullOrEmpty(json))
            {
                throw new JsonException("Config file is empty.");
            }

            var jobj = JObject.Parse(json);
            AdminUserPassword = jobj["adminUserPassword"].ToString();
            DeviceSerial = jobj["deviceSerial"].ToString();
            PublicInterface = jobj["publicInterface"].ToString();
            PublicNetworkAddress = jobj["publicNetworkAddress"].ToString();
            ISPInterface = jobj["ispInterface"].ToString();
            DeviceNetworkAddress = jobj["deviceNetworkAddress"].ToString();
        }
    }
}
