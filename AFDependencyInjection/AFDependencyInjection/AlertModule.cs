using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AFDependencyInjection
{
    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }

    public interface IAlertService
    {
        void SendAlert(Customer c, string title, string body);
    }

    public class EmailAlertService : IAlertService
    {
        public void SendAlert(Customer c, string title, string body)
        {
            if (string.IsNullOrEmpty(c.Email))
            {
                return;
            }

            Debug.WriteLine($"Sending e-mail to {c.Email} for {c.Name}");
        }
    }

    public class SmsAlertService : IAlertService
    {
        public void SendAlert(Customer c, string title, string body)
        {
            if (string.IsNullOrEmpty(c.Mobile))
            {
                return;
            }

            Debug.WriteLine($"Sending SMS to {c.Mobile} for {c.Name}");
        }
    }
}
