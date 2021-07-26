
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommandHelper;
using System.Windows.Input;
using System.IO;
using FileExplorer.SQL;
using FileExplorer.Objects;
using Microsoft.Toolkit;
using GalaSoft.MvvmLight.Command;

namespace FileExplorer.ViewModels.Commands
{
    class Command : INotifyPropertyChanged
    {
        public string _value;

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _eventScanButton;
        private ICommand _eventSearchButton;
        private ICommand _eventOpenFileButton;

        Scanner Scanner1 = new Scanner();
        Connection connector = new Connection();



        public Command()
        {

        }

        private void notifyPropertyChanged(string propname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        private void RaisePropertyChanged(Func<string> p)
        {
            throw new NotImplementedException();
        }


        /**
         * Gives Back the Value of the TextBox (SearchBox) on SearchButton Click
         */
        public string SearchBoxText
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                notifyPropertyChanged("SearchBoxText");
            }
        }




        #region Events

        /**
         * Command for the "Scan" Button
         */
        public ICommand EventScanButton
        {
            get
            {
                _eventScanButton = new CommandHelper.RelayCommand(c => ScanData());
                return _eventScanButton;
            }
        }

        /**
         * Command for the "Search" Button
         */
        public ICommand EventSearchButton
        {
            get
            {
                _eventSearchButton = new CommandHelper.RelayCommand(c => SearchFiles());
                return _eventSearchButton;
            }
        }


        /**
         * Command for the "Open" Button
         */
        public ICommand EventOpenFileButton
        {
            get
            {
                _eventOpenFileButton = new RelayCommand<string>(openFile);
                return _eventOpenFileButton;
            }
        }

        #endregion


        #region Methods

        /**
         * Scans the Children Directories and Files of the FilePath
         * @return Return a Boolean Value
         */
        public bool ScanData()
        {
            string BasePath;
            
            BasePath = ((MainWindow)System.Windows.Application.Current.MainWindow).getSelectedItem();
            if (BasePath != null) { 
                Scanner1.Scan(BasePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * Getting all Files from the DB by the Search Criteria and Filling them in a List
         */
        public void SearchFiles()
        {
            List<EFile> Files = new List<EFile>();
            Console.WriteLine("Search Value : " + _value);
            Files = connector.searchFiles(_value);
            ((MainWindow)System.Windows.Application.Current.MainWindow).addSearchedItems(Files);
        }

        /**
         * Opens the File that is clicked
         */
        public void openFile(string Path)
        {
            System.Diagnostics.Process.Start(@Path);
        }

        #endregion
    }
}
