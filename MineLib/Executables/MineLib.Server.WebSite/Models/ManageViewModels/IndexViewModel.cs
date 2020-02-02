using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat.Models.ManageViewModels
{
    public sealed class IndexViewModel
    {
        public string Username { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;
    }
}