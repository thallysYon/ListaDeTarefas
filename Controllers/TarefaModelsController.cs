using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext.Data;
using ListaDeTarefas.Models;
using ListaDeTarefas.Enums;

namespace ListaDeTarefas.Controllers
{
    public class TarefaModelsController : Controller
    {
        private readonly ListaDeTarefasContext _context;

        public TarefaModelsController(ListaDeTarefasContext context)
        {
            _context = context;
        }

        // GET: TarefaModels
        public async Task<IActionResult> Index(
            string searchString,
            string statusFilter,
            string prioridadeFilter,
            string dataCriacaoFilter,
            string dataLimiteFilter,
            int? categoriaFilter,
            int? pageNumber)
        {
            
            ViewData["CurrentFilter"] = searchString ?? "";
            ViewData["CurrentStatus"] = statusFilter ?? "";
            ViewData["CurrentPrioridade"] = prioridadeFilter ?? "";
            ViewData["CurrentDataCriacao"] = dataCriacaoFilter ?? "";
            ViewData["CurrentDataLimite"] = dataLimiteFilter ?? "";
            ViewData["CurrentCategoria"] = categoriaFilter?.ToString() ?? "";

            
            ViewBag.StatusOptions = Enum.GetNames(typeof(SituacaoEnum)).ToList();
            ViewBag.PrioridadeOptions = Enum.GetNames(typeof(PrioridadeEnum)).ToList();
            ViewBag.Categorias = await _context.CategoriaModel.ToListAsync();

            var query = _context.TarefaModel
                        .Include(t => t.Categoria)
                        .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(t =>
                    EF.Functions.Like(t.Titulo, $"%{searchString}%") ||
                    EF.Functions.Like(t.Descricao, $"%{searchString}%") ||
                    EF.Functions.Like(t.Categoria.Nome, $"%{searchString}%") ||
                    EF.Functions.Like(t.Prioridade.ToString(), $"%{searchString}%")
                );
                
            }

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                if (Enum.TryParse<SituacaoEnum>(statusFilter, true, out var situacao))
                {
                    query = query.Where(t => t.Situacao == situacao);
                }
            }

            if (!string.IsNullOrWhiteSpace(prioridadeFilter))
            {
                if (Enum.TryParse<PrioridadeEnum>(prioridadeFilter, true, out var prioridade))
                {
                    query = query.Where(t => t.Prioridade == prioridade);
                }
            }

            if (categoriaFilter.HasValue)
            {
                query = query.Where(t => t.CategoriaId == categoriaFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(dataCriacaoFilter))
            {
                if (DateTime.TryParse(dataCriacaoFilter, out DateTime dataCriacao))
                {
                    query = query.Where(t => t.DataDeCriacao.Date == dataCriacao.Date);
                }
            }

            if (!string.IsNullOrWhiteSpace(dataLimiteFilter))
            {
                if (DateTime.TryParse(dataLimiteFilter, out DateTime dataLimite))
                {
                    query = query.Where(t => t.DataLimite.Date == dataLimite.Date);
                }
            }

            query = query.OrderBy(t => t.DataDeCriacao);

            int pageSize = 10;
            var model = await PaginatedList<TarefaModel>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize);

            return View(model);
        }

        // GET: TarefaModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefaModel = await _context.TarefaModel
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefaModel == null)
            {
                return NotFound();
            }

            return View(tarefaModel);
        }

        // GET: TarefaModels/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaModel, "Id", "Nome");
            return View();
        }

        // POST: TarefaModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,DataDeCriacao,DataLimite,Prioridade,Situacao,CategoriaId")] TarefaModel tarefaModel)
        {

            if (ModelState.IsValid)
            {
                _context.Add(tarefaModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaModel, "Id", "Nome", tarefaModel.CategoriaId);
            return View(tarefaModel);
        }

        // GET: TarefaModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefaModel = await _context.TarefaModel.FindAsync(id);

            if (tarefaModel == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_context.CategoriaModel, "Id", "Nome", tarefaModel.CategoriaId);
            return View(tarefaModel);
        }

        // POST: TarefaModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,DataDeCriacao,DataLimite,Prioridade,Situacao,CategoriaId")] TarefaModel tarefaModel)
        {
            if (id != tarefaModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarefaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaModelExists(tarefaModel.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.CategoriaModel, "Id", "Nome", tarefaModel.CategoriaId);
            return View(tarefaModel);
        }

        // GET: TarefaModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarefaModel = await _context.TarefaModel
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tarefaModel == null)
            {
                return NotFound();
            }

            return View(tarefaModel);
        }

        // POST: TarefaModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefaModel = await _context.TarefaModel.FindAsync(id);
            if (tarefaModel != null)
            {
                _context.TarefaModel.Remove(tarefaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TarefaModelExists(int id)
        {
            return _context.TarefaModel.Any(e => e.Id == id);
        }
    }
}
