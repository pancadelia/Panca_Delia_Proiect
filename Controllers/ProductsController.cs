using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopModel.Data;
using ShopModel.Models;

namespace Panca_Delia_Proiect.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Shop _context;

        public ProductsController(Shop context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["WeightSortParm"] = sortOrder == "Weight" ? "weight_desc" : "Weight";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var products = from b in _context.Products
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    products = products.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    products = products.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(b => b.Price);
                    break;
                case "Weight":
                    products = products.OrderBy(b => b.Weight);
                    break;
                case "weight_desc":
                    products = products.OrderByDescending(b => b.Weight);
                    break;
                default:
                    products = products.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ??1, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
 .Include(s => s.Orders)
 .ThenInclude(e => e.Customer)
 .AsNoTracking()
 .FirstOrDefaultAsync(m => m.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Brand,Price,Weight")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
 {
 if (id == null)
 {
 return NotFound();
 }
 var studentToUpdate = await _context.Products.FirstOrDefaultAsync(s => s.ID == id);
 if (await TryUpdateModelAsync<Product>(
 studentToUpdate,
 "",
 s => s.Brand, s => s.Title, s => s.Price, s => s.Weight))
 {
 try
 {
 await _context.SaveChangesAsync();
 return RedirectToAction(nameof(Index));
 }
 catch (DbUpdateException /* ex */)
 {
 ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists");
 }
 }
 return View(studentToUpdate);
 }
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] ="Delete failed. Try again";
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {

                _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
