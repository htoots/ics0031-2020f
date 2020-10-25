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
    public class RSAController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RSAController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RSA
        public async Task<IActionResult> Index()
        {
            return View(await _context.RSAResults.ToListAsync());
        }

        // GET: RSA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rSAClass = await _context.RSAResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rSAClass == null)
            {
                return NotFound();
            }

            return View(rSAClass);
        }

        // GET: RSA/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RSA/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PrimeP,PrimeQ")] RSAClass rSAClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rSAClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rSAClass);
        }

        // GET: RSA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rSAClass = await _context.RSAResults.FindAsync(id);
            if (rSAClass == null)
            {
                return NotFound();
            }
            return View(rSAClass);
        }

        // POST: RSA/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimeP,PrimeQ")] RSAClass rSAClass)
        {
            if (id != rSAClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rSAClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RSAClassExists(rSAClass.Id))
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
            return View(rSAClass);
        }

        // GET: RSA/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rSAClass = await _context.RSAResults
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rSAClass == null)
            {
                return NotFound();
            }

            return View(rSAClass);
        }

        // POST: RSA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rSAClass = await _context.RSAResults.FindAsync(id);
            _context.RSAResults.Remove(rSAClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RSAClassExists(int id)
        {
            return _context.RSAResults.Any(e => e.Id == id);
        }
    }
}
