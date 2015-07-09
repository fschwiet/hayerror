using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monarquia
{
    public class RoleSelections
    {
        Dictionary<Role, RoleSelection> Roles;

        public RoleSelections()
        {
            this.Roles = new Dictionary<Role, RoleSelection>();
        }

        public RoleSelections WithRole(Role role, RoleSelection value)
        {

            var result = new RoleSelections();

            result.Roles = new Dictionary<Role, RoleSelection>(this.Roles);

            result.Roles.Add(role, value);

            return result;
        }

        public ITranslateable GetForRole(Role role)
        {
            RoleSelection result;

            if (!this.Roles.TryGetValue(role, out result))
                return null;
            
            return result.Value;
        }
    }
}
