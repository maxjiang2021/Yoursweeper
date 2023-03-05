using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Yoursweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    Image f = new Image();
                    f.Source = new BitmapImage(new Uri("unknown.png", UriKind.Relative));
                    f.SetValue(Grid.RowProperty, x);
                    f.SetValue(Grid.ColumnProperty, y);
                    MineView.Children.Add(f);
                }
            }
        }

        byte mode = 0;
        const byte ALLMINE = 0;
        const byte ALLHITMINE = 1;
        const byte ALLHITSINGLEMINE = 2;
        const byte NOMINES = 3;
        const byte ONEMINE = 4;
        const byte ALLEXCEPTONEMINE = 5;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Image child in MineView.Children.OfType<Image>())
            {
                child.Source = new BitmapImage(new Uri("unknown.png", UriKind.Relative));
            }
            MineView.IsEnabled = true;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Status.Source = new BitmapImage(new Uri("hold_reset.png", UriKind.Relative));
        }

        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Status.Source = new BitmapImage(new Uri("running.png", UriKind.Relative));
        }

        Image? lastImg = null;
        bool isMouseDown = false;
        Random r = new Random();

        private void MineView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(Mouse.DirectlyOver is Image)) return;
            isMouseDown = true;
            Status.Source = new BitmapImage(new Uri("choose.png", UriKind.Relative));
            if (lastImg != null)
                lastImg.Source = new BitmapImage(new Uri("unknown.png", UriKind.Relative));
            lastImg = (Image)Mouse.DirectlyOver;
            lastImg.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
        }

        private void MineView_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (lastImg == null || !MineView.IsEnabled) return;
            lastImg.Source = new BitmapImage(new Uri("unknown.png", UriKind.Relative));
            isMouseDown = false;
            lastImg.Source = new BitmapImage(new Uri("hit_mine.png", UriKind.Relative));
            if (mode == ALLMINE)
            {
                foreach (Image child in MineView.Children.OfType<Image>())
                {
                    if (child != lastImg)
                        child.Source = new BitmapImage(new Uri("mine.png", UriKind.Relative));
                }
            }
            if (mode == ALLHITMINE)
            {
                foreach (Image child in MineView.Children.OfType<Image>())
                {
                    if (r.NextDouble() < 0.1 && child != lastImg)
                    {
                        child.Source = new BitmapImage(new Uri("mine.png", UriKind.Relative));
                    }
                }
            }
            Status.Source = new BitmapImage(new Uri("die.png", UriKind.Relative));
            if (mode == ONEMINE)
            {
                lastImg.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
                int i = r.Next() % (16 * 16);
                int n = 0;
                foreach (Image child in MineView.Children.OfType<Image>())
                {
                    if (child != lastImg)
                        child.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
                    n++;
                    if (i == n)
                    {
                        if (child == lastImg) i++;
                        else
                        {
                            child.Source = new BitmapImage(new Uri("mine.png", UriKind.Relative));
                        }
                    }
                }
                Status.Source = new BitmapImage(new Uri("win.png", UriKind.Relative));
            }
            if (mode == NOMINES)
            {
                foreach (Image child in MineView.Children.OfType<Image>())
                {
                    child.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
                }
                Status.Source = new BitmapImage(new Uri("win.png", UriKind.Relative));
            }
            if (mode == ALLEXCEPTONEMINE)
            {
                foreach (Image child in MineView.Children.OfType<Image>())
                {
                    child.Source = new BitmapImage(new Uri("mine.png", UriKind.Relative));
                }
                lastImg.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
                Status.Source = new BitmapImage(new Uri("win.png", UriKind.Relative));
            }
            MineView.IsEnabled = false;
        }

        private void MineView_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.DirectlyOver is Image)
            {
                if (isMouseDown)
                {
                    if (lastImg != null)
                        lastImg.Source = new BitmapImage(new Uri("unknown.png", UriKind.Relative));
                    lastImg = (Image)Mouse.DirectlyOver;
                    lastImg.Source = new BitmapImage(new Uri("empty.png", UriKind.Relative));
                }
            }
        }

        private void am_Click(object sender, RoutedEventArgs e)
        {
            mode = ALLMINE;
            ahm.IsChecked = false;
            ahsm.IsChecked = false;
            om.IsChecked = false;
            nob.IsChecked = false;
            eom.IsChecked = false;
        }

        private void ahm_Click(object sender, RoutedEventArgs e)
        {
            mode = ALLHITMINE;
            am.IsChecked = false;
            ahsm.IsChecked = false;
            om.IsChecked = false;
            nob.IsChecked = false;
            eom.IsChecked = false;
        }

        private void ahsm_Click(object sender, RoutedEventArgs e)
        {
            mode = ALLHITSINGLEMINE;
            ahm.IsChecked = false;
            am.IsChecked = false;
            om.IsChecked = false;
            nob.IsChecked = false;
            eom.IsChecked = false;
        }

        private void nob_Click(object sender, RoutedEventArgs e)
        {
            mode = NOMINES;
            ahm.IsChecked = false;
            am.IsChecked = false;
            om.IsChecked = false;
            ahsm.IsChecked = false;
            eom.IsChecked = false;
        }

        private void om_Click(object sender, RoutedEventArgs e)
        {
            mode = ONEMINE;
            ahm.IsChecked = false;
            am.IsChecked = false;
            nob.IsChecked = false;
            ahsm.IsChecked = false;
            eom.IsChecked = false;
        }

        private void eom_Click(object sender, RoutedEventArgs e)
        {
            mode = ALLEXCEPTONEMINE;
            ahm.IsChecked = false;
            am.IsChecked = false;
            nob.IsChecked = false;
            ahsm.IsChecked = false;
            om.IsChecked = false;
        }
    }
}
