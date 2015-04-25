using System;

namespace monarquia
{
	public interface ITranslateable {
		string AsSpanish(PointOfView pointOfView);
		string AsEnglish(PointOfView pointOfView);
	}
	
}
