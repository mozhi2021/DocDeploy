using System.ComponentModel.DataAnnotations;

namespace DocumentManagementAPI.Models
{
    public class DocItem
    {
        [Key]
        public IFormFile? File { get; set; }
    }
}
