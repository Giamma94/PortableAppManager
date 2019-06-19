using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Reflection;

namespace PortableAppManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Thread TH;
        bool runs;

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;

        private System.Windows.Forms.NotifyIcon MyNotifyIcon;

        public MainWindow()
        {
            InitializeComponent();

            ShowNotify();

            TH = new Thread(Keyboardd);
            TH.SetApartmentState(ApartmentState.STA);
            runs = true;
            TH.Start();

            CollectData();

        }
              
        void CollectData()
        {

            MainWP.Children.Clear();

            List<string> filePath = new List<string>();

            string AppsDir = System.AppDomain.CurrentDomain.BaseDirectory + @"\PorableAPPS";
            string[] subdirectoryEntries = Directory.GetDirectories(AppsDir);

            foreach (string item in subdirectoryEntries)
            {
                filePath.AddRange(Directory.GetFiles(item, "*.exe"));
            }

            List<Application_UC_Data> Dat = new List<Application_UC_Data>();

            Dat = GetPortableApps(filePath);

            foreach (Application_UC_Data item in Dat)
            {
                MainWP.Children.Add(new Component.Application_UC(item.ItemIcon, item.ItemText));
            }

        }

        private List<Application_UC_Data> GetPortableApps(List<string> dir)
        {
            List<Application_UC_Data> data = new List<Application_UC_Data>(); 

            foreach (string filePath in dir)
            {
                var sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
                var bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                            sysicon.Handle,
                            System.Windows.Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                sysicon.Dispose();

                var itm = new Application_UC_Data() { ItemIcon = bmpSrc, ItemText = filePath };

                data.Add(itm);
            }

            return data;

        }

        void Keyboardd()
        {

            while (runs)
            {
                Thread.Sleep(200);
                if ((Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) > 0)
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {

                        this.Visibility = Visibility.Visible;

                        CollectData();

                    }));

                }

            }

        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        public void ShowNotify()
        {

            MyNotifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "Data Collector",
                Visible = true
            };

            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1, this.menuItem2 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize menuItem2
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "Show";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            MyNotifyIcon.ContextMenu = this.contextMenu1;

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            runs = false;
            Thread.Sleep(20);
            this.Close();
        }


    }
}
