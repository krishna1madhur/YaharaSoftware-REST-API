using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Yahara2.Web.Models;
using System.IO;
using Yahara2.Web.Service;

namespace Yahara2.Web.Service
{
    public class YaharaServiceImplementation : YaharaService
    {
        public Segment GenerateSegment(IList<JToken> list)
        {
            if (list == null) return null;
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


        public bool checkFutureRequest(Segment[] segments, Segment[] existingSegments)
        {
            if (segments == null || existingSegments == null) return false;

            if (DoLinesIntersect(segments[0], existingSegments[0]) ||
                DoLinesIntersect(segments[0], existingSegments[1]) ||
                    DoLinesIntersect(segments[1], existingSegments[0]) ||
                    DoLinesIntersect(segments[1], existingSegments[1]))
            {
                return true;
            }
            return false;
        }

        public bool DoLinesIntersect(Segment line1, Segment line2)
        {
            if (line1 == null || line2 == null) return false;

            Point p1 = line1.P1;
            Point q1 = line1.P2;

            Point p2 = line2.P1;
            Point q2 = line2.P2;

            int o1 = Orientation(p1, q1, p2);
            int o2 = Orientation(p1, q1, q2);
            int o3 = Orientation(p2, q2, p1);
            int o4 = Orientation(p2, q2, q1);

            if (o1 != o2 && o3 != o4)
                return true;

            if (o1 == 0 && CheckOnSegment(p1, p2, q1)) return true;
            
            if (o2 == 0 && CheckOnSegment(p1, q2, q1)) return true;
            
            if (o3 == 0 && CheckOnSegment(p2, p1, q2)) return true;
            
            if (o4 == 0 && CheckOnSegment(p2, q1, q2)) return true;

            return false; 
        }

        public int Orientation(Point P, Point Q, Point R)
        {
            int val = ((Q.Y - P.Y) * (R.X - Q.X)) - ((Q.X - P.X) * (R.Y - Q.Y));
            if (val == 0) return 0;
            return (val > 0) ? 1 : 2;
        }

        public bool CheckOnSegment(Point P, Point Q, Point R)
        {
            if (Q.X <= Math.Max(P.X, R.X) && Q.X >= Math.Min(P.X, R.X) &&
                Q.Y <= Math.Max(P.Y, R.Y) && Q.Y >= Math.Min(P.Y, R.Y))
                return true;
            return false;
        }

        
    }
}