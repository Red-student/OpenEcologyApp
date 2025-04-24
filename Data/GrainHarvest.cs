using System.ComponentModel.DataAnnotations;

namespace OpenEcologyApp.Data
{
    public class GrainHarvest
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        public string Region { get; set; } = string.Empty;
        
        [Required]
        public double Yield { get; set; }  // Урожайность в ц/га
        
        [Required]
        public double SownArea { get; set; }  // Посевная площадь в тыс. га
        
        [Required]
        public double GrossHarvest { get; set; }  // Валовый сбор в тыс. тонн
    }
} 