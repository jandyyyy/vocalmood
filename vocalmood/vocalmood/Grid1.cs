using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace vocalmood
{
	public class Grid1 : ContentPage
	{
        int count = 1;

		public Grid1 ()
		{
            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = 20
            };

            var grid = new Grid
            {
                RowSpacing = 50
            };

            grid.Children.Add(new Xamarin.Forms.Label { Text = "This" }, 0, 0);
            grid.Children.Add(new Xamarin.Forms.Label { Text = "text is" }, 1, 0);
            grid.Children.Add(new Xamarin.Forms.Label { Text = "in a" }, 0, 1);
            grid.Children.Add(new Xamarin.Forms.Label { Text = "grid!" }, 1, 1);

            var gridButton = new Button { Text = "So is this button!\nClick here." };
            gridButton.Clicked += delegate
            {
                gridButton.Text = string.Format("Thanks! {0} clicks.", count++);

            };

            layout.Children.Add(grid);
            layout.Children.Add(gridButton);
            Content = layout;

		}
	}
}
