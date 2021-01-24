﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Uri defaultImgPath = new Uri($"{Environment.CurrentDirectory}\\PicData\\Default.png");
            var fileExists = File.Exists(filePath);
            if (!fileExists)
            {
                return new BitmapImage(defaultImgPath);
            }
            return new BitmapImage(new Uri(filePath));
        }

        public void SavePic(BitmapImage img, string fileName)
        {
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            try
            {
                //var fileExists = File.Exists(fileName);
                //if (fileExists)
                //{
                //    File.Delete(fileName);
                //    //File.Delete("D:\\C_ALL\\TraderJournal\\TJ\\bin\\Debug\\PicData\\1.txt");
                //}
                    
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