using System;

namespace BokeEnInstallationAPI.Models;

public class BaseBooking
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string MobileNumber { get; set; }
    public string Discription { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public bool CustomerPay { get; set; }
    public bool Fineished { get; set; }

}
