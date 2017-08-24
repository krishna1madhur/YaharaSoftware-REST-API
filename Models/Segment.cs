using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yahara2.Web.Models
{
    public class Segment
    {
        public Point P1 { set; get; }
        public Point P2 { set; get; }

        public Segment(Point P1, Point P2)
        {
            this.P1 = P1;
            this.P2 = P2;
        }
    }
}