using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace reName
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string extension = "";
        string prepend = "";
        string append = "";
        string filter;
        string oldValue;
        string newValue = "";
        string zipIt = null;
        string location;
        string[] childItems;


        public MainWindow()
        {
            InitializeComponent();
            renameClick.Click += Rename_Click;
        }

        void Rename_Click(object sender, RoutedEventArgs e)
        {
            extensionBox.IsEnabled = false;
            prependBox.IsEnabled = false;
            appendBox.IsEnabled = false;
            locationBox.IsEnabled = false;
            oldBox.IsEnabled = false;
            newBox.IsEnabled = false;
            filterBox.IsEnabled = false;
            zipBox.IsEnabled = false;


            extension = extensionBox.Text;
            prepend = prependBox.Text;
            append = appendBox.Text;
            location = locationBox.Text;
            oldValue = oldBox.Text;
            newValue = newBox.Text;
            filter = filterBox.Text;
            zipIt = zipBox.Text;

            if (location != null && Directory.Exists(location) && location != "")
            {

                if (oldValue != null && oldValue != "")
                {
                    childItems = Directory.GetFiles(@location, "*", SearchOption.TopDirectoryOnly);
                    foreach (string file in childItems)
                    {
                        if (file.Contains(oldValue) && (filter == null || filter == ""))
                        {
                            File.Move(file, file.Replace(oldValue, newValue));
                        }
                        else if (file.Contains(oldValue) && (filter != null && filter != ""))
                        {
                            if (file.Contains(filter))
                            {
                                File.Move(file, file.Replace(oldValue, newValue));
                            }
                        }
                    }
                }

                childItems = Directory.GetFiles(@location, "*", SearchOption.TopDirectoryOnly);
                if (extension != null && extension != "")
                {
                    if (extension.Contains("."))
                    {
                        extension = extension.Replace(".", "");
                    }
                    foreach (string file in childItems)
                    {
                        if (filter == null || filter == "")
                        {
                            File.Move(file, file.Replace(file.Split("\\").Last(), "") + prepend + file.Split("\\").Last().Split(".")[0] + append + "." + extension);
                        }
                        else if (filter != null && filter != "")
                        {
                            if (file.Contains(filter))
                            {
                                File.Move(file, file.Replace(file.Split("\\").Last(), "") + prepend + file.Split("\\").Last().Split(".")[0] + append + "." + extension);
                            }
                        }
                    }
                }
                else if (extension == null || extension == "")
                {
                    foreach (string file in childItems)
                    {
                        if (filter == null || filter == "")
                        {
                            File.Move(file, file.Replace(file.Split("\\").Last(), "") + prepend + file.Split("\\").Last().Split(".")[0] + append + "." + file.Split(".").Last());
                        }
                        else if (filter != null && filter != "")
                        {
                            if (file.Contains(filter))
                            {
                                File.Move(file, file.Replace(file.Split("\\").Last(), "") + prepend + file.Split("\\").Last().Split(".")[0] + append + "." + file.Split(".").Last());
                            }
                        }
                    }
                }

                if (zipIt != null && zipIt != "")
                {
                    if (filter == null || filter == "")
                    {
                        if (!location.EndsWith("/") || !location.EndsWith("\\"))
                        {
                            location += "\\";
                        }
                        if (!File.Exists(System.IO.Path.GetTempPath().ToString() + zipIt + ".zip"))
                        {
                            ZipFile.CreateFromDirectory(location, System.IO.Path.GetTempPath().ToString() + zipIt + ".zip");
                            File.Move(System.IO.Path.GetTempPath().ToString() + zipIt + ".zip", location + zipIt + ".zip");
                        }
                        else
                        {
                            while (true)
                            {
                                int rand = new Random().Next(10000, 40000);
                                if (!File.Exists(System.IO.Path.GetTempPath().ToString() + zipIt + rand + ".zip"))
                                {
                                    ZipFile.CreateFromDirectory(location, System.IO.Path.GetTempPath().ToString() + zipIt + rand + ".zip");
                                    File.Move(System.IO.Path.GetTempPath().ToString() + zipIt + rand + ".zip", location + zipIt + ".zip");
                                    break;
                                }
                            }
                        }
                    }
                    else if (filter != null && filter != "")
                    {
                        childItems = Directory.GetFiles(@location, "*", SearchOption.TopDirectoryOnly);
                        if (!location.EndsWith("/") || !location.EndsWith("\\"))
                        {
                            location += "\\";
                        }

                        while (true)
                        {
                            int rand = new Random().Next(10000, 40000);
                            if (!Directory.Exists(System.IO.Path.GetTempPath().ToString() + rand))
                            {
                                Directory.CreateDirectory(System.IO.Path.GetTempPath().ToString() + rand);
                                foreach (string file in childItems)
                                {
                                    if (file.Contains(filter))
                                    {
                                        File.Copy(file, System.IO.Path.GetTempPath().ToString() + rand + file.Split("\\").Last());
                                    }
                                }
                                ZipFile.CreateFromDirectory(System.IO.Path.GetTempPath().ToString() + rand, location + zipIt + ".zip");
                                Directory.Delete(System.IO.Path.GetTempPath().ToString() + rand, true);
                                break;
                            }
                        }
                    }
                }
            }

                extensionBox.Text = null;
                appendBox.Text = null;
                prependBox.Text = null;
                oldBox.Text = null;
                newBox.Text = null;
                filterBox.Text = null;
                zipBox.Text = null;

                extensionBox.IsEnabled = true;
                prependBox.IsEnabled = true;
                appendBox.IsEnabled = true;
                locationBox.IsEnabled = true;
                oldBox.IsEnabled = true;
                newBox.IsEnabled = true;
                filterBox.IsEnabled = true;
                zipBox.IsEnabled = true;
            
        }
    }
}