using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminPanelWithApi.Helpers.Image;
using Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Handlers.Queries.Products.GetProductsList;
using MediatR;
using Application.Handlers.Queries.Products.GetProductsList.GetCategoriesListQuery;
using Application.Handlers.Commands.Products.CreateProduct;
using AutoMapper;
using Application.Handlers.Commands.Products.UpdateProduct;
using Application.Handlers.Queries.Products.GetProductsDetail;
using Application.Handlers.Commands.Products.DeleteProduct;

namespace AdminPanelWithApi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageHelper _imageHelper;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment,
            IImageHelper imageHelper,
            IMediator mediator,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageHelper = imageHelper;
            _mediator = mediator;
            _mapper = mapper;
        }

        // GET: Products
        public async Task<IActionResult> Index(Guid? categoryId)
        {
            var query = new GetProductsListQuery(categoryId);
            var products = await _mediator.Send(query);
            ViewBag.Categories = await _mediator.Send(new GetCategoriesListQuery());
            return View(products);
        }


        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = new SelectList(await _mediator.Send(new GetCategoriesListQuery()), "Id", "TitleEn");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductCommand command, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

                if (img is not null)
                    command.Image = (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2;
                var productId = await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _mediator.Send(new GetProductsDetailQuery(id.Value));
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(await _mediator.Send(new GetCategoriesListQuery()), "Id", "TitleEn");
            return View(_mapper.Map<UpdateProductCommand>(product));
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateProductCommand command, IFormFile? img)
        {
            if (id != command.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var productEntity = await _mediator.Send(new GetProductsDetailQuery(id));
                    if (productEntity == null)
                    {
                        return NotFound();
                    }
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    command.Image = img is not null ? (await _imageHelper.ProcessImageUpload(img, uploadsFolder)).Item2 : productEntity.Image;
                    await _mediator.Send(command);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(command.Id))
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
            return View(command);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _mediator.Send(new GetProductsDetailQuery(id.Value));
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
            var product = await _mediator.Send(new GetProductsDetailQuery(id));
            if (product != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                if (!string.IsNullOrEmpty(product.Image))
                {
                    string filePath = Path.Combine(uploadsFolder, product.Image);
                    await _imageHelper.DeleteFile(filePath);
                }
                await _mediator.Send(new DeleteProductCommand { Id = id });
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _mediator.Send(new GetProductsDetailQuery(id)) is not null;
        }


    }
}
