using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using network_traffic.net;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using network_traffic.Views;
using network_traffic.Models;
using PacketDotNet;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace network_traffic.ViewModels
{
    public class DeviceInfoViewModel
    {
        private static ObservableCollection<DateTimePoint> _devices = new();
        private static ObservableCollection<DateTimePoint> _packets = new();
        private static ObservableCollection<DateTimePoint> _tcpPackets = new();
        private static ObservableCollection<DateTimePoint> _udpPackets = new();
        private static ObservableCollection<DateTimePoint> _IPv4Packets = new();
        private static ObservableCollection<DateTimePoint> _IPv6Packets = new();
        public ObservableCollection<ISeries> Series { get; set; }
        private static Timer Timer = new(AddItemCallback, null, 1000, 0);

        public SolidColorPaint LedgendBackgroundPaint { get; set; } =
            new SolidColorPaint(new SKColor(240, 240, 240));

        public SolidColorPaint LegendTextPaint { get; set; } =
            new SolidColorPaint
            {
                Color = new SKColor(50, 50, 50),
                SKTypeface = SKTypeface.FromFamilyName("Courier New")
            };

        public DeviceInfoViewModel()
        {
            Series = new ObservableCollection<ISeries>
            {
                 new LineSeries<DateTimePoint>
                 {
                      Name = "Devices",
                      Values = _devices,
                      Fill = null,
                      GeometryFill = null,
                      GeometryStroke = null,
                 },
                 new LineSeries<DateTimePoint>
                 {
                      Name = "All Packets",
                      Values = _packets,
                      Fill = null,
                      GeometryFill = null,
                      GeometryStroke = null,
                 },
                 new LineSeries<DateTimePoint>
                 {
                     Name = "Tcp Packets",
                     Values = _tcpPackets,
                     Fill = null,
                     GeometryFill = null,
                     GeometryStroke = null,
                 },
                 new LineSeries<DateTimePoint>
                 {
                     Name = "Udp Packets",
                     Values = _udpPackets,
                     Fill = null,
                     GeometryFill = null,
                     GeometryStroke = null,
                 },
                 new LineSeries<DateTimePoint>
                 {
                     Name = "IPv4 Packets",
                     Values = _IPv4Packets,
                     Fill = null,
                     GeometryFill = null,
                     GeometryStroke = null,
                 },
                 new LineSeries<DateTimePoint>
                 {
                     Name = "IPv6 Packets",
                     Values = _IPv6Packets,
                     Fill = null,
                     GeometryFill = null,
                     GeometryStroke = null,
                 }
            };
        }

        private static void AddItemCallback(Object o)
        {
            Timer.Change(1000, 0);
            var devices = TrafficAnalyzer.CaptureDevices();
            var packets = TrafficAnalyzer.GetPacketsInfo();
            var tcpPackets = TrafficAnalyzer.GetPacketsInfo(ProtocolType.Tcp);
            var udpPackets = TrafficAnalyzer.GetPacketsInfo(ProtocolType.Udp);
            var IPv4Packets = TrafficAnalyzer.GetPacketsInfo(RawIPPacketProtocol.IPv4);
            var IPv6Packets = TrafficAnalyzer.GetPacketsInfo(RawIPPacketProtocol.IPv6);
            _devices.Add(new DateTimePoint(DateTime.Now, devices.Count));
            _packets.Add(new DateTimePoint(DateTime.Now, packets.Count));
            _tcpPackets.Add(new DateTimePoint(DateTime.Now, tcpPackets.Count));
            _udpPackets.Add(new DateTimePoint(DateTime.Now, udpPackets.Count));
            _IPv4Packets.Add(new DateTimePoint(DateTime.Now, IPv4Packets.Count));
            _IPv6Packets.Add(new DateTimePoint(DateTime.Now, IPv6Packets.Count));
        }

        public Axis[] XAxes { get; set; } =
        {
           new DateTimeAxis(TimeSpan.FromSeconds(1), date => date.ToString("fff") + "s")
        };

        private RelayCommand _showPackets;
        public RelayCommand ShowPackets
        {
            get
            {
                return _showPackets ??
                       (_showPackets = new RelayCommand(obj =>
                       {
                           Packets packetsInfo = new();
                           packetsInfo.DataContext = new PacketsViewModel();
                           packetsInfo.Show();
                       }));
            }
        }
    }
}
