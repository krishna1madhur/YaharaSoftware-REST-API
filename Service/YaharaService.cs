using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yahara2.Web.Models;

namespace Yahara2.Web.Service
{
    interface YaharaService
    {
        Segment GenerateSegment(IList<JToken> list);

        void StoreInFile(Segment segment1, Segment segment2);

        Segment[] GenerateExistingSegment();
        
        bool checkFutureRequest(Segment[] segments, Segment[] existingSegments);

        bool DoLinesIntersect(Segment line1, Segment line2);

        int Orientation(Point P, Point Q, Point R);

        bool CheckOnSegment(Point P, Point Q, Point R);

    }
}
