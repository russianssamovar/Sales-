using System;
using System.Collections.Generic;

namespace Sales.Models
{
    public class Book
    {
            public int Id { get; set; }
            public string Name { get; set; } 
            public string Autor { get; set; } 
            public DateTime ReleaseDate { get; set; }
            public string ISBNCode { get; set; }
            public int Price { get; set; }
            public int Count { get; set; }

    }
}
