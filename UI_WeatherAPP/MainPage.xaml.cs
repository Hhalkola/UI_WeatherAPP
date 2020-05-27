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
            GetAllData();

        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DpStarting.Date > DpUntil.Date)
            {
                MessageDialog ms = new MessageDialog("Starting date can't be later than ending date");
                _ = ms.ShowAsync();
            }
            if (SliderTempMin.Value > SliderTempMax.Value)
            {
                MessageDialog ms = new MessageDialog("Minimum temperature can't be higher than maximum temperature");
                _ = ms.ShowAsync();
            }
            RefreshGrid();
        }

        private void GetTop5() //Call this on startup to setup search window to show 5 newest 
        {
            string sql = "select id,temperature,datevalue,humidity,pressure from weather order by id desc  limit 10";
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
                    Date = (DateTime)row["datevalue"]
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
                    Date = (DateTime)row["datevalue"]
                };
                obj.Add(col);
            }
            GrdWeather.Header = $"Found " + dt.Rows.Count.ToString() + " items";
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

            decimal t = Math.Round(avgtemp, 2);
            decimal h = Math.Round(avghum, 2);
            decimal p = Math.Round(avgpres, 2);


            ObservableCollection<Averageweather> obj = new ObservableCollection<Averageweather>();
            var col = new Averageweather()
            {
                AvgTemperature = t,
                AvgHumidity = h,
                AvgPressure = p,
            };
            obj.Add(col);

            grdview.ItemsSource = obj;
        }

        private void GetAllData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = Sql.FillDT(dt, "SELECT * FROM WEATHER");

                ObservableCollection<Weather> obj = new ObservableCollection<Weather>();
                foreach (DataRow row in dt.Rows)
                {
                    var col = new Weather()
                    {
                        Temperature = (decimal)row["temperature"],
                        Humidity = (decimal)row["humidity"],
                        Pressure = (decimal)row["pressure"],
                        Date = (DateTime)row["datevalue"]
                    };
                    obj.Add(col);
                }
                GrdSearchAll.ItemsSource = obj;
                TbItemsCount.Text = $"There are " + dt.Rows.Count.ToString()+ " items in database" ;
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                _ = ms.ShowAsync();
            }
        }
    }
}
