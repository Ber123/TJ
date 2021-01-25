using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using TJ.Models;
using TJ.Services;

namespace TJ
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\tjData.json";
        private readonly string PATHPICK= $"{Environment.CurrentDirectory}\\PicData\\";
        private BindingList<TjModel> _tjData;
        private FileIOService _fileIOService;
        private FileIOService _picIOService;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(PATH);

            try
            {
                _tjData = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            dgTraderTable.ItemsSource = _tjData;
            _tjData.ListChanged += _tjData_ListChanged;

            Uri startImgPath = new Uri($"{Environment.CurrentDirectory}\\PicData\\Default.png"); 
        }
        private void _tjData_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }
        private void FileDropBorder_Drop(object sender, DragEventArgs e)
        {
            _picIOService = new FileIOService(PATHPICK);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {

                var currentRowIndex = dgTraderTable.Items.IndexOf(dgTraderTable.CurrentItem);
                if (currentRowIndex > -1)
                {
                    if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    {
                        var border = (Border)sender;
                        var imgThumbnail = (Image)border.Child;
                        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                        string filename = Path.GetFileName(files[0]);
                        Uri filepath = new Uri(files[0]);

                        imgThumbnail.Source = new BitmapImage(filepath);
                        var imageName = PATHPICK+"#"+currentRowIndex+ imgThumbnail.Name+".bmp";

                        _fileIOService.SavePic((BitmapImage)imgThumbnail.Source, imageName);
                    }
                }
            }
        }
        private void SelectionChange_DG(object sender, SelectionChangedEventArgs e)
        {

            var currentRowIndex = dgTraderTable.Items.IndexOf(dgTraderTable.CurrentItem);
            _picIOService = new FileIOService(PATHPICK);
            DirectoryInfo folder = new DirectoryInfo(PATHPICK);
            Im10.Source = null;
            Im11.Source = null;
            Im20.Source = null;
            Im21.Source = null;
            Im30.Source = null;
            Im31.Source = null;

            try
            {
                foreach (FileInfo o in folder.GetFiles())       //
                {

                    string name = o.Name;
                    if (name.StartsWith("#"))
                    {
                        if (o.Name.StartsWith("#"))
                        {
                            
                            var newName = o.Name.Replace("#", "");
                            if (File.Exists(PATHPICK + newName))
                            {
                                File.Delete(PATHPICK + newName);
                            }

                            File.Copy(PATHPICK+o.Name, PATHPICK+ newName);
                            File.Delete(PATHPICK+o.Name);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            Im10.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im10.bmp");
            Im11.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im11.bmp");
            Im20.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im20.bmp");
            Im21.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im21.bmp");
            Im30.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im30.bmp");
            Im31.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im31.bmp");

        }

        private void MouseLeftButtonDown_Border(object sender, MouseButtonEventArgs e)
        {
            var MinWidth = 700;
            var border = (Border)sender;
            var img = (Image)border.Child;
            BitmapImage imgBkp = (BitmapImage)Im31.Source;


            var rowIndex = Convert.ToInt32(img.Name.Substring(2, 1));
            var colIndex = Convert.ToInt32(img.Name.Substring(3, 1));
            var grid = (Grid)border.Parent;
            var currentRowIndex = dgTraderTable.Items.IndexOf(dgTraderTable.CurrentItem);


            if (border.ActualWidth < MinWidth)
            {

                Grid.SetColumn(bLast, 0);
                Grid.SetRow(bLast, 1);
                Grid.SetColumnSpan(bLast, 2);
                Grid.SetRowSpan(bLast, 3);
                Im31.Source = img.Source;
            }
            else
            {
                Grid.SetColumn(bLast, 1);
                Grid.SetRow(bLast, 3);
                Grid.SetColumnSpan((Border)sender, 1);
                Grid.SetRowSpan((Border)sender, 1);
                Im31.Source = _picIOService.LoadPic(PATHPICK + currentRowIndex + "Im31.bmp");
            }
        }

    }

    public class ConvertItemToIndex : IValueConverter
    {
        #region IValueConverter Members
        //Convert the Item to an Index
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //Get the CollectionView from the DataGrid that is using the converter
                DataGrid dg = (DataGrid)Application.Current.MainWindow.FindName("dgTraderTable");
                CollectionView cv = (CollectionView)dg.Items;
                //Get the index of the item from the CollectionView
                int rowindex = cv.IndexOf(value) + 1;

                return rowindex.ToString();
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
        //One way binding, so ConvertBack is not implemented
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
