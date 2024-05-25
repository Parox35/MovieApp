using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Duration { get; set; }
        public string Summary { get; set; }
    }
}
