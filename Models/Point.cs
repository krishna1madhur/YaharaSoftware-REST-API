using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yahara2.Web.Models
{
    public class Point
    {

        public int X { get; set; }
        public int Y { get; set; }

        public Point(int v1, int v2)
        {
            this.X = v1;
            this.Y = v2;
        }

    }
    
}