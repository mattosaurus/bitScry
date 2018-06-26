using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.BingSearch
{
    public class ImageSearchParameters
    {
        /// <summary>
        /// The user's search query term. The term cannot be empty.
        /// 
        /// The term may contain Bing Advanced Operators. For example, to limit images to a specific domain, use the site: operator.
        /// 
        /// To help improve relevance of an insights query (see insightsToken), you should always include the user's query term
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The number of images to return in the response. The actual number delivered may be less than requested. The default is 35. The maximum value is 150.
        /// 
        /// You use this parameter along with the offset parameter to page results. For example, if your user interface displays 20 images per page, set count to 20 and offset to 0 to get the first page of results. For each subsequent page, increment offset by 20 (for example, 0, 20, 40).
        /// </summary>
        public UInt16 Count { get; set; }

        /// <summary>
        /// The zero-based offset that indicates the number of images to skip before returning images. The default is 0. The offset should be less than (totalEstimatedMatches - count).
        /// 
        /// To page results, use this parameter along with the count parameter. For example, if your user interface displays 20 images per page, set count to 20 and offset to 0 to get the first page of results. For each subsequent page, increment offset by 20 (for example, 0, 20, 40).
        /// 
        /// It is possible for multiple pages to include some overlap in results. To prevent duplicates, see nextOffset.
        /// </summary>
        public UInt16 Offset { get; set; }
    }
}
