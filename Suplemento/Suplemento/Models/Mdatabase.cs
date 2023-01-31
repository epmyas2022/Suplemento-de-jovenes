using System;
using System.Collections.Generic;
using System.Text;

namespace Suplemento.Models
{
    public class Mdatabase

    { 
        public string name { get; set; }
        public int total { get; set; }

        public string DataTime { get; set; }

        public IList<Schema> schema { get; set; }

    }


    public class Schema
    {
        public string Title { get; set; }
        public int Number { get; set; }

        public IList<string[]> Letters { get; set; }


    }
}
