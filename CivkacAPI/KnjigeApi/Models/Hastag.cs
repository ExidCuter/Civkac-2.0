using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    public class Hastag
    {
        private int id;
        private String name;

        public int Id => id;

        public string Name => name;

        public Hastag(string name)
        {
            this.id = -1;
            this.name = name;
        }
    }
}
