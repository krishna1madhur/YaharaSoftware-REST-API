using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Yahara2.Web.Models;
using System.IO;

namespace Yahara2.Web.Service
{
    public class YaharaServiceImplementation : YaharaService
    {
        public Segment GenerateSegment(IList<JToken> list)
        {
            Point P1 = new Point((int)list[0][0], (int)list[0][1]);
            Point P2 = new Point((int)list[1][0], (int)list[1][1]);
        
            Segment segment = new Segment(P1,P2);
            return segment;
        }

        public void StoreInFile(Segment segment1, Segment segment2)
        {
            try
            {
                string points = segment1.P1.X + "," + segment1.P1.Y + "," + segment1.P2.X + "," + segment1.P2.Y + "," +
                    segment2.P1.X + "," + segment2.P1.Y + "," + segment2.P2.X + "," + segment2.P2.Y;
                File.WriteAllText(System.Web.HttpContext.Current.Server.MapPath("~/Persistence/ExistingPoints.txt"), points);

            }
            catch (FileNotFoundException e) {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
        
        public Segment[] GenerateExistingSegment() {
            Segment[] segments = new Segment[2];
            try
            {
                string text = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Persistence/ExistingPoints.txt"));
                String[] points = text.Split(',');
                if (text == string.Empty) {
                    return null;
                }
                Point existingP1 = new Point(int.Parse(points[0]), int.Parse(points[1]));
                Point existingP2 = new Point(int.Parse(points[2]), int.Parse(points[3]));
                Point existingP3 = new Point(int.Parse(points[4]), int.Parse(points[5]));
                Point existingP4 = new Point(int.Parse(points[7]), int.Parse(points[7]));

                segments[0] = new Segment(existingP1, existingP2);
                segments[1] = new Segment(existingP3, existingP4);
            }
            catch (FileNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return segments;
        }

        public bool DoLinesIntersect(Segment line1, Segment line2)
        {
            return CrossProduct(line1.P1, line1.P2, line2.P1) !=
                   CrossProduct(line1.P1, line1.P2, line2.P2) ||
                   CrossProduct(line2.P1, line2.P2, line1.P1) !=
                   CrossProduct(line2.P1, line2.P2, line1.P2);
        }

        public double CrossProduct(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }
    }
}