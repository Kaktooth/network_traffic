using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Unicode;
using PacketDotNet;

namespace network_traffic.Models
{
    public class PacketInfo
    {
        public IPAddress SourceIpAddress { get; set; }
        public IPAddress DestinationIpAddress { get; set; }
        public String DataHeader { get; set; }
        public String Data { get; set; }
        public String PayloadHeader { get; set; }
        public String Payload { get; set; }
        public ProtocolType SourceProtocol { get; set; }
        public RawIPPacketProtocol DestinationProtocol { get; set; }

        public static List<PacketInfo> from(List<Packet> packets)
        {
            return packets.Select(packet => from(packet)).ToList();
        }

        public static PacketInfo from(Packet packet)
        {
            PacketInfo packetInfo = new PacketInfo();
          
            if (packet.PayloadPacket is IPv4Packet)
            {
                var payload = ((IPv4Packet)packet.PayloadPacket);
                packetInfo.DestinationIpAddress = payload.DestinationAddress;
                packetInfo.SourceIpAddress = payload.SourceAddress;
                packetInfo.SourceProtocol = payload.Protocol;
            }
            else
            {
                var payload = ((IPv6Packet)packet.PayloadPacket);
                packetInfo.DestinationIpAddress = payload.DestinationAddress;
                packetInfo.SourceIpAddress = payload.SourceAddress;
                packetInfo.SourceProtocol = payload.Protocol;
            }

            packetInfo.DataHeader = Encoding.UTF8.GetString(packet.HeaderDataSegment.Bytes);
            packetInfo.Data = Encoding.UTF8.GetString(packet.Bytes);
            packetInfo.PayloadHeader = Encoding.UTF8.GetString(packet.PayloadPacket.HeaderData);
            packetInfo.Payload = Encoding.UTF8.GetString(packet.PayloadPacket.Bytes);
            packetInfo.DestinationProtocol = ((RawIPPacket)packet).Protocol;
            return packetInfo;
        }
    }
}
