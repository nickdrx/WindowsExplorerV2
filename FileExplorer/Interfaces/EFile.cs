
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Objects
{
    
    public class EFile
    {
        public string Filename { get; set; }
        public string Filevalue { get; set; }
        public string Path { get; set; }

        /**
        * File Object that is send to the Database
        * @params filename The Name of the File
        * @params filevalue The Content of the File
        * @params path The Filepath of the File
        */
        public EFile( string filename, string filevalue, string path)
        {
            Filename = filename;
            Filevalue = filevalue;
            Path = path;
        }

    }
}
