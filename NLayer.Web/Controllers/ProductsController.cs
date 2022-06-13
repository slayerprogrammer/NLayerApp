﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs.Category;
using NLayer.Core.DTOs.Product;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.MVC.Filters;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _category;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, ICategoryService category, IMapper mapper)
        {
            _service = service;
            _category = category;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _service.GetProductsWithCategory().ConfigureAwait(false);
            return View(product.Data);
        }

        public async Task<IActionResult> Save()
        {
            var categories = await _category.GetAllAsync().ConfigureAwait(false);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto,"Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _category.GetAllAsync().ConfigureAwait(false);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");
            return View();

        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(long id)
        {
            var product = await _service.GetByIdAsync(id).ConfigureAwait(false);

            var categories = await _category.GetAllAsync().ConfigureAwait(false);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name",product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _category.GetAllAsync().ConfigureAwait(false);
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());
            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }

        public async Task<IActionResult> Remove(long id)
        {
            var product = await _service.GetByIdAsync(id).ConfigureAwait(false);

            await _service.RemoveAsync(product).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }
    }
}
