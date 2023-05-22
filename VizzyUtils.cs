using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace REWVIZZY
{
    public class IDGenerator
    {
        private int currentId = 0;

        public int Next()
        {
            return (currentId++);
        }
    }

    public enum LayoutStyle
    {
        Row,
        Column,
    }
    public class PosLayoutTool
    {
        public LayoutStyle layoutStyle = LayoutStyle.Row;

        public int maxRowOrColumn = 5;

        public int blockDistance = 1000;

        private int xCurrent = 0;
        private int yCurrent = 0;

        public void NextPos(out int x, out int y)
        {
            x = xCurrent * blockDistance;
            y = yCurrent * blockDistance;

            if (layoutStyle == LayoutStyle.Row)
            {
                xCurrent++;
                if(xCurrent > maxRowOrColumn)
                {
                    xCurrent = 0;
                    yCurrent += 1;
                }
            }
            else
            {
                yCurrent++;
                if(yCurrent > maxRowOrColumn)
                {
                    yCurrent++;
                    xCurrent = 0;
                }
            }
        }
    }

}
