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
using System.Diagnostics;
using System.IO;

namespace PortableAppManager.Component
{
    /// <summary>
    /// Interaction logic for Application_UC.xaml
    /// </summary>
    public partial class Application_UC : UserControl
    {

        string FilePath;

        public Application_UC(BitmapSource ItemIcon, string Name)
        {
            InitializeComponent();

            FilePath = Name;
        
            NameSW.Content = Path.GetFileName(Name).Replace(".exe", "").Replace("Portable", "");

            Img.Source = ItemIcon;

        }

        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Process.Start(FilePath);

        }
    }
}
