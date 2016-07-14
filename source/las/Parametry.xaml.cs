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

namespace Automaton
{
    /// <summary>
    /// Interaction logic for Parametry.xaml
    /// </summary>
    public partial class Parameters : Window
    {
        public ParametersPresenter ParamP { get; set; }
        public Parameters(AutomatonPresenter ap)
        {
            ParamP = new ParametersPresenter(ap.Params, ap.ParamNames); 
            InitializeComponent();
            CanClose = false;
        }

        public bool CanClose { get; set; }
        protected override void OnClosing(CancelEventArgs e)
        {
            if (CanClose) base.OnClosing(e);
            else
            {
                e.Cancel = true;
                Hide();
            }
        }

    }
}
