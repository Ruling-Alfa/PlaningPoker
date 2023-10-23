using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PlaningPokerInLaw.Shared
{
    public class Participant
    {
        public int Id { get; set; }
        [Required]
        [NotNull]
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        [NotNull]
        public string Email { get; set; }
        public bool isModerator { get; set; }
    }
}
