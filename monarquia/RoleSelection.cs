using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class RoleSelection
    {
        public ITranslateable Value { get; private set; }
        public Noun UnderlyingObject { get; private set; }

        public RoleSelection(ITranslateable value, Noun underlyingObject = null)
        {
            this.Value = value;
            this.UnderlyingObject = (underlyingObject ?? value) as Noun;
        }
    }

}
