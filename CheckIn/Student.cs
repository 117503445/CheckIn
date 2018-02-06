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
    public class Student
    {
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;
        private int score = 0;
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
        Ellipse ellipse;
        Button button = new Button();
        public void ShowButtonOfStudent(Grid grid)
        {


            StackPanel stackPanel = new StackPanel
            {
                Padding = new Thickness(0),
                Orientation = Orientation.Horizontal
            };
            TextBlock textBlock = new TextBlock
            {
                Text = Name,
                FontSize = 18
            };
            ellipse = new Ellipse
            {
                Margin = new Thickness(0, 0, 0, 0),
                Width = 18,
                Height = 18,
                //Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 0, 0))
            }; stackPanel.Children.Add(ellipse);
            stackPanel.Children.Add(textBlock);
            Button.HorizontalContentAlignment = HorizontalAlignment.Left;
            Button.Content = stackPanel;
            grid.Children.Add(Button);
            Grid.SetRow(Button, 2 * Row - 2);
            Grid.SetColumn(Button, 2 * Column - 2);
            //button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
            Button.HorizontalAlignment = HorizontalAlignment.Stretch;
            Button.VerticalAlignment = VerticalAlignment.Stretch;
            Button.Click += Button_Click;


        }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public Button Button { get => button; set => button = value; }
        public int Score { get => score; set => score = value; }

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
    public class SortedStudentById : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            if (x.Id > y.Id)
            {
                return 1;
            }
            else if (x.Id < y.Id)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
