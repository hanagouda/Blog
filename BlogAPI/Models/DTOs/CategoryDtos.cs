using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs
{
    public class CategoryDtos
    {
        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
