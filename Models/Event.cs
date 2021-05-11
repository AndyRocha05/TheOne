using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace TheOne.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Title%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Enter Title of event")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string Title { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Time%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
    
        public DateTime Time { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%% Date%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Duration Address%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int Duration { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Description%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public string Type { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Description%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        [Required(ErrorMessage="Please add a Description")]

        public string Description{ get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        // %%%%%%%%%%%%%%%%%%%%%%%Gets the UserId %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Who posted the Wedding  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User PostedBy { get; set;}
        // %%%%%%%%%%%%%%%%%%%%%%%Gets you to the middle table %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public List<Participant> Guests  { get; set;}
    }
}