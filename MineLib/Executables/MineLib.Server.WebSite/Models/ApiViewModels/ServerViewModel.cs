using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.WebSite.Models.ApiViewModels
{
    public sealed class ServerViewModel
    {
        [Required]
        [Display(Name = "User")]
        public string? User { get; set; } = default!;

        [Required]
        [Display(Name = "Servers")]
        public IEnumerable<ClassicServer> Servers { get; set; } = default!;
    }
}