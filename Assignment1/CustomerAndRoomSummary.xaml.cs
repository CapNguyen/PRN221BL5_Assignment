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
    /// Interaction logic for CustomerAndRoomSummary.xaml
    /// </summary>
    public partial class CustomerAndRoomSummary : Window
    {
        private PRN221BL5_Assignment1Context db;
        public CustomerAndRoomSummary(PRN221BL5_Assignment1Context db)
        {
            InitializeComponent();
            this.db = db;
            LoadData();
        }

        private void LoadData(DateTime? from = null, DateTime? to = null)
        {

            var rawData = db.Bookings
                .Where(b =>
                        (from == null || b.CheckInDate >= from) &&
                        (to == null || b.CheckOutDate <= to))
                .Include(b => b.Room)
                .GroupBy(b => b.Room!.RoomType)
                .Select(a => new AnalysisData
                {
                    RoomType = a.Key,
                    Customers = a.Count()
                })
                .ToList();

            var dataSet = db.Bookings
                .Select(b => b.Room!.RoomType)
                .Distinct()
                .ToList()
                        .Select(rt => new AnalysisData
                        {
                            RoomType = rt,
                            Customers = rawData
                        .Where(r => r.RoomType == rt)
                        .Select(r => r.Customers)
                        .FirstOrDefault()
                        })
                        .ToList();
            List<KeyValuePair<string, int>> data = new();
            foreach (var d in dataSet)
            {
                data.Add(new KeyValuePair<string, int>(d.RoomType, d.Customers));
            }
            ((ColumnSeries)Analysis.Series[0]).ItemsSource = data;
        }

        private void dateSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var from = dateFrom.SelectedDate;
            var to = dateTo.SelectedDate;
            LoadData(from, to);
        }
    }
    public class AnalysisData
    {
        public string RoomType { get; set; }
        public int Customers { get; set; }
    }
}
