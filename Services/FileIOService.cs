using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TJ.Models;

namespace TJ.Services
{
    class FileIOService
    {
        private readonly string PATH;
        

        public FileIOService(string path)
        {
            PATH = path;
        }

        public BindingList<TjModel> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new BindingList<TjModel>();
            }
            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<TjModel>>(fileText);
            }
        }

        public void SaveData(object tjDataList)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(tjDataList);
                writer.Write(output);
            }

        }

        public BitmapImage LoadPic(string filePath)
        {
            //Uri defaultImgPath = new Uri($"{Environment.CurrentDirectory}\\PicData\\Default.png");

            var fileExists = File.Exists(filePath);
            if (!fileExists)
            {
                string[] all = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();
                var btmimg = new Bitmap(System.Reflection.Assembly.GetEntryAssembly(). GetManifestResourceStream("MyProject.Resources.myimage.png"));
                //return new BitmapImage(defaultImgPath);
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            
            image.UriSource = new Uri(filePath);
            image.EndInit();
            
            return image;
        }

        public void SavePic(BitmapImage img, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            try
            {
                    
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    encoder.Save(stream);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
