using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIn
{
    public class Students
    {
        private string name = "";
        private int positionX = 0;
        private int positionY = 0;
        public int color = 0;
        private bool isCheck = true;
        private bool enabled = false;
        public string Name { get => name; set => name = value; }
        public int PositionX { get => positionX; set => positionX = value; }
        public int PositionY { get => positionY; set => positionY = value; }
        public bool IsCheck { get => isCheck; set => isCheck = value; }
        public bool Enabled { get => enabled; set => enabled = value; }
    }
}
