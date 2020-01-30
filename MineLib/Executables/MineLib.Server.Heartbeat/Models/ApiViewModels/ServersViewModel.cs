using MineLib.Server.Heartbeat.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat.Models.ManageViewModels
{
    public class ServersViewModel
    {
        public List<ClassicServer> Servers { get; set; }
        /*
        public string PhoneNumber { get; set; }

        [Required, MaxLength(64), Display(Name = "Name")]
        public string Name { get; set; }

        [Required, MaxLength(45), Display(Name = "IP")]
        public string IP { get; set; }

        [Required, Display(Name = "Port")]
        public ushort Port { get; set; }

        [StringLength(32, MinimumLength = 32, ErrorMessage = "This field must be 32 characters"), Display(Name = "Hash")]
        public string Hash { get; set; }

        [Required, MaxLength(256), Display(Name = "Salt")]
        public string Salt { get; set; }

        [Required, Display(Name = "Players")]
        public int Players { get; set; }

        [Required, Display(Name = "Max Players")]
        public int MaxPlayers { get; set; }

        [Display(Name = "Version")]
        public int? Version { get; set; }

        [MaxLength(256), Display(Name = "Software")]
        public string? Software { get; set; }

        [Display(Name = "Is Supporting Web")]
        public bool? IsSupportingWeb { get; set; }

        [Required, Display(Name = "Added")]
        public DateTimeOffset Added { get; set; }
        */
    }
}