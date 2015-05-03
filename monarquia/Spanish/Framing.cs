using System;
using System.Collections.Generic;

namespace monarquia
{
	public class Framing
	{
		public readonly PointOfView PointOfView;
		public readonly Conjugation Conjugation;

		public Framing (PointOfView pointOfView, Conjugation conjugation)
		{
			this.PointOfView = pointOfView;
			this.Conjugation = conjugation;
		}
	}

	public class RoleSelection
	{
		Framing Framing;
		Dictionary<string, ITranslateable> Roles;

		public RoleSelection(Framing framing) {
			this.Framing = framing;
			this.Roles = new Dictionary<string, ITranslateable> ();
		}
			
		public RoleSelection WithRole(string role, ITranslateable value) {
		
			var result = new RoleSelection (this.Framing);

			result.Roles = new Dictionary<string, ITranslateable> (this.Roles);

			result.Roles.Add (role, value);

			return result;
		}

		public ITranslateable GetForRole(string role) {
		
			return this.Roles [role];
		}
	}
}

