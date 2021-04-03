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


        public static string GetRowDefinitions(DependencyObject obj)
        {
            return (string)obj.GetValue(RowDefinitionsProperty);
        }

        public static void SetRowDefinitions(DependencyObject obj, string value)
        {
            obj.SetValue(RowDefinitionsProperty, value);
        }

        // Using a DependencyProperty as the backing store for RowDefinitions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowDefinitionsProperty =
            DependencyProperty.RegisterAttached("RowDefinitions", typeof(string), typeof(GridDef), new PropertyMetadata("",OnChanged));



        public static string GetColumnDefinitions(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnDefinitionsProperty);
        }

        public static void SetColumnDefinitions(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnDefinitionsProperty, value);
        }

        // Using a DependencyProperty as the backing store for ColumnDefinitions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnDefinitionsProperty =
            DependencyProperty.RegisterAttached("ColumnDefinitions", typeof(string), typeof(GridDef), new PropertyMetadata("",OnColChanged));

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
