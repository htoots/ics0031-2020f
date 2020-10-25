using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crypto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.CodeAnalysis.FlowAnalysis;
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
            return View(await _context.RsaResults.ToListAsync());
        }

        // GET: RSA/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rSAClass = await _context.RsaResults
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
        public async Task<IActionResult> Create(RSAClass RsaClass)
        {
            if (!Helpers.PrimalityTest(RsaClass.PrimeP) || RsaClass.PrimeP <= 0)
            {
                ModelState.AddModelError(nameof(RsaClass.PrimeP), "PrimeP has to be a Prime and bigger than 0");
            }

            if (!Helpers.PrimalityTest(RsaClass.PrimeQ) || RsaClass.PrimeQ <= 0)
            {
                ModelState.AddModelError(nameof(RsaClass.PrimeQ), "PrimeQ has to be a prime and bigger than 0");
            }

            if (String.IsNullOrEmpty(RsaClass.BaseText))
            {
                ModelState.AddModelError(nameof(RsaClass.BaseText), "BaseText cannot be empty");
            }
            if (ModelState.IsValid)
            {
                byte[] enbytes = RSA.RsaEncryptString(RsaClass.BaseText, RsaClass.PrimeP, RsaClass.PrimeQ);
                // dirty debug
                //Console.WriteLine(Helpers.GetString(enbytes));
                //Console.WriteLine("Debug:" + Helpers.GetString(RSA.RsaDecrypt(enbytes, RsaClass.PrimeP, RsaClass.PrimeQ)));
                RsaClass.EncryptedText = enbytes;
                _context.Add(RsaClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(RsaClass);
        }

        // GET: RSA/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rSAClass = await _context.RsaResults.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,PrimeP,PrimeQ,BaseText,EncryptedText")] RSAClass rSAClass)
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

            var rSAClass = await _context.RsaResults
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
            var rSAClass = await _context.RsaResults.FindAsync(id);
            _context.RsaResults.Remove(rSAClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RSAClassExists(int id)
        {
            return _context.RsaResults.Any(e => e.Id == id);
        }
    }
}
