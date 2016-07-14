//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Automaton
{
    public class StatystykiPresenter:INotifyPropertyChanged
    {
        private StatsModel _stats;
        private AutomatonPresenter _ap;
        private ObservableCollection<StatPresenter> _presenters;
        public ObservableCollection<StatPresenter> Stats { get { return _presenters; } }
        public Dictionary<Cell.StateEnum, StatModel> StatsD { get { return _stats.Stats; } }
        private int _height;
        private int _lenght;
        public int TotalHeight
        {
            get
            {
                return Stats.Select(x => x.Height).Sum();
            }
            set
            {
                int n=Stats.Count;
                foreach (var s in Stats) s.Height=value/n;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value != _height)
                {
                    foreach (var s in Stats) s.Height = value;
                    Height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public int Lenght 
        {
            get
            {
                return _lenght;
            }
            set
            {
                if (value != _lenght)
                {
                    _lenght = value;
                    foreach (var s in Stats) s.MaxBuffer = value;
                    OnPropertyChanged("Lenght");
                }
            }
        }

        public StatystykiPresenter(AutomatonPresenter ap, int l=300,int h=100)
        {
            _lenght=l;
            _height = h;
            _ap = ap;
            _stats = new StatsModel(_ap.Stats);
            _presenters = new ObservableCollection<StatPresenter>();
            foreach (var p in _stats.Stats) _presenters.Add(new StatPresenter(p.Value, _ap,Lenght,Height));
            _ap.ModelChanged += OnNewAutomat;
        }

        public void Update()
        {
            _stats.Update(_ap.Stats);
            foreach (var p in _presenters) p.update();
        }

        public void Update(object sender, StepData s)
        {
            _stats.Update(s.Counts);
            foreach (var p in _presenters) p.update();
        }

        public void Refresh()
        {
        foreach (var st in Stats) st.refresh();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

         

        private void OnNewAutomat(object s, EventArgs e)
        {
            _stats = new StatsModel(_ap.Stats);
            _presenters = new ObservableCollection<StatPresenter>();
            foreach (var p in _stats.Stats) _presenters.Add(new StatPresenter(p.Value, _ap, Lenght, Height));
//            _ap.ModelChanged += OnNewAutomat;
            OnPropertyChanged("Stats");
        }

        
    }
}
