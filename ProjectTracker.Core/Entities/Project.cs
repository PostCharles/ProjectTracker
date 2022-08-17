using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Core.Entities
{
    public class Project
    {
        
        public long? Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
