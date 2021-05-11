using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace TheOne.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Connect to one User%%%%%%%%%%%%%&&&&&&&&&&&&&&&&&&&&&
        public int UserId { get; set; }
        // %%%%%%%%%%%%%%%%%%%%%%%Connects to all Users%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public User UserToEvent { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Connect to  one Wedding %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public int EventId { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Connect to all Weddings %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public Event EventToUser { get; set; }

        // %%%%%%%%%%%%%%%%%%%%%%%Update time and date%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}