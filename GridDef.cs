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

                    var rowDefinitions = definitions.Select(row => new RowDefinition()
                    {
                        Height = (GridLength)converter.ConvertFromString(row)
                    });

                    foreach (var def in rowDefinitions)
                        grid.RowDefinitions.Add(def);
                }
            }
        }
    }
}
