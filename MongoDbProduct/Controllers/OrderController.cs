using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDbProduct.Dtos.OrderDtos;
using MongoDbProduct.Services.CustomerServices;
using MongoDbProduct.Services.OrderServices;
using MongoDbProduct.Services.ProductServices;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace MongoDbProduct.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public OrderController(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IActionResult> OrderList()
        {
            var values = await _orderService.GetAllOrderAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            List<SelectListItem> customers = (from x in await _customerService.GetAllCustomerAsync()
                                              select new SelectListItem
                                              {
                                                  Text = x.CustomerName,
                                                  Value = x.CustomerId.ToString()
                                              }).ToList();
            ViewBag.Customer = customers;

            List<SelectListItem> products = (from x in await _productService.GetAllProductAsync()
                                             select new SelectListItem
                                             {
                                                 Text = x.ProductName,
                                                 Value = x.ProductId.ToString()
                                             }).ToList();
            ViewBag.Product = products;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {   
            var product=await _productService.GetByIdProductAsync(createOrderDto.ProductId);
            if (createOrderDto.Piece > product.Stock)
            {
                ModelState.AddModelError("Piece", "Bu üründen " + product.Stock + " adet bulunmaktadır. Daha fazla sipariş veremezsiniz");


                List<SelectListItem> customers = (from x in await _customerService.GetAllCustomerAsync()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CustomerName,
                                                      Value = x.CustomerId.ToString()
                                                  }).ToList();
                ViewBag.Customer = customers;

                List<SelectListItem> products = (from x in await _productService.GetAllProductAsync()
                                                 select new SelectListItem
                                                 {
                                                     Text = x.ProductName,
                                                     Value = x.ProductId.ToString()
                                                 }).ToList();
                ViewBag.Product = products;

                return View(createOrderDto);
            }

            await _orderService.CreateOrderAsync(createOrderDto);
            await _productService.StockUpdateWhenOrderCreated(createOrderDto.ProductId, createOrderDto.Piece);
            
            return RedirectToAction("OrderList");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            _orderService.DeleteOrderAsync(id);
            return RedirectToAction("OrderList", "Order");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(string id)
        {
            List<SelectListItem> customers = (from x in await _customerService.GetAllCustomerAsync()
                                              select new SelectListItem
                                              {
                                                  Text = x.CustomerName,
                                                  Value = x.CustomerId.ToString()
                                              }).ToList();
            ViewBag.Customer = customers;

            List<SelectListItem> products = (from x in await _productService.GetAllProductAsync()
                                             select new SelectListItem
                                             {
                                                 Text = x.ProductName,
                                                 Value = x.ProductId.ToString()
                                             }).ToList();
            ViewBag.Product = products;
         
            var value = await _orderService.GetByIdOrderAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            await _orderService.UpdateOrderAsync(updateOrderDto);
            return RedirectToAction("OrderList", "Order");
        }

        public async Task<IActionResult> DownloadOrderListPdf()
        {
            List<ResultOrderDto> orders = await _orderService.GetAllOrderAsync();

            // PDF dosyasını oluştur
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, ms);
                document.Open();

                document.Add(new Paragraph("Sipariş Listesi"));

                PdfPTable table = new PdfPTable(5);
              
                table.AddCell("Sipariş No");
                table.AddCell("Müşteri Adı");
                table.AddCell("Ürün Adı");
                table.AddCell("Sipariş Adedi");
                table.AddCell("Sipariş Tarihi");
               
                foreach (var order in orders)
                {
                    table.AddCell(order.OrderId.ToString());
                    table.AddCell(order.CustomerName);
                    table.AddCell(order.ProductName);
                    table.AddCell(order.Piece.ToString());
                    table.AddCell(order.OrderDate.ToString("dd.MM.yyyy"));
                  
                }

                document.Add(table);
                document.Close();
                byte[] bytes = ms.ToArray();
                return File(bytes, "application/pdf", "SiparisListesi.pdf");
            }
        }

    }
}