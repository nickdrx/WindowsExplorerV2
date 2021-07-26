using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visualis.Extractor;
using iTextSharp;
using System.IO;
using System.ComponentModel;
using System.Windows;
using FileExplorer.SQL;
using FileExplorer.Objects;
using FileExplorer.Helper;

namespace FileExplorer.ViewModels
{
    public class Scanner
    {
        Connection connector = new Connection();
        DirHelper Helper = new DirHelper();


        public static TextExtractorD extractor = new TextExtractorD();
        public static int n = 0;

        public static Dictionary<DirectoryInfo, long> Dir = new Dictionary<DirectoryInfo, long>();

        List<EFile> Files = new List<EFile>();



        public Scanner()
        {
        }

        public void getAllFiles(string directoryPath)
        {
            DirectoryInfo sd = new DirectoryInfo(directoryPath);

            EFile File;

            foreach (FileInfo fi in sd.GetFiles())
            {

                File = new EFile(fi.Name, extractor.Extract(fi.FullName), fi.FullName);

                Files.Add(File);
            }

            List<string> directories = Helper.GetDirectories(directoryPath);

            for(int i = 0; i < directories.Count; i++)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(directories[i]);

                foreach (FileInfo fi in DirInfo.GetFiles())
                {

                    File = new EFile(fi.Name, extractor.Extract(fi.FullName), fi.FullName);

                    Files.Add(File);
                }
            }

        }



        public void Scan(string basePath)
        {
            
            Files.Clear();

            getAllFiles(basePath);

            connector.uploadFiles(Files, basePath);
        }
    }
}
