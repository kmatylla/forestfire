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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Automaton
{
    /// <summary>
    /// Interaction logic for TriangleWindow.xaml
    /// </summary>
    public partial class TriangleWindow : Window, INotifyPropertyChanged
    {
        private TrianglePresenter _triangleP;
        public TrianglePresenter TriangleP { get {return _triangleP;}}
        public int X { get { return (int)(_triangleP.X*Width/2 + Width / 2); } }
        public int Y { get { return (int)(_triangleP.Y*Height/2 + Height / 2); } }
        public int PSize { get { return (int)(_triangleP.PSize); } }
        public void Refresh()
        {
            _triangleP.Refresh();
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
            OnPropertyChanged("PSize");
            OnPropertyChanged("CBrush");
        }

        public Brush CBrush
        {
            get
            {
                return new SolidColorBrush(_triangleP.PColor);
            }
        }

        public TriangleWindow(TrianglePresenter t)
        {
            _triangleP = t;
            InitializeComponent();
            CanClose = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
            { h(this, new PropertyChangedEventArgs(propertyName)); }
        }


        public bool CanClose { get; set; }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (CanClose) base.OnClosing(e);
            else
            {
                e.Cancel=true;
                Hide();
            }
        }
    }
}
