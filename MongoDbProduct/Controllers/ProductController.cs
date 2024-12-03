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
        public async Task <IActionResult> CreateProduct()
        {
            List<ResultCategoryDto> categoryList= await _categoryService.GetAllCategoryAsync();
            ViewBag.categories=categoryList;
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
    }
}
