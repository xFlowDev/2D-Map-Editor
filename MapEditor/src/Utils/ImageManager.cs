using MapEditor.src.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace MapEditor.src.Utils
{
	internal class ImageManager
	{
		public List<BitmapImage> SplitSpritesheet(BitmapImage Spritesheet, int tileWidth, int tileHeight)
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
				var bitmap = new Bitmap(ms);
				var format = bitmap.PixelFormat;

				for (int r = 0; r < rows; r++)
				{
					for (int c = 0; c < cols; c++)
					{
						var rect = new Rectangle(c * tileWidth, r * tileHeight, tileWidth, tileHeight);
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

		public BitmapImage CreateMapImage(Map map)
		{
			var MapImage = new BitmapImage();
			var bMapImage = new WriteableBitmap(map.tiles[0].Image);

			int width = map.x * map.width;
			int height = map.y * map.height;
			int stride = bMapImage.BackBufferStride;

			int[] mapPixel = new int[width * height];
			bMapImage.CopyPixels(mapPixel, stride, 0);

			foreach (var tile in map.tiles)
			{
				var wBitmap = new WriteableBitmap(tile.Image);
				int tileStride = wBitmap.BackBufferStride;
				int[] pixels = new int[map.x * map.y];
				//speichere die Pixel in dem Array
				wBitmap.CopyPixels(pixels, tileStride, 0);
				//schreibe die Bitmap innerhalb eines Rectangles in das neue Bitmap
				bMapImage.WritePixels(new System.Windows.Int32Rect(), pixels, tileStride, 10);
			}

			return MapImage;
		}
	}
}