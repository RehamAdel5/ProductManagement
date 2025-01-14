using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using AdminPanelWithApi.Helpers.Image;
using Domain.Abstractions;
using Newtonsoft.Json;

namespace AdminPanelWithApi.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageHelper _imageHelper;

        public CategoriesController(ICategoryRepository categoryRepository, 
            IWebHostEnvironment webHostEnvironment,
            IImageHelper imageHelper)
        {
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = imageHelper;
        }

        // GET: Categorys
        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepository.ListCategoriesAsync());
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult> GetCategoryProducts(Guid id)
        {
            var products = await _categoryRepository.GetProductsByCategoryIdAsync(id);
            TempData["Products"] = JsonConvert.SerializeObject(products);
            return RedirectToAction("Index", "Products");
        }

        // GET: Categorys/Create
        public IActionResult Create()
        {            
            return View();
        }
       

        // POST: Categorys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Category category, IFormFile? img)
        {
            if (ModelState.IsValid)
            {               
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (img is not null)
                    category.Image = (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2;
                await _categoryRepository.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categorys/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _categoryRepository.GetCategoryAsync(id.Value);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        // POST: Categorys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category,IFormFile? img)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var categoryEntity = await _categoryRepository.GetCategoryAsync(id);
                    if (categoryEntity == null)
                    {
                        return NotFound();
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    category.Image = img is not null ? (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2 : categoryEntity.Image;
                    await _categoryRepository.UpdateCategoryAsync(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categorys/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categorys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);
            if (category != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (!string.IsNullOrEmpty(category.Image))
                {
                    string filePath = Path.Combine(uploadsFolder, category.Image);
                    await _imageHelper.DeleteFile(filePath);
                }
                await _categoryRepository.DeleteCategoryAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(Guid id)
        {
            return _categoryRepository.GetCategoryAsync(id) is not null;
        }        
    }
}
