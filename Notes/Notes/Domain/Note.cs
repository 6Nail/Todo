using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Domain
{
    public class Note
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
