using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using AdminPanelWithApi.Helpers.Image;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AdminPanelWithApi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageHelper _imageHelper;

        public ProductsController(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IImageHelper imageHelper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = imageHelper;
        }

        // GET: Products
        public async Task<IActionResult> Index(Guid? categoryId) { var products = categoryId.HasValue ? 
                await _productRepository.GetProductsByCategoryId(categoryId.Value) 
                : await _productRepository.ListProducts();
            ViewBag.Categories = await _categoryRepository.GetAllCategories(); 
            return View(products); }


        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = new SelectList(await _categoryRepository.ListCategoriesAsync(), "Id", "TitleEn");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (img is not null)
                    product.Image = (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2;
                await _productRepository.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _productRepository.GetProduct(id.Value);
            if (Product == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(await _categoryRepository.ListCategoriesAsync(), "Id", "TitleEn");
            return View(Product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Product product, IFormFile? img)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productEntity = await _productRepository.GetProduct(id);
                    if (productEntity == null)
                    {
                        return NotFound();
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    product.Image = img is not null ? (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2 : productEntity.Image;
                    await _productRepository.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (!string.IsNullOrEmpty(product.Image))
                {
                    string filePath = Path.Combine(uploadsFolder, product.Image);
                    await _imageHelper.DeleteFile(filePath);
                }
                await _productRepository.DeleteProduct(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _productRepository.GetProduct(id) is not null;
        }


    }
}
