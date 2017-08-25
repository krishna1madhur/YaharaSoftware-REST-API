using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Yahara2.Web.Models;
using Yahara2.Web.Service;

namespace Yahara2.Web.Controllers
{
    public class SegmentsController : ApiController
    {
        YaharaService service = new YaharaServiceImplementation();

        public Boolean Post([FromBody] JObject jObject)
        {
            bool responseValue = false;
            try
            {
                JObject inputJObject = JObject.Parse(jObject.ToString());
                IList<JToken> results = inputJObject["segments"].Children().ToList();
                foreach (JToken result in results)
                {
                    IList<JToken> aValue = result["a"].Children().ToList();
                    IList<JToken> bValue = result["b"].Children().ToList();

                    Segment segment1 = service.GenerateSegment(aValue);
                    Segment segment2 = service.GenerateSegment(bValue);
                    if (segment1 == null || segment2 == null) return false;

                    Segment[] segments = new Segment[2];
                    segments[0] = segment1;
                    segments[1] = segment2;

                    Segment[] existingSegments = service.GenerateExistingSegment();

                    if (existingSegments != null)
                    {
                        responseValue = service.checkFutureRequest(segments, existingSegments);
                    }
                    service.StoreInFile(segment1, segment2);
                }
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return responseValue;
        }
    }
}
