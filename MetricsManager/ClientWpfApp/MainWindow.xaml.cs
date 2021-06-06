using System;
using System.Windows;
using MetricsManagerClient.Client.Interfaces;
using MetricsManagerClient.DataLayer.Interfaces;
using MetricsManagerClient.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger _logger;
        
        private readonly IAppModel _appModel;
        private readonly ICpuMetricModel _cpuModel;
        private readonly IDotNetMetricModel _dotNetModel;
        private readonly IHddMetricModel _hddModel;
        private readonly INetworkMetricModel _networkModel;
        private readonly IRamMetricModel _ramModel;
        
        private readonly ICpuMetricsClient _cpuClient;
        private readonly IDotNetMetricsClient _dotnetClient;
        private readonly IHddMetricsClient _hddClient;
        private readonly INetworkMetricsClient _networkClient;
        private readonly IRamMetricsClient _ramClient;

        public MainWindow(ILogger<MainWindow> logger, IServiceProvider provider)
        {
            InitializeComponent();
            _logger = logger;
            _provider = provider;
            _appModel = _provider.GetService<IAppModel>();
            _cpuModel = _provider.GetService<ICpuMetricModel>();
            _dotNetModel = _provider.GetService<IDotNetMetricModel>();
            _hddModel = _provider.GetService<IHddMetricModel>();
            _networkModel = _provider.GetService<INetworkMetricModel>();
            _ramModel = _provider.GetService<IRamMetricModel>();
            
            _cpuClient = _provider.GetService<ICpuMetricsClient>();
            _dotnetClient = _provider.GetService<IDotNetMetricsClient>();
            _hddClient = _provider.GetService<IHddMetricsClient>();
            _networkClient = _provider.GetService<INetworkMetricsClient>();
            _ramClient = _provider.GetService<IRamMetricsClient>();

            if (_cpuModel != null) 
                _cpuModel.OnMetricsValueChange += OnCpuDataChange;
            if (_dotNetModel != null) 
                _dotNetModel.OnMetricsValueChange += OnDotNetDataChange;
            if (_hddModel != null) 
                _hddModel.OnMetricsValueChange += OnHddDataChange;
            if (_networkModel != null) 
                _networkModel.OnMetricsValueChange += OnNetworkDataChange;
            if (_ramModel != null) 
                _ramModel.OnMetricsValueChange += OnRamDataChange;
        }
        
        private void OnCpuDataChange()
        {
            var metrics = _cpuModel.Metrics;

            CpuChart.ColumnSeriesValues[0].Values.Clear();
            
            foreach (var metric in metrics)
            {
                CpuChart.ColumnSeriesValues[0].Values.Add((double)metric.Value);
            }
        }
        
        private void OnDotNetDataChange()
        {
            var metrics = _dotNetModel.Metrics;

            DotNetChart.ColumnSeriesValues[0].Values.Clear();
            
            foreach (var metric in metrics)
            {
                DotNetChart.ColumnSeriesValues[0].Values.Add((double)metric.Value);
            }
        }
        private void OnHddDataChange()
        {
            var metrics = _hddModel.Metrics;

            HddChart.ColumnSeriesValues[0].Values.Clear();
            
            foreach (var metric in metrics)
            {
                HddChart.ColumnSeriesValues[0].Values.Add((double)metric.Value);
            }
        }
        private void OnNetworkDataChange()
        {
            var metrics = _networkModel.Metrics;

            NetworkChart.ColumnSeriesValues[0].Values.Clear();
            
            foreach (var metric in metrics)
            {
                NetworkChart.ColumnSeriesValues[0].Values.Add((double)metric.Value);
            }
        }
        private void OnRamDataChange()
        {
            var metrics = _ramModel.Metrics;

            RamChart.ColumnSeriesValues[0].Values.Clear();
            
            foreach (var metric in metrics)
            {
                RamChart.ColumnSeriesValues[0].Values.Add((double)metric.Value);
            }
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
        }*/

        private void Follow_The_Agent(object sender, RoutedEventArgs e)
        {
            if (FollowAgentCheckBox.IsChecked != null) 
                _appModel.IsFollowAgent = FollowAgentCheckBox.IsChecked.Value;
        }

        private void Get_Metrics(object sender, RoutedEventArgs e)
        {
            _appModel.ManagerBaseAddress = ManagerAddress.Text;
            var cpuMetrics = _cpuClient.GetMetricsFromAllCluster(new GetAllCpuMetricsRequest
            {
                FromTime = _appModel.From,
                ToTime = _appModel.To
            });
            var dotnetMetrics = _dotnetClient.GetMetricsFromAllCluster(new GetAllDotNetMetricsRequest
            {
                FromTime = _appModel.From,
                ToTime = _appModel.To
            });
            var hddMetrics = _hddClient.GetMetricsFromAllCluster(new GetAllHddMetricsRequest
            {
                FromTime = _appModel.From,
                ToTime = _appModel.To
            });
            var networkMetrics = _networkClient.GetMetricsFromAllCluster(new GetAllNetworkMetricsRequest
            {
                FromTime = _appModel.From,
                ToTime = _appModel.To
            });
            var ramMetrics = _ramClient.GetMetricsFromAllCluster(new GetAllRamMetricsRequest
            {
                FromTime = _appModel.From,
                ToTime = _appModel.To
            });
            
            _cpuModel.AddMetrics(cpuMetrics.Metrics);
            _dotNetModel.AddMetrics(dotnetMetrics.Metrics);
            _hddModel.AddMetrics(hddMetrics.Metrics);
            _networkModel.AddMetrics(networkMetrics.Metrics);
            _ramModel.AddMetrics(ramMetrics.Metrics);
        }
        
        private void Calendar_SelectedDateFromChanged(object sender, RoutedEventArgs e)
        {
            var selectedDate = CalendarFrom.SelectedDate;

            if (selectedDate != null)
            {
                _appModel.From = (DateTimeOffset)selectedDate.Value.Date;
            }
        }
        
        private void Calendar_SelectedDateToChanged(object sender, RoutedEventArgs e)
        {
            var selectedDate = CalendarTo.SelectedDate;

            if (selectedDate != null)
            {
                _appModel.To = (DateTimeOffset)selectedDate.Value.Date;
            }
        }
    }
}