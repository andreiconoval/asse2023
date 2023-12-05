﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.DAL.DomainModel
{
    public class BookDomain
    {
        [Key, Column(Order = 0)]
        public int DomainId { get; set; }

        [Key, Column(Order = 1)]
        public int BookId { get; set; }

        [ForeignKey("DomainId")]
        public virtual Domain Domain { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }

}
