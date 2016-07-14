//Copyright (C) 2010 Katarzyna Matylla

//This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 

//See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Automaton
{
    /// <summary>
    /// Interaction logic for NowyAutomat.xaml
    /// </summary>
    public partial class NewAutomaton : Window
    {
        public NewAutomaton(AutomatonPresenter mama)
        {
            _x = 50;
            _y = 50;
            Wx = true;
            Wy = true;
            _proc = 50;
            _mama = mama;
            InitializeComponent();
        }

        private AutomatonPresenter _mama;
        private int _x;
        private int _y;
        private int _proc;


        public string X 
        {
            get { return _x.ToString();  }
            set { Int32.TryParse(value, out _x); }
        }
        public string Y
        {
            get { return _y.ToString(); }
            set { Int32.TryParse(value, out _y); }
        }
        public bool Wx { get; set; }
        public bool Wy { get; set; }
        public string Proc
        {
            get { return _proc.ToString(); }
            set { Int32.TryParse(value, out _proc); }
        }

        private void ok_ev(object sender, RoutedEventArgs e)
        {
            _mama.MakeNew(_x, _y, Wx, Wy,(double) _proc/100);
            Close();
        }

        private void cancel_ev(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
