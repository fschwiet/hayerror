using System;
using System.Collections.Generic;
using System.Linq;

namespace monarquia
{
	public interface ICannedData {
		IEnumerable<ExpressableVerbRoleSelection> GetAllVerbRoleSelectors();
	}
	
}
