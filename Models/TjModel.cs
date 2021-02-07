using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace TJ.Models
{
    class TjModel: INotifyPropertyChanged
    {
        private string _dateIn;
        private bool _isLongPos;
        private string _ticker;
        private string _comment;
        private int _volume;
        private double _priceIn;
        private double _komissiaIn;
        private double _sborIn;
        private double _konvert;
        private string _dateOut;
        private double _priceOut;
        private double _komissiaOut;
        private double _sborOut;
        private int _number;
        

        public string DateIn
        {
            get { return _dateIn; }
            set
            {
                if (_dateIn == value)
                    return;
                DateTime tempDataIn;
                DateTime.TryParse(value, out tempDataIn);
                _dateIn = tempDataIn.ToString("dd MMMM yy");
                OnPropertyChanged("DateIn");
                OnPropertyChanged("Time");
            }
        }
        public string DateOut
        {
            get { return _dateOut; }
            set
            {
                if (_dateOut == value)
                    return;
                DateTime tempData;
                DateTime.TryParse(value, out tempData);
                _dateOut = tempData.ToString("dd MMMM yy");
                OnPropertyChanged("DateOut");
                OnPropertyChanged("Time");
            }
        }
        public bool LongPos
        {
            get { return _isLongPos; }
            set
            {
                if (_isLongPos == value)
                    return;
                _isLongPos = value;
                OnPropertyChanged("LonPgos");
                MassPropertyChange();
            }
        }
        public string Ticker
        {
            get { return _ticker; }
            set
            {
                if (_ticker == value)
                    return;
                _ticker = value;
                OnPropertyChanged("Ticker");
            }
        }
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment == value)
                    return;
                _comment = value;
                OnPropertyChanged("Comment");
            }
        }
        public int Volume
        {
            get { return _volume; }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;
                OnPropertyChanged("Volume");
            }
        }
        public int Time
        {
            get {
                DateTime dateEnd;
                DateTime.TryParse(DateOut, out dateEnd);
                DateTime dateStart;
                DateTime.TryParse(DateIn, out dateStart);

                return dateEnd.Subtract(dateStart).Days;
            }
        }
        public double PriceIn
        {
            get { return _priceIn; }
            set {
                if (_priceIn == value)
                    return;
                _priceIn = value;
                OnPropertyChanged("PriceIn");
                MassPropertyChange();
            }
        }
        public double KomissiaIn
        {
            get { return _komissiaIn; }
            set
            {
                if (_komissiaIn == value)
                    return;
                _komissiaIn = value;
                OnPropertyChanged("KomissiaIn");
                MassPropertyChange();
            }
        }
        public double SborIn
        {
            get { return _sborIn; }
            set
            {
                if (_sborIn == value)
                    return;
                _sborIn = value;
                OnPropertyChanged("SborIn");
                MassPropertyChange();
            }
        }
        public double ItogIn
        {
            get
            {
                return _priceIn * _volume + _komissiaIn + _sborIn;
            }
        }
        public double Konvert
        {
            get { return _konvert; }
            set
            {
                if (_konvert == value)
                    return;
                _konvert = value;
                OnPropertyChanged("Konvert");
                MassPropertyChange();
            }
        }
        public double PriceOut
        {
            get { return _priceOut; }
            set
            {
                if (_priceOut == value)
                    return;
                _priceOut = value;
                OnPropertyChanged("PriceOut");
                MassPropertyChange();
            }
        }
        public double KomissiaOut
        {
            get { return _komissiaOut; }
            set
            {
                if (_komissiaOut == value)
                    return;
                _komissiaOut = value;
                OnPropertyChanged("KomissiaOut");
                MassPropertyChange();
            }
        }
        public double SborOut
        {
            get { return _sborOut; }
            set
            {
                if (_sborOut == value)
                    return;
                _sborOut = value;
                OnPropertyChanged("SborOut");
                MassPropertyChange();
            }
        }
        public double ItogOut
        {
            get {
                return _priceOut * _volume - _komissiaOut - _sborOut;
            }
        }
        public double ResultNominal
        {
            get
            {
                return _isLongPos ? ItogOut - ItogIn : ItogIn - ItogOut;
            }
        }
        public double ResultPercent
        {
            get
            {
                return _isLongPos ? Math.Round(100*(ItogOut - ItogIn)/ ItogIn,2,MidpointRounding.ToEven) : Math.Round(100 * (ItogIn - ItogOut)/ ItogIn, 2, MidpointRounding.ToEven);
            }
        }
        public double ProfitPerTime
        {
            get
            {
                return Math.Round(ResultNominal / Time, 2, MidpointRounding.ToEven);
            }
        }

        public double Rating
        {
            get
            {

                var marga = (_isLongPos) ? ItogOut - ItogIn : ItogIn - ItogOut;
                if (marga > 0.3 * Konvert)
                    return 5;
                else
                {
                    if (marga > 0.2 * Konvert)
                        return 4;
                    else
                    {
                        if (marga > 0.1 * Konvert)
                            return 3;
                        else
                            return 2;
                    }
                }
            }
        }

        ImageSource _imageSource21;

        public ImageSource ImageSource21
        {
            get {

                //Uri defaultImgPath = new Uri($"{Environment.CurrentDirectory}\\PicData\\Default.png");
                //string currentImgPath = $"{Environment.CurrentDirectory}\\PicData\\)"+_number.ToString()+"Im21";
                //var fileExists = File.Exists(currentImgPath);
                //if (!fileExists)
                //{
                //    return new BitmapImage(defaultImgPath);
                //}
                return _imageSource21; 
            }
            set
            {
                _imageSource21 = value;
                OnPropertyChanged("ImageSource21");
            }
        }

        //public double RatingIn
        //{
        //    get { 
        //        return _ratingIn;
        //    }
        //}
        //public double RatingOut
        //{
        //    get { return _ratingOut; }
        //    set
        //    {
        //        if (_ratingOut == value)
        //            return;
        //        _ratingOut = value;
        //        OnPropertyChanged("RatingOut");
        //    }
        //}

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MassPropertyChange()
        {
            OnPropertyChanged("ItogIn");
            OnPropertyChanged("ItogOut");
            OnPropertyChanged("ResultNominal");
            OnPropertyChanged("ResultPercent");
            OnPropertyChanged("Rating");
            OnPropertyChanged("ProfitPerTime");
        }

        public void SetImageData(byte[] data)
        {
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(data);
            source.EndInit();

            // use public setter
            ImageSource21 = source;
        }
    }
}
