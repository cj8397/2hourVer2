using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.ViewModel
{
    public class Message
    {
        [Display(Name = "Your email address")]
        [Required(ErrorMessage = "Your email address is required.")]
        [RegularExpression(@"^[_A-Za-z0-9-\+]+(\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,4})$", ErrorMessage = "Enter Correct Email Address!")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "An email subject is required.")]
        public string Subject { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "A message is required.")]
        public string Body { get; set; }

        public Message() { }
        public Message(string sender, string subject, string body)
        {
            Sender = sender;
            Subject = subject;
            Body = body;
        }

    }
}