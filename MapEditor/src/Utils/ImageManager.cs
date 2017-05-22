using System;
using System.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MapEditor.src.Utils
{
	internal class ImageManager
	{
		public static List<BitmapImage> SplitSpritesheet(BitmapImage Spritesheet, int tileWidth, int tileHeight)
		{
			var ss = Spritesheet;
			var TileList = new List<BitmapImage>();

			var rows = (int)ss.Height / tileHeight - (int)ss.Height % tileHeight;
			var cols = (int)ss.Width / tileWidth - (int)ss.Width % tileWidth;

			using (var ms = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(ss));
				enc.Save(ms);
				var bitmap = new System.Drawing.Bitmap(ms);
				var format = bitmap.PixelFormat;

				for (int r = 0; r < rows; r++)
				{
					for (int c = 0; c < cols; c++)
					{
						var rect = new System.Drawing.Rectangle(c * tileWidth, r * tileHeight, tileWidth, tileHeight);
						var bm = bitmap.Clone(rect, format);

						using (var ms2 = new MemoryStream())
						{
							bm.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
							ms2.Position = 0;

							var tile = new BitmapImage();
							tile.BeginInit();
							tile.StreamSource = ms2;
							tile.CacheOption = BitmapCacheOption.OnLoad;
							tile.EndInit();
							tile.Freeze();

							TileList.Add(tile);
						}

					}
				}

			}

			return TileList;
		}
	}
}