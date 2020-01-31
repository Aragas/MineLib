﻿using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat.Models.AccountViewModels
{
    public class UseRecoveryCodeViewModel
    {
        [Required]
        public string Code { get; set; }

        public string? ReturnUrl { get; set; }
    }
}