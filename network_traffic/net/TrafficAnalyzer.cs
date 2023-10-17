using network_traffic.Models;
using PacketDotNet;
using SharpPcap;
using SharpPcap.WinDivert;
using System.Collections.Generic;
using System.Linq;

namespace network_traffic.net
{
    public class TrafficAnalyzer
    {

        private static List<Packet> _packets;
        public static ulong WINDIVERT_FLAG_DROP { get; private set; }

        static TrafficAnalyzer()
        {
            _packets = new List<Packet>();
            CapturePackets();
        }

        private static void Device_OnPacketArrival(object s, PacketCapture e)
        {
            var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
            _packets.Add(packet);
        }

        public static CaptureDeviceList CaptureDevices()
        {
            return CaptureDeviceList.Instance;
        }

        private static void CapturePackets()
        {
            var device = new WinDivertDevice();

            device.Flags = WINDIVERT_FLAG_DROP;
            device.OnPacketArrival += Device_OnPacketArrival;
            device.Open();

            device.StartCapture();
        }

        public static List<Packet> GetPackets()
        {
            return _packets;
        }

        public static List<PacketInfo> GetPacketsInfo()
        {
            return PacketInfo.from(_packets);
        }

        public static List<PacketInfo> GetPacketsInfo(ProtocolType protocolType)
        {
            return PacketInfo.from(_packets).Where(packet => packet.SourceProtocol == protocolType).ToList();
        }

        public static List<PacketInfo> GetPacketsInfo(RawIPPacketProtocol protocolType)
        {
            return PacketInfo.from(_packets).Where(packet => packet.DestinationProtocol == protocolType).ToList();
        }
    }
}
