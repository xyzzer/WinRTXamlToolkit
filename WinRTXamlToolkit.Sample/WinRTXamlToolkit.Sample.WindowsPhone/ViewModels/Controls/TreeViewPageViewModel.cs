using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Imaging;
using WinRTXamlToolkit.Sample.Common;
using WinRTXamlToolkit.Tools;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Sample.ViewModels.Controls
{
    public class TreeViewPageViewModel : BindableBase
    {
        #region TreeItems
        private ObservableCollection<TreeItemViewModel> _treeItems;
        public ObservableCollection<TreeItemViewModel> TreeItems
        {
            get { return _treeItems; }
            set { this.SetProperty(ref _treeItems, value); }
        }
        #endregion

        private int _branchId;
        private Random _rand = new Random();
        private List<Color> _namedColors = ColorExtensions.GetNamedColors();

        public TreeViewPageViewModel()
        {
            _branchId = 1;
            TreeItems = BuildTree(5, 5);
        }

        private ObservableCollection<TreeItemViewModel> BuildTree(int depth, int branches)
        {
            var tree = new ObservableCollection<TreeItemViewModel>();

            if (depth > 0)
            {
                var depthIndices = Enumerable.Range(0, branches).Shuffle();

                for (int i = 0; i < branches; i++)
                {
                    var d = depthIndices[i] % depth;
                    var b = _rand.Next(branches / 2, branches);

                    tree.Add(
                        new TreeItemViewModel
                        {
                            Text = "Branch " + _branchId++,
                            Brush = new SolidColorBrush(_namedColors[_rand.Next(0, _namedColors.Count)]),
                            Children = BuildTree(d, b)
                        });
                }
            }

            return tree;
        }
    }

    public class TreeItemViewModel : BindableBase
    {
        #region Text
        private string _text;
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { this.SetProperty(ref _text, value); }
        }
        #endregion

        #region Children
        private ObservableCollection<TreeItemViewModel> _children = new ObservableCollection<TreeItemViewModel>();
        /// <summary>
        /// Gets or sets the child items.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return _children; }
            set { this.SetProperty(ref _children, value); }
        }
        #endregion

        #region Brush
        private SolidColorBrush _brush;
        /// <summary>
        /// Gets or sets the brush.
        /// </summary>
        public SolidColorBrush Brush
        {
            get { return _brush; }
            set { this.SetProperty(ref _brush, value); }
        }
        #endregion
    }

    public class TreeItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BranchTemplate { get; set; }
        public DataTemplate LeafTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var treeItem = (TreeItemViewModel)item;

            if (treeItem.Children == null ||
                treeItem.Children.Count == 0)
            {
                return this.LeafTemplate;
            }
            else
            {
                return this.BranchTemplate;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return this.SelectTemplateCore(item);
        }
    }
}
