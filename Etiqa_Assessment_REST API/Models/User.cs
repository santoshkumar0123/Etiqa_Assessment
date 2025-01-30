using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etiqa_Assessment_REST_API.Models
{
    public class User
    {
        [Key]
        public required string username { get; set; }
        public string? mail {  get; set; }
        public int? phonenumber { get; set; }
        public string? skillsets { get; set; }
        public string? hobby { get; set; }
        //public UserDetail? UserDetails { get; set; }
    }
}
