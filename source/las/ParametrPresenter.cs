//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Automaton
{
    public class ParameterPresenter:INotifyPropertyChanged
    {
        private readonly ParametersPresenter _mama;
        private readonly int _number;
//        public int Y { get { return _numer * 20; } }
        public double Value
        {
            get
            {
                return _mama.Values[_number];
            }
            set
            {
                if (value <= _mama.Max && value >= _mama.Min && _mama.Values[_number] != value)
                {
                    _mama.Values[_number] = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public string Name
        {
            get
            {
                return _mama.Names[_number];
            }
            set
            {
                if (_mama.Names[_number] != value)
                {
                    _mama.Names[_number] = value;
                    OnPropertyChanged("Names");
                }
            }
        }

        public ParameterPresenter(ParametersPresenter mama, int nr)
        {
            _number = nr;
            _mama = mama;
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
