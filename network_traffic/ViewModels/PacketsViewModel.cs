using System.Collections.Generic;
using MaterialDesignThemes.Wpf;
using network_traffic.net;
using network_traffic.Views;
using PacketDotNet;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Documents;
using network_traffic.Models;

namespace network_traffic.ViewModels
{
    public class PacketsViewModel
    {
        private static Packets _packetsWindow;

        public PacketsViewModel()
        {
            foreach (var window in System.Windows.Application.Current.Windows)
            {
                if (window is Packets info)
                {
                    _packetsWindow = info;
                }
            }
           
            if (_packetsWindow.dataGrid1 != null)
            {
                _packetsWindow.dataGrid1.ItemsSource = TrafficAnalyzer.GetPacketsInfo();
            }

            _packetsWindow.Show();
        }

        private RelayCommand _refreshPackets;
        public RelayCommand RefreshPackets
        {
            get
            {
                return _refreshPackets ??
                       (_refreshPackets = new RelayCommand(obj =>
                       {
                           _packetsWindow.dataGrid1.ItemsSource = TrafficAnalyzer.GetPacketsInfo();
                       }));
            }
        }
    }
}
