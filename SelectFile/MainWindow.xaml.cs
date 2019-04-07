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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SelectFile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath = null;
        private static Thread subthread;
        private static bool IsCheck = true;
        private static bool IsOpen = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            subthread = new Thread(new ThreadStart(ChangePic));
            filePath = folder.Path;
            subthread.Start();

            while (!subthread.IsAlive)
                Thread.Sleep(1);
        }

        private void ChangePic()
        {
            if (!Directory.Exists(filePath))
            {
                WriteLable("没有找到文件夹");
                return;
            }
            var filePath_ZM = filePath + "\\桌面壁纸";
            if (!Directory.Exists(filePath_ZM))
            {
                Directory.CreateDirectory(filePath_ZM);
                WriteLable("创建桌面壁纸文件夹：" + filePath_ZM);
            }

            var filePath_SJ = filePath + "\\手机壁纸";
            if (!Directory.Exists(filePath_SJ))
            {
                Directory.CreateDirectory(filePath_SJ);
                WriteLable("创建手机壁纸文件夹：" + filePath_SJ);
            }

            WriteLable("***************************************************");
            WriteLable("开始处理文件");
            var images = new List<System.String>(Directory.GetFiles(filePath, ".", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg") || s.EndsWith(".gif") || s.EndsWith(".bmp")));
            images = images.Where(q => (q.ToString().IndexOf(filePath_ZM) < 0) && (q.ToString().IndexOf(filePath_SJ) < 0)).ToList();

            Dispatcher.BeginInvoke(new Action(delegate
            {
                bar.Maximum = images.Count();
                bar.Minimum = 0;
                bar.Value = 0;
            }));
            int count = 0, sj = 0, zm = 0, error = 0;
            foreach (var i in images)
            {
                var file_name = System.IO.Path.GetFileName(i.ToString());
                try
                {
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        bar.Value += 1;
                    }));
                    System.Drawing.Image img = System.Drawing.Image.FromFile(i.ToString());
                    if (img.Width < img.Height * 1.3)
                    {
                        img.Dispose();
                        file_name = GetNewName(filePath_SJ + "\\", file_name);
                        string newfile = filePath_SJ + "\\" + file_name;

                        if (file_name != null)
                        {
                            File.Move(i.ToString(), newfile);
                            WriteLable(file_name + " 已移动到手机壁纸文件夹");
                            count++; sj++;
                        }
                        else
                        {
                            WriteLable(file_name + " 已存在于手机壁纸文件夹");
                            count++; error++;
                        }
                    }
                    else
                    {
                        img.Dispose();
                        file_name = GetNewName(filePath_ZM + "\\", file_name);
                        string newfile = filePath_ZM + "\\" + file_name;

                        if (file_name != null)
                        {
                            File.Move(i.ToString(), newfile);
                            WriteLable(file_name + " 已移动到桌面壁纸文件夹");
                            count++; zm++;
                        }
                        else
                        {
                            WriteLable(file_name + " 已存在于桌面壁纸文件夹");
                            count++; error++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLable(file_name + " 处理出现问题：" + ex.Message);
                    error++;
                }
                WriteCount(count, zm, sj, error);
            }

            WriteLable("处理已完成！");
            WriteLable("***************************************************");
            if (IsOpen)
            {
                System.Diagnostics.Process.Start("Explorer.exe", filePath);
            }
        }

        private string GetNewName(string peth, string newfile, int i = 1)
        {
            string name = newfile.Substring(0, newfile.LastIndexOf('.'));
            string type = newfile.Substring(newfile.LastIndexOf('.') + 1);
            if (File.Exists(peth + newfile))
            {
                if (IsCheck)
                {
                    string returnname = name + "(" + i++ + ")" + "." + type;
                    if (File.Exists(peth + returnname))
                        GetNewName(peth, newfile, i);
                    else
                        return returnname;
                }
                else
                    return null;
            }
            return newfile;
        }

        private string ChangeName(string name)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                name += DateTime.Today.ToLongDateString().Replace("-", "").Replace(".", "").Replace("年", "").Replace("月", "").Replace("日", "");
                if (!Directory.Exists(name))
                {
                    Directory.CreateDirectory(name);
                    WriteLable("创建桌面壁纸文件夹：" + name);
                }
                else { ChangeName(name); }

            }));
            return name;
        }

        private void WriteCount(int count, int zm, int sj, int error)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                string value = "计数：总数：" + count + "桌面壁纸数：" + zm + "手机壁纸数：" + sj + "错误：" + error + "";
                Count.Content = value;
            }));
        }

        private void WriteLable(string text)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                if (Text.Text.Length > 2000) Text.Text.Remove(2000, Text.Text.Length - 2000);
                Text.Text = text + "\r\n" + Text.Text;
            }));
        }

        private void Rename_Checked(object sender, RoutedEventArgs e)
        {
            IsCheck = true;
        }

        private void Open_Checked(object sender, RoutedEventArgs e)
        {
            IsCheck = false;
        }

        private void Rename_Unchecked(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void Open_Unchecked(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

    }
}
