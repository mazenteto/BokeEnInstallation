using System;
using BokeEnInstallationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BokeEnInstallationAPI.DBContext;

public class AppDBContext(DbContextOptions Options) : DbContext(Options)
{
    public DbSet<BokeEnInstallationForm> Booking { get; set; }
}
