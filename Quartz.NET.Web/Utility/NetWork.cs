using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Quartz.NET.Web.Utility
{
    public class NetWork
    {
        public static IPAddress GetInternalIP()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                foreach (var uni in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return uni.Address;
                    }
                }
            }
            return null;
        }
    }
}
