﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartPPA.Models;
using SmartPPA.Models.ViewModels;

namespace SmartPPA.Controllers
{
    [Authorize]
    public class SmartUsersController : Controller
    {
        private IDocumentRepository _repo;

        public SmartUsersController(IDocumentRepository repo)
        {
            _repo = repo;
        }

        // GET: SmartUsers
        public IActionResult Index(string searchString)
        {
            UserIndexListViewModel vm = new UserIndexListViewModel();
            vm.CurrentFilter = searchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                vm.Users = _repo.Users.Where(x => x.DisplayName.Contains(searchString));
            }
            else
            {
                vm.Users = _repo.Users.ToList();
            }
            return View(vm);
        }


        // GET: SmartUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SmartUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,BlueDeckId,LogonName,DisplayName")] SmartUser smartUser)
        {
            if (ModelState.IsValid)
            {
                _repo.SaveUser(smartUser);
                
                return RedirectToAction(nameof(Index));
            }
            return View(smartUser);
        }

        // GET: SmartUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartUser = _repo.Users.FirstOrDefault(x => x.UserId == id);
            if (smartUser == null)
            {
                return NotFound();
            }
            return View(smartUser);
        }

        // POST: SmartUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,DisplayName")] SmartUser smartUser)
        {
            if (id != smartUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _repo.SaveUser(smartUser);                    
                return RedirectToAction("Choices", "Home");
            }
            return View(smartUser);
        }

        // GET: SmartUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartUser = _repo.Users
                .FirstOrDefault(m => m.UserId == id);
            if (smartUser == null)
            {
                return NotFound();
            }

            return View(smartUser);
        }

        // POST: SmartUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smartUser = _repo.Users.FirstOrDefault(x => x.UserId == id);
            _repo.RemoveUser(smartUser);
            return RedirectToAction(nameof(Index));
        }

        private bool SmartUserExists(int id)
        {
            return _repo.Users.Any(e => e.UserId == id);
        }
    }
}
