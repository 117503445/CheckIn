﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace CheckIn_WPF
{
    public class Student : IComparable<Student>
    {
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;
        private int score = 0;
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        private CheckType cType = CheckType.Present;
        public CheckType CType
        {
            get { return cType; }
            set
            {
                switch (value)
                {
                    case CheckType.Present:
                        //Btnstu.BorderBrush = new SolidColorBrush(Windows.UI.Colors.ForestGreen);
                        //Btnstu.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 0, 255, 0));
                        //Btnstu.BorderThickness = new Thickness(0);
                        Btnstu.BorderBrush = new SolidColorBrush(Colors.Black);
                        Btnstu.BorderThickness = new Thickness(1);
                        break;
                    case CheckType.Absent:

                        Btnstu.BorderBrush = new SolidColorBrush(Colors.Orange);
                        Btnstu.BorderThickness = new Thickness(4);
                        break;
                    case CheckType.Leave:
                        Btnstu.BorderBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                        Btnstu.BorderThickness = new Thickness(4);
                        break;
                    default:
                        break;
                }
                cType = value;
            }
        }

        public Student(string name, int id, int row, int column)
        {
            Name = name;
            Id = id;
            Row = row;
            Column = column;
        }

        Button btnStu = new Button { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Colors.Black), Height = double.NaN };
        public void ShowButtonOfStudent(Grid grid)
        {
            Btnstu.Content = new Viewbox
            {
                Child = new TextBlock
                {
                    Text = Name,
                }
            };

            grid.Children.Add(Btnstu);
            Grid.SetRow(Btnstu, 2 * Row - 2);
            Grid.SetColumn(Btnstu, 2 * Column - 2);
            Btnstu.HorizontalAlignment = HorizontalAlignment.Stretch;
            Btnstu.VerticalAlignment = VerticalAlignment.Stretch;
            Btnstu.Click += Button_Click;
        }

        public Button Btnstu { get => btnStu; set => btnStu = value; }
        public int Score
        {
            get => score;
            set
            {
                score = value;
            }
        }
        /// <summary>
        /// 内部click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("内部click");
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
            BtnClick(this, new EventArgs());
        }
        public event EventHandler BtnClick;
        public int CompareTo(Student other)
        {
            return id - other.id;
        }
    }
}
