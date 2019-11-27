﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;

namespace SmartDocs.Controllers
{
    [Authorize(Roles = "User, Administrator")]
    public class AwardController : Controller
    {
        private IDocumentRepository _repository;


        public AwardController(IDocumentRepository repo)
        {
            _repository = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(string NomineeName, string ClassTitle, string Division, string Agency = "Prince George's County Police Department")
        {
            int UserId = 0;
            if(User.HasClaim(x => x.Type == "UserId"))
            {
                UserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                EmptyAwardViewModel vm = new EmptyAwardViewModel();
                vm.AuthorUserId = UserId;
                vm.AgencyName = Agency;
                vm.NomineeName = NomineeName;
                vm.ClassTitle = ClassTitle;
                vm.Division = Division;
                vm.Components = _repository.Components.ToList();
                vm.Users = _repository.Users.ToList();

                ViewData["Title"] = "Create Award Form";
                return View(vm);
            }
            else
            {
                return RedirectToAction("Access Denied", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentId,AuthorUserId,AgencyName,NomineeName,ClassTitle,Division,Kind,AwardClass,AwardName,ComponentViewName,Description,HasRibbon,EligibilityConfirmationDate,StartDate,EndDate,SelectedAwardType")] SmartAwardViewModel form)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Create Award Form: Error";
                form.Components = _repository.Components.ToList();
                form.Users = _repository.Users.ToList();
                return View(form);
            }
            else
            {
                // do Award Form Factory stuff
                SmartAwardFactory factory = new SmartAwardFactory(_repository);
                factory.CreateSmartAwardForm(form);
                return RedirectToAction("SaveSuccess", new { id = factory._awardForm.DocumentId });
            }
        }
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM            
            ViewBag.SmartDocumentId = id;
            ViewBag.FileName = _repository.Documents.FirstOrDefault(x => x.DocumentId == id).FileName;
            ViewData["Title"] = "Success!";
            return View();

        }
        public IActionResult GetAwardFormViewComponent(int awardId)
        {
            SmartAwardViewModel awardVM;

            switch (awardId)
            {
                case 1:
                    GoodConductAwardViewModel goodConductAward = new GoodConductAwardViewModel();
                    goodConductAward.EligibilityConfirmationDate = DateTime.Now;
                    awardVM = goodConductAward;                    
                    break;
                case 2:
                    OutstandingPerformanceAwardViewModel outAward = new OutstandingPerformanceAwardViewModel();
                    outAward.EndDate = DateTime.Now;
                    outAward.StartDate = outAward.EndDate.AddYears(-1);
                    awardVM = outAward;
                    break;
                default:
                    return NotFound();
            }
            if(awardVM != null)
            {
                // return the ViewComponent
                return ViewComponent("AwardTypeForm", awardVM);
            }
            else
            {
                return NotFound();
            }
        }
    }
}