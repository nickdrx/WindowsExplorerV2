using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FileExplorer.ShellClasses;
using FileExplorer.Objects;
using FileExplorer.ViewModels;
using System.Windows.Controls;
using FileExplorer.ViewModels.Commands;
using System.Windows.Data;
using System.Reflection;

namespace FileExplorer
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeFileSystemObjects();

        }

        #region Events

        private void FileSystemObject_AfterExplore(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void FileSystemObject_BeforeExplore(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Wait;
        }

        #endregion

        #region Methods

        /**
         ***        Getting the Path from the Selected Item in the Treeview 
         * @return  Returns the Path
         */
        public string getSelectedItem()
        {
            string Path = null;
            var _Selected_Item = treeView.SelectedItem;

            var _Parsed_Selected_Item = (FileSystemObjectInfo)_Selected_Item;
            var Final_SelectedItem = _Parsed_Selected_Item.FileSystemInfo;

            Path = Final_SelectedItem.FullName;


            return Path;
        }

        /**
        ***        Initialize all Files and Directories of the Drives and adding them into a treeView
        */
        private void InitializeFileSystemObjects()
        {
            var drives = DriveInfo.GetDrives();

            DriveInfo
                .GetDrives()
                .ToList()
                .ForEach(drive =>
                {
                    var fileSystemObject = new FileSystemObjectInfo(drive);
                    fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                    fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                    treeView.Items.Add(fileSystemObject);
                });




        }

        /**
         ***    Adding all found Items from the Search Criteria into a ListBox
         ***    Creates A Button for every File that opens it through a Binding
         * @params Files: All found Files in a List from the Type EFile
         * @return Returns the ListBox
         */
        public void addSearchedItems(List<EFile> Files)
        {
            SearchList.Items.Clear();
            Console.WriteLine(SearchList);

            foreach (var element in Files)
            {
                SearchList.Items.Add(createItem(element));
            }

        }

        public ListBoxItem createItem(EFile File)
        {

            var ItemHeader = new TextBlock()
            {
                FontSize = 20,
                Text = File.Path
            };

            var ItemValue = new TextBlock()
            {
                FontSize = 16,
                Text = File.Filevalue.Substring(0, 80)
            };

            var ItemName = new TextBlock()
            {
                Text = File.Filename
            };
            var ItemButton = new Button()
            {

                Width = 100,
                Content = "Open"

            };


            ItemButton.CommandParameter = File.Path;
            ItemButton.SetBinding(Button.CommandProperty, new Binding("EventOpenFileButton"));




            var newStackPanel = new StackPanel() { Orientation = Orientation.Vertical, Width = 843.33 };

            newStackPanel.Children.Add(ItemHeader);
            newStackPanel.Children.Add(ItemValue);
            newStackPanel.Children.Add(ItemButton);


            return new ListBoxItem() { Content = newStackPanel };

        }
        #endregion
    }
}
