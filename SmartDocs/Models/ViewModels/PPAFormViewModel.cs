﻿using SmartDocs.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models.ViewModels
{
    /// <summary>
    /// The View Model used to Create/Edit a PPA Document
    /// </summary>
    public class PPAFormViewModel
    {
        /// <summary>
        /// Gets or sets the PPA document's identifier.
        /// </summary>
        /// <remarks>
        /// This is the Id of the <see cref="Models.SmartDocument"/> from which the View Model is built.
        /// </remarks>
        /// <value>
        /// The ppa identifier.
        /// </value>
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the First Name of the employee who is the subject of the PPA.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter a First Name.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name of the employee who is the subject of the PPA.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter a Last Name.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the PGPD Id Number (Badge Number) of the employee who is the subject of the PPA
        /// </summary>
        /// <value>
        /// The department identifier number.
        /// </value>
        [Display(Name = "ID#"), StringLength(5, ErrorMessage = "Must be 5 characters or fewer."), Required(ErrorMessage = "Enter employees ID Number")]
        public string DepartmentIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Payroll Identifier Number (EIS) of the employee who is the subject of the PPA.
        /// </summary>
        /// <value>
        /// The payroll identifier number.
        /// </value>
        [Display(Name = "Payroll ID#"), StringLength(10, ErrorMessage = "Must be 10 characters or fewer."), Required(ErrorMessage = "A Payroll number is required.")]
        public string PayrollIdNumber { get; set; }

        /// <summary>
        /// Gets or sets the Position Number of the employee who is the subject of the PPA.
        /// </summary>
        /// <value>
        /// The position number.
        /// </value>
        [Display(Name = "Position Number"), StringLength(10, ErrorMessage = "Must be 10 characters or fewer."), Required(ErrorMessage = "Enter the employee's Position Number.")]
        public string PositionNumber { get; set; }

        /// <summary>
        /// Gets or sets the Department or Division to which the subject employee is assigned.
        /// </summary>
        /// <value>
        /// The department division.
        /// </value>
        [Display(Name = "Department/Division:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please select the employee's assigned unit.")]
        public string DepartmentDivision {get;set;}

        /// <summary>
        /// Gets or sets the Code for the Department or Division to which the subject employee is assigned.
        /// </summary>
        /// <value>
        /// The department division code.
        /// </value>
        [Display(Name = "Department/Division Code:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer."), Required(ErrorMessage = "Please enter the Department Code.")]
        public string DepartmentDivisionCode { get; set; }

        /// <summary>
        /// Gets or sets the subject employee's work place address.
        /// </summary>
        /// <value>
        /// The work place address.
        /// </value>
        [Display(Name = "Work Location:"), StringLength(100, ErrorMessage = "Must be 100 characters or fewer."), Required(ErrorMessage = "Enter the employee's work address.")]
        public string WorkPlaceAddress { get; set; }

        /// <summary>
        /// Gets or sets the component for which the subject employee is a supervisor.
        /// </summary>
        /// <value>
        /// The supervised by employee.
        /// </value>
        [Display(Name = "Supervised by Employee:"), StringLength(50, ErrorMessage = "Must be 50 characters or fewer.")]
        public string SupervisedByEmployee { get; set; }

        /// <summary>
        /// Gets or sets the Start Date for the rating period of the PPA.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Display(Name = "Start Date:"), Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the End Date for the rating period of the PPA.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        [Display(Name = "End Date:"), Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the <see cref="SmartJob"/> associated with the <see cref="SmartDocument.SmartDocumentType.PPA"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        [Display(Name = "Job Title"), Required(ErrorMessage = "Please choose a Job Description.")]
        public int JobId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="JobDescription"/> associated with the <see cref="SmartDocument.SmartDocumentType.PPA"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public JobDescription Job { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="T:SmartDocs.Models.SmartUser"/> who is the author of the <see cref="SmartDocument.SmartDocumentType.PPA"/> from which the View Model is built.
        /// </summary>
        /// <value>
        /// The author user identifier.
        /// </value>
        [Display(Name = "Immediate Supervisor"), Required(ErrorMessage = "Please select your name from the list.")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// Gets or sets the Assessment narrative field text.
        /// </summary>
        /// <value>
        /// The assessment.
        /// </value>
        [MaxLength(5000, ErrorMessage = "This field is limited to 5000 characters."), Display(Name ="Performance Assessment:")]
        [DataType(DataType.MultilineText)]
        public string Assessment { get; set; }

        /// <summary>
        /// Gets or sets the Recommendation narrative field text.
        /// </summary>
        /// <value>
        /// The recommendation.
        /// </value>
        [MaxLength(5000, ErrorMessage = "This field is limited to 5000 characters."), Display(Name = "Supervisor's Recommendations:")]
        [DataType(DataType.MultilineText)]
        public string Recommendation { get; set; }

        /// <summary>
        /// Gets or sets the list of categories associated with the Job Description for the PPA.
        /// </summary>
        /// <remarks>
        /// I *think* this has been deprecated, now that the VM object contains a <see cref="SmartJob"/> property.
        /// As I recall, I was using this to invoke the JobDescriptionCategoryList view component, but I changed the ViewComponent to invoke directly from a
        /// <see cref="JobDescription"/> instead of a List of <see cref="JobDescriptionCategory"/>
        /// </remarks>
        /// <value>
        /// The categories.
        /// </value>
        public List<JobDescriptionCategory> Categories { get; set; }

        /// <summary>
        /// Gets or sets the list of Job Descriptions available in the database.
        /// </summary>
        /// <remarks>
        /// This builds a list of <see cref="JobDescriptionListItem"/> from the available <see cref="SmartJob"/>s.
        /// </remarks>
        /// <value>
        /// The job list.
        /// </value>
        public List<JobDescriptionListItem> JobList { get; set; }

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <remarkds>
        /// This shows a list of <see cref="OrganizationComponent"/> in the database.
        /// </remarkds>
        /// <value>
        /// The components.
        /// </value>
        public List<OrganizationComponent> Components { get; set; }

        /// <summary>
        /// Gets or sets the Users list.
        /// </summary>
        /// <remarks>
        /// This assembles a List of <see cref="UserListItem"/> from the available <see cref="SmartUser"/>s.
        /// </remarks>
        /// <value>
        /// The users.
        /// </value>
        public List<UserListItem> Users { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PPAFormViewModel"/> class.
        /// <remarks>
        /// This constructor is used when a new PPA is being created.
        /// </remarks>
        /// </summary>
        public PPAFormViewModel()
        {
            // default the dates to today and a year ago from today
            StartDate = DateTime.Today.AddYears(-1);
            EndDate = DateTime.Today;

            // assign list properties to new lists to prevent NullRef errors
            Categories = new List<JobDescriptionCategory>();
            JobList = new List<JobDescriptionListItem>();
            Components = new List<OrganizationComponent>();
            Users = new List<UserListItem>();            
        }
    }
}
