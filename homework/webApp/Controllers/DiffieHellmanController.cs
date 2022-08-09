using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Authorization;
using webApp.Data;

namespace webApp.Controllers
{
    [Authorize]
    public class DiffieHellmanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiffieHellmanController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier);
            return claim?.Value ?? "";
        }

        // GET: DiffieHellman
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.DiffieHellmanResults.Where(c => c.UserId == userId).ToListAsync());
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
        public async Task<IActionResult> Create(DiffieHellmanClass diffieHellmanClass)
        {
            if (diffieHellmanClass.SecretA <= 0)
            {
                ModelState.AddModelError(nameof(diffieHellmanClass.SecretA), "SecretA cannot be 0 or lower");
            }

            if (diffieHellmanClass.SecretB <= 0)
            {
                ModelState.AddModelError(nameof(diffieHellmanClass.SecretB), "SecretB cannot be 0 or lower");
            }
            if (!Helpers.PrimalityTest(diffieHellmanClass.BaseG) || diffieHellmanClass.BaseG <= 0)
            {
                ModelState.AddModelError(nameof(diffieHellmanClass.BaseG), "BaseG has to be prime and bigger than 0");
            }

            if (!Helpers.PrimalityTest(diffieHellmanClass.ModulusP) || diffieHellmanClass.ModulusP <= 0)
            {
                ModelState.AddModelError(nameof(diffieHellmanClass.ModulusP), "ModulusP has to be prime and bigger than 0");
            }

            if (ModelState.IsValid)
            {
                List<ulong> keyList = DiffieHellman.DiffieHellmanCalc(diffieHellmanClass.SecretA,
                    diffieHellmanClass.SecretB, diffieHellmanClass.ModulusP, diffieHellmanClass.BaseG);
                diffieHellmanClass.Key1 = keyList[0];
                diffieHellmanClass.Key2 = keyList[1];
                diffieHellmanClass.UserId = GetUserId();
                if (diffieHellmanClass.UserId != "")
                {
                    _context.Add(diffieHellmanClass);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
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
            if (diffieHellmanClass == null || diffieHellmanClass.UserId != GetUserId())
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,SecretA,SecretB,ModulusP,BaseG,Key1,Key2")] DiffieHellmanClass diffieHellmanClass)
        {
            if (id != diffieHellmanClass.Id)
            {
                return NotFound();
            }


            diffieHellmanClass.UserId = GetUserId();
            
            if (ModelState.IsValid && diffieHellmanClass.UserId != "")
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
            if (diffieHellmanClass == null || diffieHellmanClass.UserId != GetUserId())
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
            if (diffieHellmanClass.UserId == GetUserId())
            {
                _context.DiffieHellmanResults.Remove(diffieHellmanClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DiffieHellmanClassExists(int id)
        {
            return _context.DiffieHellmanResults.Any(e => e.Id == id);
        }
    }
}
