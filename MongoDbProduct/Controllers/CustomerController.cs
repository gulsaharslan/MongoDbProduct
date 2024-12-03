﻿using Microsoft.AspNetCore.Mvc;
using MongoDbProduct.Dtos.CustomerDtos;
using MongoDbProduct.Services.CustomerServices;

namespace MongoDbProduct.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> CustomerList()
        {
            var values = await _customerService.GetAllCustomerAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            await _customerService.CreateCustomerAsync(createCustomerDto);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            _customerService.DeleteCustomerAsync(id);
            return RedirectToAction("CustomerList", "Customer");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(string id)
        {
            var value = await _customerService.GetByIdCustomerAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto updateCustomerDto)
        {
            await _customerService.UpdateCustomerAsync(updateCustomerDto);
            return RedirectToAction("CustomerList", "Customer");
        }
    }
}
