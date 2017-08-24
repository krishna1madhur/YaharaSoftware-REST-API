using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Yahara2.Web.Models;
using System.Collections;
using Yahara2.Web.Service;
using System.IO;

namespace Yahara2.Web.Controllers
{
    public class ValuesController : ApiController
    {
        YaharaService service = new YaharaServiceImplementation();
        // POST api/values
        public Boolean Post([FromBody] JObject jObject)
        {
            JObject googleSearch = JObject.Parse(jObject.ToString());
            IList<JToken> results = googleSearch["segments"].Children().ToList();
            bool responseValue = false;
 
            foreach (JToken result in results)
            {
                IList<JToken> aValue = result["a"].Children().ToList();
                IList<JToken> bValue = result["b"].Children().ToList();
                
                Segment segment1 = service.GenerateSegment(aValue);
                Segment segment2 = service.GenerateSegment(bValue);

                Segment[] existingSegments = service.GenerateExistingSegment();

                if (existingSegments != null) {
                    if (service.DoLinesIntersect(segment1, existingSegments[0]) || service.DoLinesIntersect(segment1, existingSegments[1]) ||
                    service.DoLinesIntersect(segment2, existingSegments[0]) || service.DoLinesIntersect(segment2, existingSegments[1]))
                    {
                        responseValue = true;
                    }
                }
                service.StoreInFile(segment1, segment2);
            }
            return responseValue;
        }
    }
}
