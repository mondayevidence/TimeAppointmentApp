﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace TimeAppointmentApp
{
    public class Library
    {
        public void Show(string content, string title)
        {
            IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();           
        }
        public void New(DatePicker startDate, TimePicker startTime,
                TextBox subject, TextBox location, TextBox details, ComboBox duration, CheckBox allDay)
        {
            startDate.Date = DateTime.Now;
            startTime.Time = DateTime.Now.TimeOfDay;
            subject.Text = string.Empty;
            details.Text = string.Empty;
            duration.SelectedIndex = 0;
            allDay.IsChecked = false;
        }

        public async void Add(object sender, DatePicker startDate, TimePicker startTime,
                TextBox subject, TextBox location, TextBox details, ComboBox duration, CheckBox allDay)
        {
            FrameworkElement element = (FrameworkElement) sender;
            GeneralTransform transform = element.TransformToVisual(null);
            Point point = transform.TransformPoint(new Point());
            Rect rect = new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
            DateTimeOffset date = startDate.Date;
            TimeSpan time = startTime.Time;
            int minutes = int.Parse((string)((ComboBoxItem)duration.SelectedItem).Tag);

            Appointment appointment = new Appointment()
            {
                StartTime = new DateTimeOffset(date.Year, date.Month, date.Day,
                time.Hours, time.Minutes, 0, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)),
                Subject = subject.Text,
                Location = location.Text,
                Details = details.Text,
                Duration = TimeSpan.FromMinutes(minutes),
                AllDay = (bool)allDay.IsChecked
            };
            string id = await AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Placement.Default);

            if (string.IsNullOrEmpty(id))
                Show("Appontment not added", "Appointment App");
            else
                Show(string.Format("Appointment {0} added", id), "Appoitment App");
        }
        public async void Calendar (DatePicker startDate, TimePicker startTime)
        {
            await AppointmentManager.ShowTimeFrameAsync(startDate.Date, startTime.Time);
        }


    }
}
