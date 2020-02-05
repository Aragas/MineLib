using System;
using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.WebSite.Models
{
    public sealed class ClassicServer
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(45)]
        public string IP { get; set; } = default!;

        [Required]
        public ushort Port { get; set; } = default!;

        [Key]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "This field must be 32 characters")]
        public string Hash { get; set; } = default!;

        [Required, MaxLength(256)]
        public string Salt { get; set; } = default!;

        [Required]
        public int Players { get; set; } = default!;

        [Required]
        public int MaxPlayers { get; set; } = default!;

        [Required]
        public bool IsPublic { get; set; } = default!;

        public int? Version { get; set; } = default!;

        [MaxLength(256)]
        public string? Software { get; set; } = default!;

        public bool? IsSupportingWeb { get; set; } = default!;

        [Required]
        public DateTimeOffset Added { get; set; } = default!;

        [Required]
        public DateTimeOffset LastUpdate { get; set; } = default!;
    }
}