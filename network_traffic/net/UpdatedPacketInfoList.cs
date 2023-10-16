using network_traffic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace network_traffic.net
{
    class UpdatedPacketInfoList
    {
        private static List<PacketInfo> Packets { get; set; }
        private static readonly Timer Timer = new(PacketsCallback, null, 1000, 0);

        private static void PacketsCallback(object? o)
        {
            Packets = TrafficAnalyzer.GetPacketsInfo();
            Timer.Change(1000, 0);
        }
    }
}
