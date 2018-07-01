using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    interface IEditable
    {
        void edit(String newText);
        void delete();
    }
}
