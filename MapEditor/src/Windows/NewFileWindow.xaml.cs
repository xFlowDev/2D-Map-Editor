using System.Windows;
using MapEditor.src.Models;

namespace MapEditor.src.Windows
{
	/// <summary>
	/// Interaction logic for NewFile.xaml
	/// </summary>
	public partial class NewFileWindow : Window
	{
		public NewFileWindow()
		{
			InitializeComponent();
			Focus();
		}

		private void CreateNewMap(object sender, RoutedEventArgs e)
		{
			var x = int.Parse(TileSizeX.Text);
			var y = int.Parse(TileSizeY.Text);
			var width = int.Parse(GridWidth.Text);
			var height = int.Parse(GridHeight.Text);
			var map = new Map(x, y, width, height);
			((MainWindow)Application.Current.MainWindow).CreateNewMap(map) ;
			Close();
		}
	}
}
