using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext.Data;
using ListaDeTarefas.Models;

namespace ListaDeTarefas.Controllers
{
    public class CategoriaModelsController : Controller
    {
        private readonly ListaDeTarefasContext _context;

        public CategoriaModelsController(ListaDeTarefasContext context)
        {
            _context = context;
        }

        // GET: CategoriaModels
        public async Task<IActionResult> Index(
            string searchString,
            string sortOrder,
            int? pageNumber)
        {
            
            ViewData["CurrentFilter"] = searchString ?? "";
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescSortParm"] = sortOrder == "desc" ? "desc_desc" : "desc";

            var query = _context.CategoriaModel.AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c =>
                    EF.Functions.Like(c.Nome, $"%{searchString}%") ||
                    EF.Functions.Like(c.Descricao, $"%{searchString}%"));
            }

            
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(c => c.Nome),
                "desc" => query.OrderBy(c => c.Descricao),
                "desc_desc" => query.OrderByDescending(c => c.Descricao),
                _ => query.OrderBy(c => c.Nome)
            };

            
            int pageSize = 10;
            var model = await PaginatedList<CategoriaModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(model);
        }

        // GET: CategoriaModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaModel = await _context.CategoriaModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoriaModel == null)
            {
                return NotFound();
            }

            return View(categoriaModel);
        }

        // GET: CategoriaModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao")] CategoriaModel categoriaModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaModel);
        }

        // GET: CategoriaModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaModel = await _context.CategoriaModel.FindAsync(id);
            if (categoriaModel == null)
            {
                return NotFound();
            }
            return View(categoriaModel);
        }

        // POST: CategoriaModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] CategoriaModel categoriaModel)
        {
            if (id != categoriaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaModelExists(categoriaModel.Id))
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
            return View(categoriaModel);
        }

        // GET: CategoriaModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaModel = await _context.CategoriaModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoriaModel == null)
            {
                return NotFound();
            }

            return View(categoriaModel);
        }

        // POST: CategoriaModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriaModel = await _context.CategoriaModel.FindAsync(id);
            if (categoriaModel != null)
            {
                _context.CategoriaModel.Remove(categoriaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaModelExists(int id)
        {
            return _context.CategoriaModel.Any(e => e.Id == id);
        }
    }
}