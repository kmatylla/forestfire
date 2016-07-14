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
    public class ParametersPresenter:INotifyPropertyChanged
    {
        private readonly double _min;
        private readonly double _max;
        public double Min { get { return _min; } }
        public double Max { get { return _max; } }
        private readonly double[] _values;
        public double[] Values { get { return _values; } }
        private readonly string[] _names;
        public string[] Names { get { return _names; } }


        private readonly ObservableCollection<ParameterPresenter> _parameters;
        public ObservableCollection<ParameterPresenter> ParametersOC { get { return _parameters; } }

        public ParametersPresenter(double[] values, string[] names, double min=0, double max=1)
        {
            _max = max;
            _min = min;
            if (names.Length != values.Length) throw (new ArgumentException(
                String.Format("There is a different number of names ({0}) and values ({1}).", names.Length, values.Length)));
            _values = values;
            _names = names;
            _parameters= new ObservableCollection<ParameterPresenter>();
            for (int i = 0; i < _names.Length; ++i)
            {
                _parameters.Add(new ParameterPresenter(this,i));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }

    }
}
