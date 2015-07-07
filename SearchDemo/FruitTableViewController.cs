using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;

using UIKit;
using CoreGraphics;

namespace SearchDemo
{
    partial class FruitTableViewController : UITableViewController
    {
        private List<Fruit> fruits;
        private List<Fruit> filteredFruits;

        public FruitTableViewController(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        public FruitTableViewController()
        {
            Initialize();
        }

        private void Initialize()
        {
            fruits = new List<Fruit>
                {
                    new Fruit{ Name = "apple", ColorName = "red" },
                    new Fruit{ Name = "apple", ColorName = "green" },
                    new Fruit{ Name = "apple", ColorName = "yellow" },
                    new Fruit{ Name = "banana", ColorName = "green" },
                    new Fruit{ Name = "banana", ColorName = "yellow" },
                    new Fruit{ Name = "pear", ColorName = "yellow" },
                };
            filteredFruits = new List<Fruit>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var fruitSource = new FruitSource(this);
            TableView.Source = fruitSource;

            var sdc = searchDisplayController;
            sdc.SearchResultsSource = fruitSource;
            sdc.SearchBar.TextChanged += (sender, e) =>
                {
                    string text = e.SearchText.Trim();
                    filteredFruits = (from fruit in fruits
                        where fruit.Name.ToUpper().Contains(text.ToUpper()) ||
                        fruit.ColorName.ToUpper().Contains(text.ToUpper())
                        select fruit).ToList();
                };
        }

        // data source
        private class FruitSource:UITableViewSource
        {
            public static readonly NSString CellId = new NSString("fruitCell");

            private FruitTableViewController fruitController;
            private UISearchDisplayController search;

            public FruitSource(FruitTableViewController fruitController)
            {
                this.fruitController = fruitController;
                search = fruitController.searchDisplayController;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return GetFruit().Count;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(CellId);
                cell = cell ?? new UITableViewCell(UITableViewCellStyle.Value1, CellId);

                Fruit fruit = GetFruit()[indexPath.Row];
                cell.TextLabel.Text = fruit.Name;
                cell.DetailTextLabel.Text = fruit.ColorName;

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var detailController = new UIViewController();
                detailController.View.BackgroundColor = UIColor.White;
                detailController.Title = GetFruit()[indexPath.Row].Name;
                fruitController.NavigationController.PushViewController(detailController, true);
            }

            private List<Fruit> GetFruit()
            {
                return search.Active ? fruitController.filteredFruits : fruitController.fruits;
            }
        }
    }
}
