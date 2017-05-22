using MapEditor.src.Models;
using MapEditor.src.Utils;
using MapEditor.src.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Map MapInEditor = new Map(0, 0, 0, 0);

		private bool isMousePressed = false;
		private Point startPoint;

		private BitmapImage Spritesheet;

		private Image SelectedTile;

		public MainWindow()
		{
			InitializeComponent();

			//FooterText.Text = "Geladen" + " Map:";
			//FooterText.Text += "X: " + MapInEditor.x + " " + " Y: " + MapInEditor.y;
			//FooterText.Text += "Width: " + MapInEditor.width + " Height: " + MapInEditor.height;
		}

		private void OpenNewFileWindow(object sender, RoutedEventArgs e)
		{
			NewFileWindow newFileWindow = new NewFileWindow();
			newFileWindow.Show();
		}

		public void CreateNewMap(Map map)
		{
			MapInEditor = map;
			Focus();

			EditorCanvas.Width = map.width * map.x * 5;
			EditorCanvas.Height = map.height * map.y * 5;
			//Set the Canvas to be the Size of the Map you want to create

			//generate Map
			map.tiles = new List<Tile>();
			int id = 0;
			for (int h = 0; h < map.height; h++)
			{
				for (int w = 0; w < map.width; w++)
				{
					var tile = new Tile();
					tile.ID = id++;
					tile.Position = new Vector(w * map.x, h * map.y);
					tile.Rectangle = new Rectangle();
					tile.Rectangle.Width = map.x;
					tile.Rectangle.Height = map.y;

					tile.Rectangle.Stroke = new SolidColorBrush(Colors.Black);
					tile.Rectangle.StrokeThickness = 1;
					tile.Rectangle.Fill = new SolidColorBrush(Colors.Transparent);
					// Damit das funktioniert muss das Rectangle mit Transparent gefülltt werden
					// Sonst Klappt der Klick nur wenn man auf den Rand klickt
					tile.Rectangle.MouseLeftButtonDown += DrawTile;

					//TODO Die anfangs position der Tiles so anpassen,
					// dass sie in der Mitte des Canvas angezeigt werden
					EditorCanvas.Children.Add(tile.Rectangle);
					Canvas.SetLeft(tile.Rectangle, tile.Position.X);
					Canvas.SetTop(tile.Rectangle, tile.Position.Y);
					map.tiles.Add(tile);
				}
			}
		}

		private void DrawTile(object sender, MouseButtonEventArgs e)
		{
			if (SelectedTile != null)
			{
				var rectClicked = (Rectangle)e.Source;
				var rectInList = MapInEditor.tiles.Find(x => x.Rectangle == rectClicked);
				//Image Source dynamisch auf Auswahl eines Tiles anpassen
				rectInList.Image = SelectedTile;
				rectClicked.Fill = new ImageBrush
				{
					ImageSource = SelectedTile.Source
				};
			}
			else
			{
				FooterText.Text = "Keine Auswahl getroffen";
			}
		}

		#region CanvasMovement

		public void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
		{
			startPoint = e.GetPosition(EditorCanvas);
			isMousePressed = true;
		}

		private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isMousePressed = false;
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{
			if (isMousePressed)
			{
				var mousePos = e.GetPosition(EditorCanvas);
				var diff = startPoint - mousePos;

				Koordinaten.Text = mousePos.X + " " + mousePos.Y;
				var children = EditorCanvas.Children;
				foreach (var child in children)
				{
					var x = Canvas.GetLeft((Rectangle)child);
					var y = Canvas.GetTop((Rectangle)child);

					Canvas.SetLeft((Rectangle)child, x - diff.X);
					Canvas.SetTop((Rectangle)child, y - diff.Y);
				}
				startPoint = mousePos;
			}

			Koordinaten.Text = e.GetPosition(EditorCanvas).ToString();
		}

		#endregion CanvasMovement

		private void OpenSpritesheet(object sender, MouseButtonEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
			dlg.DefaultExt = ".png";
			dlg.Filter = "All Images |*.jpeg; *.jpg; *.png; *.bmp; *.gif|" +
						 "All Files |*.*|" +
						 "JPEG Files |*.jpeg|" +
						 "PNG Files |*.png";

			bool? result = dlg.ShowDialog();

			if (result.HasValue && result.Value)
			{
				//Beinhaltet den Pfad zum File
				var filename = dlg.FileName;
				Spritesheet = new BitmapImage(new Uri(filename));

				//Spritesheet zerschneiden
				//Einzelne Tiles als Auswahl anzeigen

				var tileList = ImageManager.SplitSpritesheet(Spritesheet, 32, 32);
				foreach (var tile in tileList)
				{
					var border = new Border();
					border.BorderBrush = Brushes.Transparent;
					border.BorderThickness = new Thickness(2);
					var tileImg = new Image();
					tileImg.Source = tile;
					tileImg.Stretch = Stretch.None;
					tileImg.VerticalAlignment = VerticalAlignment.Top;
					tileImg.HorizontalAlignment = HorizontalAlignment.Left;
					tileImg.MouseLeftButtonDown += SelectTile;
					border.Child = tileImg;
					SpriteExplorer.Children.Add(border);
				}
			}
		}

		private void SelectTile(object sender, MouseButtonEventArgs e)
		{
			foreach (Border child in SpriteExplorer.Children)
			{
				child.BorderBrush = Brushes.Transparent;
				child.BorderThickness = new Thickness(2);
			}

			SelectedTile = (Image)e.OriginalSource;
			var Border = (Border)SelectedTile.Parent;
			Border.BorderBrush = Brushes.Black;
			Border.BorderThickness = new Thickness(2);
		}
	}
}