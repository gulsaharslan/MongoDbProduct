using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MongoDbProduct.Dtos.CategoryDtos;
using MongoDbProduct.Dtos.ProductDtos;
using MongoDbProduct.Services.CategoryServices;
using MongoDbProduct.Services.ProductServices;

namespace MongoDbProduct.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> ProductList()
        {
            var values = await _productService.GetAllProductAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            List<ResultCategoryDto> categoryList = await _categoryService.GetAllCategoryAsync();
            ViewBag.categories = categoryList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await _productService.CreateProductAsync(createProductDto);
            return RedirectToAction("ProductList", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            _productService.DeleteProductAsync(id);
            return RedirectToAction("ProductList", "Product");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(string id)
        {
            List<ResultCategoryDto> categoryList = await _categoryService.GetAllCategoryAsync();
            ViewBag.categories = categoryList;
            var value = await _productService.GetByIdProductAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            await _productService.UpdateProductAsync(updateProductDto);
            return RedirectToAction("ProductList", "Product");
        }

        public async Task<IActionResult> DowloadProductListExcel()
        {
            var products = await _productService.GetAllProductAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ürün Listesi");
                worksheet.Cell(1, 1).Value = "Ürün ID";
                worksheet.Cell(1, 2).Value = "Ürün Adı";
                worksheet.Cell(1, 3).Value = "Kategori Adı";
                worksheet.Cell(1, 4).Value = "Fiyat";
                worksheet.Cell(1, 5).Value = "Stok";

                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cell(row, 1).Value = product.ProductId;
                    worksheet.Cell(row, 2).Value = product.ProductName;
                    worksheet.Cell(row, 3).Value = product.CategoryName;
                    worksheet.Cell(row, 4).Value = product.Price;
                    worksheet.Cell(row, 5).Value = product.Stock;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UrunListesi.xlsx");
                }
            }
        }
    }
}
