using ContactProBlazorApp.Client.Models;
using ContactProBlazorApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ContactProBlazorApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        [Required]
        public string? AppUserId { get; set; }

        public virtual ApplicationUser? AppUser { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = [];

        public CategoryDTO ToDTO()
        {
            CategoryDTO dto = new CategoryDTO()
            {
                Id = Id,
                Name = Name
            };

            foreach(Contact contact in Contacts)
            {
                //prevent circular reference by clearing the categories collection in the contact before converting to DTO
                contact.Categories.Clear();
                dto.Contacts.Add(contact.ToDTO());
            }
            return dto;
        }
    }
}
