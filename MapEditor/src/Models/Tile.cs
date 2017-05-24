using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapEditor.src.Models
{
	public class Tile
	{
		public int ID;
		public Vector Position;
		public Rectangle Rectangle;
		public BitmapImage Image;
	}
}