using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qBI.Areas.Identity;
using qBIPro.Data;
using qBIPro.Models;

namespace qBI.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _userContext;
        private readonly AppUserManager _appUserManager;
        //private IHostingEnvironment _hostingenvironment;
        //IHostingEnvironment _env;
        public CustomersController(ApplicationDbContext context, AppUserManager appUserManager)
        {
            _userContext = context;
            _appUserManager = appUserManager;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var user = await _appUserManager.GetUserAsync(User);

            var viewModel = from o in _userContext.Customers
                            join o2 in _userContext.Addresses 
                            on o.CustomerId equals o2.CustomerId
                            where o.CustomerId.Equals(o2.CustomerId)
                            where o.AreaId.Equals(user.AreaId)
                            select new CustomerDetails { Customer = o, Addresses = o2 };

            return View(viewModel);
          //  return View(_userContext.Customers.ToList());
        }

        public IActionResult Create(CustomerDetails customerDetails)
        {

            return View(customerDetails);


        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDetails customerDetails)
        {
            if(ModelState.IsValid)
            {
                    var user = await  _appUserManager.GetUserAsync(User);
                    customerDetails.Customer.AreaId = user.AreaId;
                    await _userContext.AddAsync(customerDetails.Customer);
                    customerDetails.Addresses.CustomerId = customerDetails.Customer.CustomerId;
                    customerDetails.Addresses.AreaId = user.AreaId;
                    await _userContext.AddAsync(customerDetails.Addresses);
                    await _userContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CustomerArea")]
        public async Task<IActionResult> Details(int Id)
        {
            CustomerDetails customerDetails = await getCustomerDetails(Id);

            return View(customerDetails);


        }

        [Authorize(Policy = "CustomerArea")]
        public async Task<IActionResult> Edit(int Id)
        {
            CustomerDetails customerDetails = await getCustomerDetails(Id);

            return View(customerDetails);
        }
        private async Task<CustomerDetails> getCustomerDetails(int Id)
        {
            var customer = await _userContext.Customers.FindAsync(Id);
            CustomerDetails customerDetails = new CustomerDetails();
            customerDetails.Customer = customer;
            string query = "SELECT * FROM Addresses WHERE CustomerId = {0}";
            customerDetails.Addresses = await _userContext.Addresses
                .FromSql(query, customer.CustomerId)
                .SingleOrDefaultAsync();
            return customerDetails;
        }

        public async Task<IActionResult> Save(CustomerDetails customerDetails)
        {


            if (ModelState.IsValid)
            {
                _userContext.Update(customerDetails.Customer);
                _userContext.Update(customerDetails.Addresses);
                await _userContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));


        }

        [Authorize(Policy = "CustomerArea")]
        public async Task<IActionResult> Delete(int Id)
        {

            var customer = await _userContext.Customers.FindAsync(Id);
            if (ModelState.IsValid)
            {
                _userContext.Remove(customer);
                await _userContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));


        }


    }
}
