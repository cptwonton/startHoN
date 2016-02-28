using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String[] skipDirs = { "Documents and Settings" };
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives) 
            {
                Logger("checking drive " + d.ToString());
               DirectoryInfo dInfo = d.RootDirectory;
               DirectoryInfo[] dArray = dInfo.GetDirectories(); 
                foreach(DirectoryInfo directory in dArray)
                {
                    Logger("Checking Directory: " + directory.ToString());
                    if (directory.Name.Contains("$") || skipDirs.Contains(directory.Name)) 
                    {
                        Logger("Skipping Hidden File");
                        continue;
                    }
                    Logger("Calling hasHon on: " + directory.ToString());
                    hasHon(directory);
                }

            }
            MessageBox.Show("derps");

        }

        private void hasHon(DirectoryInfo dirToScan) 
        {
            Logger("Entered hasHon on: " + dirToScan.ToString());
            FileInfo[] fArray = dirToScan.GetFiles();
            foreach (FileInfo fInfo in fArray) 
            {
                Logger("Checking File: " + fInfo.ToString());
                if (fInfo.Name == "hon.exe") 
                {
                    Logger("Found Hon at: " + fInfo.FullName);
                    String HonPath = fInfo.FullName;
                    MessageBox.Show("Found Hon at: " + HonPath);
                    if (StartHon.IsChecked == true) 
                    {
                        ProcessStartInfo start = new ProcessStartInfo();
                        start.Arguments = null;
                        start.FileName = fInfo.FullName;
                        start.WindowStyle = ProcessWindowStyle.Hidden;
                        start.CreateNoWindow = true;
                        int exitCode;

                        using (Process proc = Process.Start(start)) 
                        {
                            proc.WaitForExit();

                            exitCode = proc.ExitCode;
                        }
                        
                    }
                    return;
                }
            }

            Logger("Did not find Hon.exe within " + dirToScan.ToString());
            DirectoryInfo[] subDir = dirToScan.GetDirectories();
            if (subDir.Count() == 0) 
            {
                Logger("No Subdirectories within " + dirToScan.ToString());
                return;
            }
            
            foreach (DirectoryInfo _dInfo in subDir) 
            {
                
                if (_dInfo == null) 
                {
                    return;
                }
                Logger("Calling hasHon on subdir: " + _dInfo.ToString());
                hasHon(_dInfo);
            }
        }

        public void Logger(String lines)
        {

            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\test.txt", true);
            //file.WriteLine(lines);

            //file.Close();

        }

        private void StartHon_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
