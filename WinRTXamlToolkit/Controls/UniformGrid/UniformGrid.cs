using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    public class UniformGrid : Panel
    {
        /// <summary>
        /// Identifies the Columns dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the Rows dependency property.
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                "Rows",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0));

        /// <summary>
        /// Identifies the FirstColumn dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstColumnProperty =
            DependencyProperty.Register(
                "FirstColumn",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0));

        private int _columns;
        private int _rows;

        /// <summary>
        /// Gets or sets the number of columns that are in the grid.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of leading blank cells in the first row of the grid.
        /// </summary>
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of rows that are in the grid.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var finalRect = new Rect(
                0.0,
                0.0,
                arrangeSize.Width / _columns,
                arrangeSize.Height / _rows);
            var width = finalRect.Width;
            var num2 = arrangeSize.Width - 1.0;
            finalRect.X += finalRect.Width * this.FirstColumn;

            foreach (var element in Children)
            {
                element.Arrange(finalRect);

                if (element.Visibility != Visibility.Collapsed)
                {
                    finalRect.X += width;

                    if (finalRect.X >= num2)
                    {
                        finalRect.Y += finalRect.Height;
                        finalRect.X = 0.0;
                    }
                }
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UpdateComputedValues();
            var availableSize = new Size(constraint.Width / (_columns), constraint.Height / (_rows));
            var width = 0.0;
            var height = 0.0;
            var num3 = 0;
            var count = Children.Count;

            while (num3 < count)
            {
                var element = Children[num3];
                element.Measure(availableSize);
                var desiredSize = element.DesiredSize;

                if (width < desiredSize.Width)
                {
                    width = desiredSize.Width;
                }

                if (height < desiredSize.Height)
                {
                    height = desiredSize.Height;
                }

                num3++;
            }

            return new Size(width * _columns, height * _rows);
        }

        private void UpdateComputedValues()
        {
            _columns = this.Columns;
            _rows = this.Rows;

            if (this.FirstColumn >= _columns)
            {
                this.FirstColumn = 0;
            }

            if ((_rows == 0) || (_columns == 0))
            {
                var num = 0;
                var num2 = 0;

                var count = Children.Count;

                while (num2 < count)
                {
                    var element = Children[num2];

                    if (element.Visibility != Visibility.Collapsed)
                    {
                        num++;
                    }

                    num2++;
                }

                if (num == 0)
                {
                    num = 1;
                }

                if (_rows == 0)
                {
                    if (_columns > 0)
                    {
                        _rows = ((num + this.FirstColumn) + (_columns - 1)) / _columns;
                    }
                    else
                    {
                        _rows = (int)Math.Sqrt(num);

                        if ((_rows * _rows) < num)
                        {
                            _rows++;
                        }

                        _columns = _rows;
                    }
                }
                else if (_columns == 0)
                {
                    _columns = (num + (_rows - 1)) / _rows;
                }
            }
        }
    }
}