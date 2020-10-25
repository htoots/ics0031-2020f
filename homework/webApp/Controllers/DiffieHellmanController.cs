using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using webApp.Data;

namespace webApp.Controllers
{
    public class DiffieHellmanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiffieHellmanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DiffieHellman
        public async Task<IActionResult> Index()
        {
            return View(await _context.DiffieHellmanResults.ToListAsync());
        }

        // GET: DiffieHellman/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diffieHellmanClass = await _context.DiffieHellmanResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diffieHellmanClass == null)
            {
                return NotFound();
            }

            return View(diffieHellmanClass);
        }

        // GET: DiffieHellman/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiffieHellman/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SecretA,SecretB,ModulusP,BaseG")] DiffieHellmanClass diffieHellmanClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diffieHellmanClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diffieHellmanClass);
        }

        // GET: DiffieHellman/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diffieHellmanClass = await _context.DiffieHellmanResults.FindAsync(id);
            if (diffieHellmanClass == null)
            {
                return NotFound();
            }
            return View(diffieHellmanClass);
        }

        // POST: DiffieHellman/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SecretA,SecretB,ModulusP,BaseG")] DiffieHellmanClass diffieHellmanClass)
        {
            if (id != diffieHellmanClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diffieHellmanClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiffieHellmanClassExists(diffieHellmanClass.Id))
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
            return View(diffieHellmanClass);
        }

        // GET: DiffieHellman/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diffieHellmanClass = await _context.DiffieHellmanResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diffieHellmanClass == null)
            {
                return NotFound();
            }

            return View(diffieHellmanClass);
        }

        // POST: DiffieHellman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diffieHellmanClass = await _context.DiffieHellmanResults.FindAsync(id);
            _context.DiffieHellmanResults.Remove(diffieHellmanClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiffieHellmanClassExists(int id)
        {
            return _context.DiffieHellmanResults.Any(e => e.Id == id);
        }
    }
}
