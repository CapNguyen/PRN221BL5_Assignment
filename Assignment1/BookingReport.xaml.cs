using Assignment1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for BookingReport.xaml
    /// </summary>
    public partial class BookingReport : Window
    {
        private PRN221BL5_Assignment1Context db;
        public BookingReport(PRN221BL5_Assignment1Context db)
        {
            InitializeComponent();
            this.db = db;
            LoadData();
        }

        private void LoadData()
        {
            var dataSet = db.Bookings
            .Include(b => b.Room)
            .GroupBy(b => b.Room!.RoomType)
            .Select(r => new ReportData
            {
                RoomType = r.Key,
                Revenue = r.Sum(b => b.TotalPrice)
            })
            .ToList();
            txtRevenue.Content= dataSet.Sum(r => r.Revenue ?? 0).ToString()+"$";
            List<KeyValuePair<string, double?>> data = new();
            foreach (var d in dataSet)
            {
                data.Add(new KeyValuePair<string, double?>(d.RoomType, d.Revenue));
            }
            ((ColumnSeries)BookingSummary.Series[0]).ItemsSource = data;
        }
    }
    public class ReportData
    {
        public string RoomType { get; set; }
        public double? Revenue { get; set; }
    }
}
