﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CheckIn
{
    public class Student
    {
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;
 
        private CheckType cType = CheckType.Present;
        public CheckType CType
        {
            get { return cType; }
            set
            {
                switch (value)
                {
                    case CheckType.Present:
                        ellipse.Opacity = 0;
                        break;
                    case CheckType.Absent:
                        ellipse.Opacity = 1;
                        ellipse.Fill = new SolidColorBrush(Windows.UI.Colors.Orange);
                        break;
                    case CheckType.Leave:
                        ellipse.Opacity = 1;
                        ellipse.Fill = new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue);
                        break;
                    default:
                        break;
                }
                cType = value;
            }
        }

        public Student(string name, int id, int row, int column)
        {
            this.Name = name;
            this.Id = id;
            this.Row = row;
            Column = column;
            
        }

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CType == CheckType.Present)
            {
                CType = CheckType.Absent;
            }
            else if (CType == CheckType.Absent)
            {
                CType = CheckType.Leave;
            }
            else
            {
                CType = CheckType.Present;
            }
        }

    }
}
