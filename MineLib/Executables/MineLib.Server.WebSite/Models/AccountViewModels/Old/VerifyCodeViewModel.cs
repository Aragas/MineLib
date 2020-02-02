using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat.Models.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; } = default!;

        [Required]
        public string Code { get; set; } = default!;

        public string? ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}