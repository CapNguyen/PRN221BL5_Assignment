using Assignment1.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PRN221BL5_Assignment1Context db;
        private List<string> roomStatusList = new List<string>()
        {
            "Available","Occupied","Not Available"
        };

        public MainWindow()
        {
            InitializeComponent();
            db = new PRN221BL5_Assignment1Context();
            LoadData();

        }

        private void LoadData()
        {
            LoadRoom();
            LoadCustomer();
            LoadBooking();
        }

        private void LoadRoom()
        {
            var rooms = db.Rooms.ToList();
            cbStatus.ItemsSource = roomStatusList;
            DataGridRoom.ItemsSource = rooms;
            cbRoom.ItemsSource = rooms;
            cbRoom.DisplayMemberPath = "RoomType";
            cbRoom.SelectedValuePath = "RoomId";

        }

        private void LoadCustomer()
        {
            var customers = db.Customers.ToList();
            DataGridCustomer.ItemsSource = customers;
            cbCustomer.ItemsSource = customers;
            cbCustomer.DisplayMemberPath = "Name";
            cbCustomer.SelectedValuePath = "CustomerId";

        }

        private void LoadBooking()
        {
            var bookings = db.Bookings
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .ToList();
            DataGridBooking.ItemsSource = bookings;
        }

        private void btnSearchRoom_Click(object sender, RoutedEventArgs e)
        {
            var rooms = db.Rooms
                .Where(r =>
                        (string.IsNullOrEmpty(tbRoomType.Text) || r.RoomType == tbRoomType.Text) &&
                        (cbStatus.SelectedItem == null || r.Status == cbStatus.SelectedItem.ToString()) &&
                        (string.IsNullOrEmpty(tbPrice.Text) || r.Price == Convert.ToDouble(tbPrice.Text)))
                .ToList();
            DataGridRoom.ItemsSource = rooms;
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var room = new Room
                {
                    RoomType = string.IsNullOrEmpty(tbRoomType.Text) ? "Not Available Now" : tbRoomType.Text,
                    Status = tbRoomType.Text.Equals("Not Available Now") ? tbRoomType.Text : (cbStatus.SelectedItem == null ? "Not Available Now" : cbStatus.SelectedItem.ToString()),
                    Price = string.IsNullOrEmpty(tbPrice.Text) ? 0 : Convert.ToDouble(tbPrice.Text),
                };
                var check = checkDuplicateRoom(room);
                if (check)
                {
                    MessageBox.Show("Duplicated Room");
                }
                else
                {
                    db.Rooms.Add(room);
                    db.SaveChanges();
                    MessageBox.Show("Add successfully");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridRoom.SelectedValue != null)
                {
                    var r = DataGridRoom.SelectedValue as Room;
                    if (r != null)
                    {
                        r.RoomType = string.IsNullOrEmpty(tbRoomType.Text) ? "Not Available Now" : tbRoomType.Text;
                        r.Status = tbRoomType.Text.Equals("Not Available Now") ? tbRoomType.Text : (cbStatus.SelectedItem == null ? "Not Available Now" : cbStatus.SelectedItem.ToString());
                        r.Price = string.IsNullOrEmpty(tbPrice.Text) ? 0 : Convert.ToDouble(tbPrice.Text);
                        var check = checkDuplicateRoom(r);
                        if (check)
                        {
                            MessageBox.Show("Duplicated Room");
                        }
                        else
                        {
                            db.Rooms.Update(r);
                            db.SaveChanges();
                            MessageBox.Show("Successfully update room with RoomID = " + r.RoomId);
                            LoadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridRoom.SelectedValue != null)
                {
                    var r = DataGridRoom.SelectedValue as Room;
                    if (r != null)
                    {
                        var bookings = db.Bookings.Where(b => b.RoomId == r.RoomId).ToList();
                        db.Bookings.RemoveRange(bookings);
                        db.Rooms.Remove(r);
                        db.SaveChanges();
                        MessageBox.Show("Successfully delete room with RoomID = " + r.RoomId);
                        ClearRoom();
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnClearRoom_Click(object sender, RoutedEventArgs e)
        {
            ClearRoom();
        }

        private void ClearRoom()
        {
            tbRoomType.Clear();
            cbStatus.SelectedItem = null;
            tbPrice.Clear();
            LoadData();
        }

        private void btnSearchCus_Click(object sender, RoutedEventArgs e)
        {
            var customers = db.Customers
                .Where(c =>
                        (string.IsNullOrEmpty(tbName.Text) || c.Name == tbName.Text) &&
                        (string.IsNullOrEmpty(tbPhoneNumber.Text) || c.PhoneNumber == tbPhoneNumber.Text) &&
                        (string.IsNullOrEmpty(tbEmail.Text) || c.Email == tbEmail.Text) &&
                        (string.IsNullOrEmpty(tbAddress.Text) || c.Address == tbAddress.Text))
                        .ToList();
            DataGridCustomer.ItemsSource = customers;
        }

        private void btnAddCus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = new Customer
                {
                    Name = tbName.Text,
                    PhoneNumber = tbPhoneNumber.Text,
                    Email = tbEmail.Text,
                    Address = tbAddress.Text,
                };
                bool check = checkDuplicateCustomer(c);
                if (check)
                {
                    MessageBox.Show("Duplicate Customer");
                }
                db.Customers.Add(c);
                db.SaveChanges();
                MessageBox.Show("Successfully Add New Customer");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateCus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridCustomer.SelectedValue != null)
                {
                    var c = DataGridCustomer.SelectedValue as Customer;
                    if (c != null)
                    {
                        c.Name = tbName.Text;
                        c.PhoneNumber = tbPhoneNumber.Text;
                        c.Address = tbAddress.Text;
                        c.Email = tbEmail.Text;
                        var check = checkDuplicateCustomer(c);
                        if (check)
                        {
                            MessageBox.Show("Duplicated Customer");
                        }
                        else
                        {
                            db.Customers.Update(c);
                            db.SaveChanges();
                            MessageBox.Show("Successfully update room with CustomerID = " + c.CustomerId);
                            LoadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteCus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridCustomer.SelectedValue != null)
                {
                    var c = DataGridCustomer.SelectedValue as Customer;
                    if (c != null)
                    {
                        var bookings = db.Bookings.Where(b => b.CustomerId == c.CustomerId).ToList();
                        db.Bookings.RemoveRange(bookings);
                        db.Customers.Remove(c);

                        db.SaveChanges();
                        MessageBox.Show("Successfully delete customer with CustomerID = " + c.CustomerId);
                        ClearCustomer();
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnClearCus_Click(object sender, RoutedEventArgs e)
        {
            ClearCustomer();
        }

        private void ClearCustomer()
        {
            tbName.Clear();
            tbPhoneNumber.Clear();
            tbEmail.Clear();
            tbAddress.Clear();
            LoadCustomer();
        }

        private void btnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            var r = cbRoom.SelectedItem as Room ?? null;
            var c = cbCustomer.SelectedItem as Customer ?? null;
            var inDate = checkinDate.SelectedDate ?? null;
            var outDate = checkoutDate.SelectedDate ?? null;
            try
            {

                var numOfDates = GetDatesBetween(inDate, outDate).Count;
                var b = new Booking
                {
                    Room = r,
                    Customer = c,
                    RoomId = r!.RoomId,
                    CustomerId = c!.CustomerId,
                    CheckInDate = checkinDate.SelectedDate,
                    CheckOutDate = checkoutDate.SelectedDate,
                    TotalPrice = r.Price * numOfDates,
                };
                var check = ValidationBooking(b);
                if (check)
                {
                    db.Bookings.Add(b);
                    db.SaveChanges();
                    MessageBox.Show("Successfully Add");
                    LoadData();
                }
                else
                {

                    MessageBox.Show("Booking not valid!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridBooking.SelectedValue != null)
                {
                    var r = cbRoom.SelectedItem as Room;
                    var c = cbCustomer.SelectedItem as Customer;
                    var inDate = checkinDate.SelectedDate;
                    var outDate = checkoutDate.SelectedDate;
                    var numOfDates = GetDatesBetween(inDate, outDate).Count;
                    var b = DataGridBooking.SelectedValue as Booking;
                    if (b != null)
                    {
                        b.Room = r;
                        b.Customer = c;
                        b.RoomId = r!.RoomId;
                        b.CustomerId = c!.CustomerId;
                        b.CheckInDate = inDate;
                        b.CheckOutDate = outDate;
                        b.TotalPrice = r.Price * numOfDates;
                    }
                    var check = ValidationBooking(b);
                    if (check)
                    {
                        db.Bookings.Update(b);
                        db.SaveChanges();
                        MessageBox.Show("Successfully Update");
                        LoadData();
                    }
                    else
                    {

                        MessageBox.Show("Booking not valid!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridBooking.SelectedValue != null)
                {
                    var b = DataGridBooking.SelectedValue as Booking;
                    if (b != null)
                    {
                        db.Bookings.Remove(b);
                        db.SaveChanges();
                        MessageBox.Show("Successfully delete booking with BookingID = " + b.BookingId);
                        ClearBooking();
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnClearBooking_Click(object sender, RoutedEventArgs e)
        {
            ClearBooking();
        }

        private void ClearBooking()
        {
            cbCustomer.SelectedItem = null;
            cbRoom.SelectedItem = null;
            checkinDate.SelectedDate = null;
            checkoutDate.SelectedDate = null;
        }

        private void btnSearchBooking_Click(object sender, RoutedEventArgs e)
        {
            if (checkinDate.SelectedDate > checkoutDate.SelectedDate)
            {
                MessageBox.Show("Check-In Date should be less than Check-out Date");
            }
            else
            {
                var bookings = db.Bookings
                    .Where(b =>
                            (cbRoom.SelectedItem == null || b.RoomId == (int)cbRoom.SelectedValue) &&
                            (cbCustomer.SelectedItem == null || b.CustomerId == (int)cbCustomer.SelectedValue) &&
                            (checkinDate.SelectedDate == null || b.CheckInDate >= checkinDate.SelectedDate) &&
                            (checkoutDate.SelectedDate == null || b.CheckOutDate <= checkoutDate.SelectedDate))
                            .ToList();
                DataGridBooking.ItemsSource = bookings;
            }
        }
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            var report = new BookingReport(db);
            report.Show();
        }

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            var analysis = new CustomerAndRoomSummary(db);
            analysis.Show();
        }


        private void DataGridRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridRoom.SelectedValue != null)
            {
                var selectedRoom = DataGridRoom.SelectedValue as Room;
                if (selectedRoom != null)
                {
                    tbRoomId.Text = selectedRoom.RoomId.ToString();
                    tbRoomType.Text = selectedRoom.RoomType;
                    cbStatus.SelectedItem = selectedRoom.Status;
                    tbPrice.Text = selectedRoom.Price.ToString();
                }
            }
        }

        private void DataGridCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridCustomer.SelectedValue != null)
            {
                var selectedCustomer = DataGridCustomer.SelectedValue as Customer;
                if (selectedCustomer != null)
                {
                    tbCustomerId.Text = selectedCustomer.CustomerId.ToString();
                    tbName.Text = selectedCustomer.Name;
                    tbPhoneNumber.Text = selectedCustomer.PhoneNumber;
                    tbEmail.Text = selectedCustomer.Email;
                    tbAddress.Text = selectedCustomer.Address;
                }
            }
        }

        private void DataGridBooking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridBooking.SelectedValue != null)
            {
                var selectedBooking = DataGridBooking.SelectedValue as Booking;
                if (selectedBooking != null)
                {
                    tbBookingId.Text = selectedBooking.BookingId.ToString();
                    cbRoom.SelectedItem = selectedBooking.Room;
                    cbCustomer.SelectedItem = selectedBooking.Customer;
                    checkinDate.SelectedDate = selectedBooking.CheckInDate;
                    checkoutDate.SelectedDate = selectedBooking.CheckOutDate;
                }
            }
        }
        private bool checkDuplicateRoom(Room room)
        {
            var check = db.Rooms.Any(r => r.RoomType == room.RoomType);
            return check;
        }
        private bool checkDuplicateCustomer(Customer customer)
        {
            var check = db.Customers.Any(c => (c.PhoneNumber == customer.PhoneNumber || c.Email == customer.PhoneNumber));
            return check;
        }
        private bool ValidationBooking(Booking booking)
        {
            var all = db.Bookings.ToList();
            var check = true;
            var checkInput = (cbCustomer.SelectedItem != null || cbRoom.SelectedItem != null || checkinDate.SelectedDate != null || checkoutDate.SelectedDate != null);
            var checkRoomValid = booking.Room!.Status == "Available";
            var checkDateValid = checkinDate.SelectedDate <= checkoutDate.SelectedDate;
            var checkValidBooking = false;
            if (checkDateValid)
            {
                var bookingDateRange = GetDatesBetween(booking.CheckInDate, booking.CheckOutDate);
                foreach (var b in all)
                {
                    if (booking.BookingId == b.BookingId)
                    {
                        continue;
                    }
                    var itemDateRange = GetDatesBetween(b.CheckInDate, b.CheckOutDate);
                    var listIntersectDate = bookingDateRange.Intersect(itemDateRange);
                    checkValidBooking = (listIntersectDate.Count() == 0) ? true : (b.Room != booking.Room);
                    if (checkValidBooking == false)
                    {
                        return check = false;
                    }
                }
            }

            return check = checkInput || checkDateValid || checkValidBooking;
        }
        private List<DateTime> GetDatesBetween(DateTime? startDate, DateTime? endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            if (startDate == null || endDate == null)
            {
                return dates;
            }
            DateTime start = startDate.Value;
            DateTime end = endDate.Value;
            if (start > end)
            {
                return dates;
            }
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates;
        }

    }
}