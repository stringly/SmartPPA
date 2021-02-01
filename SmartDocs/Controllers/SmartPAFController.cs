﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using SmartDocs.Models;
using SmartDocs.Models.SmartDocumentClasses;
using SmartDocs.Models.Types;
using SmartDocs.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace SmartDocs.Controllers
{
    /// <summary>
    /// Controller for <see cref="SmartDocument.SmartDocumentType.PAF"/> interactions
    /// </summary>
    [Authorize(Roles = "User, Administrator")]
    public class SmartPAFController : Controller
    {
        private IDocumentRepository _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="SmartPAFController"/> class.
        /// </summary>
        /// <remarks>
        /// This controller requires a Repository to be injected when it is created. Refer to middleware in <see cref="Startup.ConfigureServices"/>
        /// </remarks>
        /// <param name="repo">An <see cref="IDocumentRepository"/></param>
        public SmartPAFController(IDocumentRepository repo)
        {
            _repository = repo;           
        }
        /// <summary>
        /// Shows the view to create a new SmartPAF.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/></returns>
        public ActionResult Create()
        {
            if (User.HasClaim(x => x.Type == "UserId"))
            {
                // create a new, empty ViewModel
                int currentUserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                PAFFormViewModel vm = new PAFFormViewModel(currentUserId);
                vm.HydrateLists(_repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(), _repository.Components.ToList(), _repository.Users.Select(x => new UserListItem(x)).ToList());
                ViewData["Title"] = "Create PAF";
                return View(vm);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        /// <summary>
        /// Creates a <see cref="SmartDocument.SmartDocumentType.PPA"/> from the POSTed form data.
        /// </summary>
        /// <param name="form">The POSTed form data, bound to a <see cref="PAFFormViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(
            "SelectedPAFType," +
            "FirstName," +
            "LastName," +
            "DepartmentIdNumber," +
            "PayrollIdNumber," +
            "DepartmentIdNumber," +
            "DepartmentDivision," +
            "DepartmentDivisionCode," +
            "WorkPlaceAddress," +
            "AuthorUserId," +
            "StartDate," +
            "EndDate," +
            "JobId," +
            "Assessment," +
            "Recommendation")] PAFFormViewModel form)
        {
            if (!ModelState.IsValid)
            {

                // next, re-populate the VM drop down lists
                form.HydrateLists(_repository.Jobs.Select(x => new JobDescriptionListItem(x)).ToList(), _repository.Components.ToList(), _repository.Users.Select(x => new UserListItem(x)).ToList());                
                // return the View with the validation messages
                ViewData["Title"] = "Create PAF: Error";
                return View(form);
            }
            else
            {
                // validation success, create new generator and pass repo
                SmartPAFFactory factory = new SmartPAFFactory(_repository);
                // call generator method to pass form data
                factory.CreatePAF(form);
                // redirect to success view with PPA as querystring param
                return RedirectToAction("SaveSuccess", new { id = factory.PAF.DocumentId });
            }
        }
        /// <summary>
        /// View that shows success message and allows user to download file or navigate back to MyDocuments.
        /// </summary>
        /// <remarks>This method displays a link that invokes the <see cref="HomeController.Download(int)"/> method.</remarks>
        /// <param name="id">The id of the newly generated <see cref="SmartDocument.SmartDocumentType.PAF"/></param>
        /// <returns>An <see cref="IActionResult"/></returns>
        public IActionResult SaveSuccess(int id)
        {
            // this is a simple view, so use VB instead of a VM                        
            SmartDocument doc = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            if (User.HasClaim(x => x.Type == "UserId"))
            {                
                int currentUserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                if (doc.AuthorUserId == currentUserId)
                {
                    ViewBag.PAFId = id;
                    ViewBag.FileName = doc.FileName;
                    ViewData["Title"] = "Success!";
                    return View();
                }                
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return RedirectToAction("NotAuthorized", "Home");
            

        }

        /// <summary>
        /// Shows the view to confirm deletion of a <see cref="SmartDocument.SmartDocumentType.PAF"/>.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="SmartDocument.SmartDocumentType.PAF"/>.</param>
        /// <returns>An <see cref="IActionResult"/></returns>
        public IActionResult Delete(int? id)
        {
            // query string is empty, return 404
            if (id == null)
            {
                return NotFound();
            }
            if (User.HasClaim(x => x.Type == "UserId"))
            {
                int currentUserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                // retrieve the SmartPPA from the repo
                var toDelete = _repository.Documents.FirstOrDefault(m => m.DocumentId == id);
                if (toDelete == null)
                {
                    // no SmartDoc could be found with the provided id
                    return NotFound();
                }
                if (toDelete.AuthorUserId == currentUserId)
                {
                    // return the View (which uses the Domain Object as a model)
                    ViewData["Title"] = "Delete PAF";
                    return View(toDelete);
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return RedirectToAction("NotAuthorized", "Home");
        }

        /// <summary>
        /// Deletes <see cref="SmartDocument.SmartDocumentType.PAF"/> with the provided ID.
        /// </summary>
        /// <param name="id">The identifier of the <see cref="SmartDocument.SmartDocumentType.PAF"/> to be deleted.</param>
        /// <returns>An <see cref="SmartDocument.SmartDocumentType.PAF"/></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // retrieve the SmartPPA from the repo
            var toDelete = _repository.Documents.FirstOrDefault(x => x.DocumentId == id);
            if (toDelete == null)
            {
                return NotFound();
            }
            if (User.HasClaim(x => x.Type == "UserId"))
            {
                int currentUserId = Convert.ToInt32(((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);
                if (toDelete.AuthorUserId == currentUserId)
                {
                    _repository.RemoveSmartDoc(toDelete);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return RedirectToAction("NotAuthorized", "Home");

        }
    }
}