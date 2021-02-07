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
        //DisplayedImagePath21
        public string DisplayedImagePath21 { get; set; }
        //{
        //    get { return @"d:\C_All\TraderJournal\TJ\Start.png"; }
        //}

        public MainWindow()
        {
            InitializeComponent();
        }

        public int GetCurrentIndex()
        {
            return dgTraderTable.Items.IndexOf(dgTraderTable.CurrentItem);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(PATH);
            if (!Directory.Exists(PATHPICK))
                Directory.CreateDirectory(PATHPICK);

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

                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.UriSource = filepath;
                        image.EndInit();
                        imgThumbnail.Source = image;

                        var imageName = PATHPICK+currentRowIndex+ imgThumbnail.Name+".bmp";
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


            Im10.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im10.bmp");
            Im11.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im11.bmp");
            Im20.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im20.bmp");

            var model = new TjModel();
            model.SetImageData(File.ReadAllBytes(PATHPICK + currentRowIndex + "Im21.bmp"));

            //_tjData.
            //DisplayedImagePath21 = new Uri(PATHPICK + currentRowIndex + "Im21.bmp").ToString();
            //Im21.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im21.bmp");
            //Im30.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im30.bmp");
            //Im31.Source = _fileIOService.LoadPic(PATHPICK + currentRowIndex + "Im31.bmp");

        }

        private void MouseLeftButtonDown_Border(object sender, MouseButtonEventArgs e)
        {
            var MinWidth = 800;
            var border = (Border)sender;
            var img = (Image)border.Child;
            BitmapImage imgBkp = (BitmapImage)Im21.Source;


            var rowIndex = Convert.ToInt32(img.Name.Substring(2, 1));
            var colIndex = Convert.ToInt32(img.Name.Substring(3, 1));
            var grid = (Grid)border.Parent;
            var currentRowIndex = dgTraderTable.Items.IndexOf(dgTraderTable.CurrentItem);

            if (border.ActualWidth < MinWidth)
            {
                Grid.SetColumn(bLast, 0);
                Grid.SetColumnSpan(bLast, 2);
                Grid.SetRow(bLast, 2);
                Grid.SetRowSpan(bLast, 2);
                Im21.Source = img.Source;
            }
            else
            {
                Grid.SetColumn(bLast, 1);
                Grid.SetColumnSpan((Border)sender, 1);
                Grid.SetRow(bLast, 2);
                Grid.SetRowSpan((Border)sender, 1);
                Im21.Source = imgBkp;
                //Im21.Source = _picIOService.LoadPic(PATHPICK + currentRowIndex + "Im21.bmp");
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
