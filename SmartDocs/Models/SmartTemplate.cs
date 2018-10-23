﻿using System.ComponentModel.DataAnnotations;

namespace SmartDocs.Models
{
    public class SmartTemplate
    {
        [Key]
        public int TemplateId { get; set; }
        public string DocumentName { get; set; }
        public byte[] DataStream { get; set; }
    }
}