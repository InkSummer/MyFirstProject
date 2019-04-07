using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic.Devices;

namespace SelectFile
{
    /// <summary>
    /// FolderChange.xaml 的交互逻辑
    /// </summary>
    public partial class FolderChange : Window
    {
        private string filePath = null;
        private static Thread subthread;
        private static bool IsFolder = false;
        private static string DelText;
        private static string CopyText;

        public FolderChange()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            subthread = new Thread(new ThreadStart(ChangeName));
            filePath = folder.Path;
            subthread.Start();

            while (!subthread.IsAlive)
                Thread.Sleep(1);
        }

        private void ChangeName()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                //DelText = Del.Text;
                //CopyText = Copy.Text;
            }));


            Computer MyComputer = new Computer();

            if (IsFolder)
            {
                var folders = Directory.GetDirectories(filePath);
                foreach (var folder in folders)
                {
                    try
                    {
                        var oldname = System.IO.Path.GetDirectoryName(folder);
                        //var newname = oldname.Replace(DelText, CopyText);
                        //newname = GetNewFolder(filePath, newname);
                        var newname = "[HERESY] エッチなマシュの人";
                        if (newname != null)
                        {
                            MyComputer.FileSystem.RenameDirectory(filePath + oldname, newname);
                            WriteLable(oldname + " 已重命名：" + newname);
                        }
                        else
                        {
                            WriteLable(folder + " 已存在");
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLable(folder + " 处理出现问题：" + ex.Message);
                    }
                }

            }
            else
            {
                var files = Directory.GetFiles(filePath, ".", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        var file_name = System.IO.Path.GetFileName(file.ToString());
                        if (file_name.IndexOf(DelText) > 0)
                        {
                            file_name = file_name.Replace(DelText, CopyText);
                            file_name = GetNewName(filePath + "\\", file_name);

                            if (file_name != null)
                            {
                                File.Move(file.ToString(), filePath + "\\" + file_name);
                                WriteLable(file_name + " 已重命名");
                            }
                            else
                            {
                                WriteLable(file_name + " 已存在");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLable(file + " 处理出现问题：" + ex.Message);
                    }
                }
            }

        }

        private string GetNewName(string path, string newfile, int i = 1)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(newfile);
            string type = System.IO.Path.GetExtension(newfile);
            if (File.Exists(path + newfile))
            {
                string returnname = name + "(" + i++ + ")" + type;
                if (File.Exists(path + returnname))
                    GetNewName(path, newfile, i);
                else
                    return returnname;
            }
            return newfile;
        }

        private String GetNewFolder(string Path, string name, int i = 0)
        {
            if (Directory.Exists(Path + name))
            {
                string newname = name + "(" + i++ + ")";
                if (Directory.Exists(Path + newname))
                    return GetNewFolder(Path, name, i);
                else
                    return name;
            }
            return name;
        }

        private void WriteLable(string text)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (Text.Text.Length > 2000) Text.Text.Remove(0, Text.Text.IndexOf('\n'));
                Text.Text = text + "\r\n" + Text.Text;
            }));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IsFolder = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IsFolder = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var Edit = new EditRule();
            Edit.Show();

        }

        List<CheckBox> headerChecks = new List<CheckBox>();


        /// <summary>
        /// 由于不是太好获取就把控件先放到内存中缓存下来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Loaded_1(object sender, RoutedEventArgs e)
        {
            CheckBox cbtemp = (CheckBox)sender;

            headerChecks.Add(cbtemp);
        }

        private void CheckBox_Click_3(object sender, RoutedEventArgs e)
        {
            headerChecks.ForEach(a => a.IsChecked = true);
        }

    }
}
