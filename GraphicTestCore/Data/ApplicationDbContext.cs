using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using qBI.Models;

namespace qBIPro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Area> Area { get; set; }
    }
    public class Area
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Customer
    {
        public int CustomerId { get; set; }
        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }
        public enum CustomerType 
        {
            [Display(Name = "Legal entity")]
            LegalEntity,
            [Display(Name = "Object")]
            Object,
            [Display(Name = "Person")]
            Person
        }
        public CustomerType Type { get; set; }
        [Display(Name = "Identification number")]
        public string IdentificationNumber { get; set; }
        public List<Address> Addresses { get; set; }
        public int AreaId { get; set; }
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int AreaId { get; set; }
    }
}
