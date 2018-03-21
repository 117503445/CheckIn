using System;
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
                        Btnstu.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                        Btnstu.BorderThickness = new Thickness(1);
                        break;
                    case CheckType.Absent:

                        Btnstu.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Orange);
                        Btnstu.BorderThickness = new Thickness(4);
                        break;
                    case CheckType.Leave:
                        Btnstu.BorderBrush = new SolidColorBrush(Windows.UI.Colors.DeepSkyBlue);
                        Btnstu.BorderThickness = new Thickness(4);
                        break;
                    default:
                        break;
                }
                cType = value;
            }
        }

        GridViewItem gvItem = new GridViewItem { Width = 100 };
        TextBlock gvTb = new TextBlock { FontSize = 20, Text = "null" };
        public Student(string name, int id, int row, int column)
        {
            Name = name;
            Id = id;
            Row = row;
            Column = column;
        }

        Button btnStu = new Button { FontSize = 24, BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black) };
        public void ShowButtonOfStudent(Grid grid)
        {
            //Btnstu.HorizontalContentAlignment = HorizontalAlignment.Center;
            Btnstu.Content = Name;
            grid.Children.Add(Btnstu);
            Grid.SetRow(Btnstu, 2 * Row - 2);
            Grid.SetColumn(Btnstu, 2 * Column - 2);
            //button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
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
                gvTb.Text = Name + " " + Score.ToString();
            }
        }
        public GridViewItem GvItem
        {
            get
            {
                gvTb.Text = Name + " " + Score.ToString();
                gvItem.Content = gvTb;
                return gvItem;
            }
            set => gvItem = value;
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
        }
        public int CompareTo(Student other)
        {
            return id - other.id;
        }
    }
}
