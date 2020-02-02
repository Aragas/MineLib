using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat.Models.AccountViewModels
{
    public sealed class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
    }
}