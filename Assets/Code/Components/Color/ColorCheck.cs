using Utils.Constants;

namespace Components.Color
{
	public sealed class ColorCheck : Colorer
	{
		public bool Check(EColors color) =>
			_color == color;
	}
}