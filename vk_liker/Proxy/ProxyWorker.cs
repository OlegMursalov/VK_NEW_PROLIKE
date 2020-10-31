using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using vk_liker.DTO;

namespace vk_liker.Proxy
{
    public static class ProxyWorker
    {
        private static bool CanPing(string address)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(address, 2000);
                if (reply == null) return false;
                return reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                return false;
            }
        }

        public static ProxyInfo[] GetAll()
        {
            var proxyInfos = new Collection<ProxyInfo>();

            foreach (var fileName in Directory.GetFiles("Proxy"))
            {
                var lines = File.ReadAllLines(fileName);
                if (lines != null && lines.Count() > 0)
                {
                    foreach (var line in lines)
                    {
                        var arr = line.Split(':');
                        var proxyInfo = new ProxyInfo
                        {
                            Host = arr[0],
                            Port = arr[1],
                            UserName = arr[2],
                            Pass = arr[3],
                        };
                        if (CanPing(proxyInfo.Host))
                        {
                            proxyInfos.Add(proxyInfo);
                        }
                    }
                }
            }

            return proxyInfos.ToArray();



        }
    }
}