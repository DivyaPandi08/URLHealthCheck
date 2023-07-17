using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;

namespace UrlHealthCheckApp
{
    public partial class MainWindow : Window
    {
        public List<ApplicationData> ApplicationList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<ApplicationData> applications = new List<ApplicationData>()
            {
                new ApplicationData { Name = "Application 1", Url = "https://example.com" },
                new ApplicationData { Name = "Application 2", Url = "https://google.com" },
                new ApplicationData { Name = "Application 3", Url = "https://openai.com" }
            };

            ApplicationList = CheckApplicationsHealth(applications);
            urlDataGrid.ItemsSource = ApplicationList;
        }

        private void Button_CheckHealth_Click(object sender, RoutedEventArgs e)
        {
            ApplicationList = CheckApplicationsHealth(ApplicationList);
        }

        public List<ApplicationData> CheckApplicationsHealth(List<ApplicationData> applications)
        {
            foreach (var app in applications)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(app.Url);
                    request.Method = "HEAD";

                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        app.Status = response.StatusCode == HttpStatusCode.OK ? "Healthy" : "Not Healthy";
                    }
                }
                catch (WebException)
                {
                    app.Status = "Not Healthy";
                }
            }

            return applications;
        }

        private void urlDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    public class ApplicationData : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; NotifyPropertyChanged(nameof(Name)); }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; NotifyPropertyChanged(nameof(Url)); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(nameof(Status)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
