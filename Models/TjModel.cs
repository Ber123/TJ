using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJ.Models
{
    class TjModel: INotifyPropertyChanged
    {
        private string _dateIn;
        private bool _isLongPos;
        private string _ticker;
        private int _volume;
        private double _priceIn;
        private double _komissiaIn;
        private double _sborIn;
        private double _konvert;
        private string _dateOut;
        private double _priceOut;
        private double _komissiaOut;
        private double _sborOut;
        private double _itogIn;
        private double _itogOut;
        private double _result;
        private double _ratingIn;
        private double _ratingOut;
        private double _rating;

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
                OnPropertyChanged("Result");
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
        public double PriceIn
        {
            get { return _priceIn; }
            set {
                if (_priceIn == value)
                    return;
                _priceIn = value;
                OnPropertyChanged("PriceIn");
                OnPropertyChanged("ItogIn");
                OnPropertyChanged("Result");
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
                OnPropertyChanged("Komissia");
                OnPropertyChanged("ItogIn");
                OnPropertyChanged("Result");
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
                OnPropertyChanged("ItogIn");
                OnPropertyChanged("Result");
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
                OnPropertyChanged("ItogOut");
                OnPropertyChanged("Result");
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
                OnPropertyChanged("ItogOut");
                OnPropertyChanged("Result");
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
                OnPropertyChanged("ItogOut");
                OnPropertyChanged("Result");
            }
        }
        public double ItogOut
        {
            get {
                return _priceOut * _volume - _komissiaOut - _sborOut;
            }
        }
        public double Result
        {
            get
            {
                return _isLongPos ? ItogOut - ItogIn : ItogIn - ItogOut; ;
            }
        }
        public double RatingIn
        {
            get { return _ratingIn; }
            set
            {
                if (_ratingIn == value)
                    return;
                _ratingIn = value;
                OnPropertyChanged("RatingIn");
            }
        }
        public double RatingOut
        {
            get { return _ratingOut; }
            set
            {
                if (_ratingOut == value)
                    return;
                _ratingOut = value;
                OnPropertyChanged("RatingOut");
            }
        }
        public double Rating
        {
            get { return _rating; }
            set
            {
                if (_rating == value)
                    return;
                _rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
