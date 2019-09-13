﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BonTemps.Data;

namespace BonTemps.Areas.Systeem.Models
{
    [Area("Systeem")]
    public class ReserveringenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReserveringenController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Systeem/Reserveringen
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reserveringen.ToListAsync());
        }

        // GET: Systeem/Reserveringen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reserveringen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservering == null)
            {
                return NotFound();
            }

            return View(reservering);
        }

        // GET: Systeem/Reserveringen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Systeem/Reserveringen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NaamReserveerder,Email,HuisTelefoonNummer,MobielTelefoonNummer,AantalPersonen,ReserveringAangemaakt,ReserveringsDatum")] Reservering reservering)
        {
            reservering.ReserveringAangemaakt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(reservering);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservering);
        }

        // GET: Systeem/Reserveringen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reserveringen.FindAsync(id);
            if (reservering == null)
            {
                return NotFound();
            }
            return View(reservering);
        }

        // POST: Systeem/Reserveringen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NaamReserveerder,Email,HuisTelefoonNummer,MobielTelefoonNummer,AantalPersonen,ReserveringAangemaakt,ReserveringsDatum")] Reservering reservering)
        {
            if (id != reservering.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservering);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReserveringExists(reservering.Id))
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
            return View(reservering);
        }

        // GET: Systeem/Reserveringen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reserveringen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservering == null)
            {
                return NotFound();
            }

            return View(reservering);
        }

        // POST: Systeem/Reserveringen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservering = await _context.Reserveringen.FindAsync(id);
            _context.Reserveringen.Remove(reservering);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReserveringExists(int id)
        {
            return _context.Reserveringen.Any(e => e.Id == id);
        }
    }
}