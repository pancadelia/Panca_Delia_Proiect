using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopModel.Data;
using ShopModel.Models;
using Panca_Delia_Proiect.Models.ShopViewModels;

namespace Panca_Delia_Proiect.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly Shop _context;

        public CategoriesController(Shop context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? id, int? productID)
        {
            var viewModel = new CategoryIndexData();
            viewModel.Categories = await _context.Categories
            .Include(i => i.PublishedProducts)
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.CategoryName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["CategoryID"] = id.Value;
                Category category = viewModel.Categories.Where(i => i.ID == id.Value).Single();
                viewModel.Products = category.PublishedProducts.Select(s => s.Product);
            }
            if (productID != null)
            {
                ViewData["ProductID"] = productID.Value;
                viewModel.Orders = viewModel.Products.Where(
                x => x.ID == productID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryName,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories
            .Include(i => i.PublishedProducts).ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }
            PopulatePublishedProductData(category);
            return View(category);

        }
        private void PopulatePublishedProductData(Category category)
        {
            var allProducts = _context.Products;
            var publisherProducts = new HashSet<int>(category.PublishedProducts.Select(c => c.ProductID));
            var viewModel = new List<PublishedProductData>();
            foreach (var product in allProducts)
            {
                viewModel.Add(new PublishedProductData
                {
                    ProductID = product.ID,
                    Title = product.Title,
                    IsPublished = publisherProducts.Contains(product.ID)
                });
            }
            ViewData["Products"] = viewModel;
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(int? id, string[] selectedProducts)
        {
            if (id == null)
            {
                return NotFound();
            }
            var categoryToUpdate = await _context.Categories
            .Include(i => i.PublishedProducts)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Category>(
            categoryToUpdate,
            "",
            i => i.CategoryName, i => i.Description))
            {
                UpdatePublishedProducts(selectedProducts, categoryToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +"Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedProducts(selectedProducts, categoryToUpdate);
            PopulatePublishedProductData(categoryToUpdate);
            return View(categoryToUpdate);
        }
        private void UpdatePublishedProducts(string[] selectedProducts, Category categoryToUpdate)
        {
            if (selectedProducts == null)
            {
                categoryToUpdate.PublishedProducts = new List<PublishedProduct>();
                return;
            }
            var selectedProductsHS = new HashSet<string>(selectedProducts);
            var publishedProducts = new HashSet<int>
            (categoryToUpdate.PublishedProducts.Select(c => c.Product.ID));
            foreach (var product in _context.Products)
            {
                if (selectedProductsHS.Contains(product.ID.ToString()))
                {
                    if (!publishedProducts.Contains(product.ID))
                    {
                        categoryToUpdate.PublishedProducts.Add(new PublishedProduct
                        {
                           CategoryID =categoryToUpdate.ID,
                            ProductID = product.ID
                        });
                    }
                }
                else
                {
                    if (publishedProducts.Contains(product.ID))
                    {
                        PublishedProduct productToRemove = categoryToUpdate.PublishedProducts.FirstOrDefault(i
                       => i.ProductID == product.ID);
                        _context.Remove(productToRemove);
                    }
                }
            }
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
