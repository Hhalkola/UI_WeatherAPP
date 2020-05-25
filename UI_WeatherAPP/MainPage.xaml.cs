using System;
using System.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows;
using Windows.UI.Popups;
using System.Collections.ObjectModel;


namespace UI_WeatherAPP
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GetTop5();
            GetDataForAverages();

        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DpStarting.Date > DpUntil.Date)
            {
                MessageDialog ms = new MessageDialog("Starting date can't be bigger than ending date");
                _ = ms.ShowAsync();
            }
            if (SliderTempMin.Value > SliderTempMax.Value)
            {
                MessageDialog ms = new MessageDialog("Minimum temperature can't be bigger than maximum value");
                _ = ms.ShowAsync();
            }
            RefreshGrid();
        }

        private void GetTop5() //Call this on startup to setup search window to show 5 newest 
        {
            string sql = "select top 10 id,temperature,date,humidity,pressure from weather order by id desc";
            DataTable dt = new DataTable();

            Sql.FillDT(dt, sql);

            ObservableCollection<Weather> obj = new ObservableCollection<Weather>();
            foreach (DataRow row in dt.Rows)
            {
                var col = new Weather()
                {
                    Temperature = (decimal)row["temperature"],
                    Humidity = (decimal)row["humidity"],
                    Pressure = (decimal)row["pressure"],
                    Date = (DateTime)row["date"]
                };
                obj.Add(col);
            }
            GrdWeather.ItemsSource = obj;
        }
        
        private void RefreshGrid() //Refresh search grid
        {
            string d1 = "";
            string d2 = "";

            if (DpStarting.Date != null)
            {
                d1 = DpStarting.Date.Value.ToString("yyyy-MM-dd");
            }

            if (DpUntil.Date != null)
            {
                d2 = DpUntil.Date.Value.ToString("yyyy-MM-dd"); 
            }

            DataTable dt = new DataTable();
            dt = Sql.FillDT(dt, Sql.Query(d1, d2, SliderTempMin.Value.ToString(), SliderTempMax.Value.ToString()));

            ObservableCollection<Weather> obj = new ObservableCollection<Weather>();
            foreach (DataRow row in dt.Rows)
            {
                var col = new Weather()
                {
                    Temperature = (decimal)row["temperature"],
                    Humidity = (decimal)row["humidity"],
                    Pressure = (decimal)row["pressure"],
                    Date = (DateTime)row["date"]
                };
                obj.Add(col);
            }
            GrdWeather.ItemsSource = obj;
        }

        private void TglStartDate_Click(object sender, RoutedEventArgs e)
        {
            DpStarting.Date = null;
        }

        private void TglEndDate_Click(object sender, RoutedEventArgs e)
        {
            DpUntil.Date = null;
        }

        private void GetDataForAverages() //Setup gridview for all time averages
        {
            DataTable dt = new DataTable();
            dt = Sql.FillDT(dt, "SELECT * FROM weather");
            decimal avgtemp = 0;
            decimal avghum = 0;
            decimal avgpres = 0;
            int index;

            foreach (DataRow item in dt.Rows)
            {
                index = dt.Rows.IndexOf(item);
                avgtemp += Convert.ToDecimal( dt.Rows[index]["temperature"]);
                avghum += Convert.ToDecimal(dt.Rows[index]["humidity"]);
                avgpres += Convert.ToDecimal(dt.Rows[index]["pressure"]);
                index++;
            }
            avgtemp = avgtemp / dt.Rows.Count;
            avghum = avghum / dt.Rows.Count;
            avgpres = avgpres / dt.Rows.Count;

            ObservableCollection<Averageweather> obj = new ObservableCollection<Averageweather>();
            var col = new Averageweather()
            {
                AvgTemperature = avgtemp,
                AvgHumidity = avghum,
                AvgPressure = avgpres,
            };
            obj.Add(col);

            grdview.ItemsSource = obj;
        }
    }
}
