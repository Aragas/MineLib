using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace MineLib.Server.Heartbeat.Models.AccountViewModels
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; } = default!;

        public ICollection<SelectListItem>? Providers { get; set; }

        public string? ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}