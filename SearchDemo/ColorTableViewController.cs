using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;

using UIKit;
using CoreGraphics;
using Foundation;

namespace SearchDemo
{
    partial class ColorTableViewController : UITableViewController
    {
        private List<string> colors;
        private List<string> filteredColors;

        public ColorTableViewController(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        public ColorTableViewController()
        {
            Initialize();
        }

        private void Initialize()
        {
            colors = new List<string>
                {
                    "white", "black", "blue",
                    "orange", "red", "purple",
                    "green", "gray", "yellow", "purple"
                };
            filteredColors = new List<string>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.RegisterClassForCellReuse(typeof(UITableViewCell), ColorSource.CellId);

            var colorSource = new ColorSource(this);
            TableView.Source = colorSource;

            var sdc = searchDisplayController;
            sdc.SearchResultsSource = colorSource;
            sdc.SearchBar.TextChanged += (sender, e) => {
                filteredColors = colors.Where(color => color.ToUpper().Contains(e.SearchText.Trim().ToUpper())).ToList();
            };
        }

        // data source
        private class ColorSource:UITableViewSource
        {
            public static readonly NSString CellId = new NSString("colorCell");

            private ColorTableViewController colorController;
            private UISearchDisplayController search;

            public ColorSource(ColorTableViewController colorController)
            {
                this.colorController = colorController;
                search = colorController.searchDisplayController;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return GetColors().Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellId);
                cell = cell ?? new UITableViewCell(UITableViewCellStyle.Default, CellId);

                List<string> result = GetColors();
                cell.TextLabel.Text = result[indexPath.Row];

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var detailController = new UIViewController();
                detailController.View.BackgroundColor = UIColor.White;
                detailController.Title = GetColors()[indexPath.Row];
                colorController.NavigationController.PushViewController(detailController, true);
            }

            private List<string> GetColors()
            {
                return search.Active ? colorController.filteredColors : colorController.colors;
            }
        }
    }
}
