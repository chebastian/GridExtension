using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GridExtension
{
    public class GridDef
    {


        public static string GetRows(DependencyObject obj)
        {
            return (string)obj.GetValue(RowsProperty);
        }

        public static void SetRows(DependencyObject obj, string value)
        {
            obj.SetValue(RowsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(GridDef), new PropertyMetadata("", OnChanged));
        private static bool loaded;
        private static string setOnInit;



        public static string GetColumns(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnsProperty);
        }

        public static void SetColumns(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnsProperty, value);
        }

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(GridDef), new PropertyMetadata("",OnColChanged));

        private static void OnColChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.ColumnDefinitions.Clear();
                if (e.NewValue is String args)
                {
                    var definitions = args.Split(",");
                    var converter = new GridLengthConverter();

                    var colDefinitions = definitions.Select(row => new ColumnDefinition()
                    {
                        Width = (GridLength)converter.ConvertFromString(row)
                    });

                    foreach (var def in colDefinitions)
                        grid.ColumnDefinitions.Add(def);
                }
            }
        }

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.RowDefinitions.Clear();
                if (e.NewValue is String args)
                {
                    var definitions = args.Split(",");
                    var converter = new GridLengthConverter();
                    if (definitions.Length == 1)
                    {
                        if (grid.ActualHeight == 0)
                        {
                            grid.SizeChanged += Grid_SizeChanged;
                            setOnInit = definitions.First();
                            return;
                        }
                        else
                        {
                            setOnInit = string.Empty;
                            grid.SizeChanged -= Grid_SizeChanged;
                        }

                        foreach (var child in grid.Children)
                            grid.RowDefinitions.Add(new RowDefinition()
                            {
                                Height = (GridLength)converter.ConvertFromString(definitions.First())
                            });
                    }

                    var rowDefinitions = definitions.Select(row => new RowDefinition()
                    {
                        Height = (GridLength)converter.ConvertFromString(row)
                    });

                    foreach (var def in rowDefinitions)
                        grid.RowDefinitions.Add(def);
                }
            }
        }

        private static void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.LayoutUpdated += Grid_LayoutUpdated;
                grid.RowDefinitions.Clear();

                var converter = new GridLengthConverter();
                foreach (var child in grid.Children)
                    grid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = (GridLength)converter.ConvertFromString(setOnInit)
                    });
            }
        }

        private static void Grid_LayoutUpdated(object sender, EventArgs e)
        {
            if (sender is Grid grid)
            {
                grid.RowDefinitions.Clear();

                var converter = new GridLengthConverter();
                foreach (var child in grid.Children)
                    grid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = (GridLength)converter.ConvertFromString(setOnInit)
                    });
            }
        }
    }
}
