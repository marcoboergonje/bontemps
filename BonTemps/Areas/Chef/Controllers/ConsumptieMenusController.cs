﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BonTemps.Areas.Systeem.Models;
using BonTemps.Data;
using Microsoft.AspNetCore.Authorization;

namespace BonTemps.Areas.Chef.Controllers
{
    [Area("Chef")]
    [Authorize(Roles ="Chef,Manager")]
    public class ConsumptieMenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsumptieMenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chef/ConsumptieMenus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ConsumptieMenu.Include(c => c.Consumptie).Include(c => c.Menu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Chef/ConsumptieMenus/Create
        public IActionResult Create()
        {
            ViewData["ConsumptieId"] = new SelectList(_context.Consumpties, "Id", "Naam");
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Menu_naam");
            return View();
        }

        // POST: Chef/ConsumptieMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,ConsumptieId")] ConsumptieMenu consumptieMenu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consumptieMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsumptieId"] = new SelectList(_context.Consumpties, "Id", "Naam", consumptieMenu.ConsumptieId);
            ViewData["MenuId"] = new SelectList(_context.Menus, "Id", "Menu_naam", consumptieMenu.MenuId);
            return View(consumptieMenu);
        }


        // GET: Chef/ConsumptieMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consumptieMenu = await _context.ConsumptieMenu
                .Include(c => c.Consumptie)
                .Include(c => c.Menu)
                .FirstOrDefaultAsync(m => m.ConsumptieId == id);
            if (consumptieMenu == null)
            {
                return NotFound();
            }

            return View(consumptieMenu);
        }

        // POST: Chef/ConsumptieMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consumptieMenu = await _context.ConsumptieMenu.FindAsync(id);
            _context.ConsumptieMenu.Remove(consumptieMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumptieMenuExists(int id)
        {
            return _context.ConsumptieMenu.Any(e => e.Id == id);
        }
    }
}
