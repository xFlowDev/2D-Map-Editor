using System.Collections.Generic;

namespace MapEditor.src.Models
{
	public class Map
	{
		public int x { get; set; }
		public int y { get; set; }
		public int width { get; set; }
		public int height { get; set; }

		public List<Tile> tiles;

		public Map(int x, int y, int w, int h)
		{
			this.x = x;
			this.y = y;
			width = w;
			height = h;
		}
	}
}