using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TambayPortal.Data.Models;

namespace TambayPortal.Data.Services
{
    public static class Linux
    {
        /// <summary>
        /// Load IP Tables
        /// </summary>
        public static void LoadIPTables()
        {
            var psi = new ProcessStartInfo("/sbin/iptables", "-L -vt nat");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Start Network Traffic Shaping
        /// </summary>
        public static void StartTrafficShaping()
        {
            var psi = new ProcessStartInfo("/sbin/tc", $"qdisc add dev {Config.PublicInterface} root handle 1: htb");            
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Add Traffic Class Rate
        /// </summary>
        /// <param name="rc"></param>
        public static void AddTrafficClassRate(RateClass rc)
        {
            var psi = new ProcessStartInfo("/sbin/tc", $"class add dev {Config.PublicInterface} parent 1: classid 1:{rc.ClassID} htb rate {rc.NetworkRate}");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Add Traffic Shaping for an IP Address
        /// </summary>
        /// <param name="rc">Rate Class to use</param>
        /// <param name="IPAddress">Client device IP Address</param>
        public static void AddIPTrafficLimit(RateClass rc, string IPAddress)
        {
            var psi = new ProcessStartInfo("/sbin/tc", $"filter add dev {Config.PublicInterface} parent 1:0 protocol ip prio 1 u32 match ip dst {IPAddress} flowid 1:{rc.ClassID}");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Stop traffic shaping
        /// </summary>
        public static void StopTrafficShaping()
        {
            var psi = new ProcessStartInfo("/sbin/tc", $"qdisc del dev {Config.PublicInterface} root");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Reboot device
        /// </summary>
        public static void Reboot()
        {
            var psi = new ProcessStartInfo("/sbin/reboot");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Stop forwarding packets
        /// </summary>
        /// <param name="rc"></param>
        public static void DropForwardPackets()
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-A FORWARD -i {Config.PublicInterface} -o {Config.ISPInterface} -j DROP");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Enable post routing masquerade to isp interface
        /// </summary>
        public static void PostMasquerage()
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-t nat -A POSTROUTING -o {Config.ISPInterface} -j MASQUERADE");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Redirect all traffic to local server
        /// </summary>
        public static void RedirectAllToLocalServer()
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-t nat -I PREROUTING -s {Config.PublicNetworkAddress} -p tcp --dport 1:65535 -j DNAT --to-destination {Config.DeviceNetworkAddress}:80");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Get MAC address based on IP Address
        /// </summary>
        /// <param name="IPAddress">IP Address of the client device</param>
        /// <returns></returns>
        public static async Task<string> GetMACAddress(string IPAddress)
        {
            var psi = new ProcessStartInfo("/bin/ip", "neighbor");
            var proc = Process.Start(psi);
            proc.WaitForExit();
            var output = await proc.StandardOutput.ReadToEndAsync();
            var lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var l in lines)
            {
                var tok = l.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (tok[0] == IPAddress)
                {
                    return tok[4];
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Exempt IP Address from traffic shaping
        /// </summary>
        /// <param name="IPAddress"></param>
        public static void ExemptIPRoute(string IPAddress)
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-t nat -I PREROUTING -p tcp -s {IPAddress} -j RETURN");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Remove exemption of IP Address
        /// </summary>
        /// <param name="IPAddress"></param>
        public static void RemoveExemptIPRoute(string IPAddress)
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-t nat -D PREROUTING -p tcp -s {IPAddress} -j RETURN");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Allow mac address to forward traffic to isp
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="MACAddress"></param>
        public static void AllowForwardMAC(RateClass rc, string MACAddress)
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-I FORWARD -i {Config.PublicInterface} -o {Config.ISPInterface} -m mac --mac-source {MACAddress} -j ACCEPT");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
        /// <summary>
        /// Remove mac address from forwarding traffic to isp
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="MACAddress"></param>
        public static void RemoveForwardMAC(RateClass rc, string MACAddress)
        {
            var psi = new ProcessStartInfo("/sbin/iptables", $"-D FORWARD -i {Config.PublicInterface} -o {Config.ISPInterface} -m mac --mac-source {MACAddress} -j ACCEPT");
            psi.CreateNoWindow = true;
            Process.Start(psi).WaitForExit();
        }
    }
}
