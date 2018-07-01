using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Civkac.Models
{
    public class Outputter
    {
        public static List<dynamic> getDynamicList<T>(List<T> list) where T : IDynamicable{
            List<dynamic> toReturn = new List<dynamic>();
            foreach (T t in list) {
                toReturn.Add(t.getDynamic());
            }

            return toReturn;
        }
    }
}
