using ApiClientTools;

namespace Api
{

/// <summary>
/// The Region class
/// Exposing the Region API interfaces
///
/// @apiHash ad4b16b3b2a922585be866de7efb2182
/// @package Api\Region
/// @internalModel App\Region
/// </summary>

    public class Region : ApiClientTools.Client
    {
        protected static string internalModel = "App\\Region";


            /// <summary>
            /// getResource
            /// </summary>
            /// <returns>
            /// Data object
            /// </returns>

            public static object getResource(System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
            {
                return doGet("api/regions/getResource", null, endpointUrlData).Result;
            }

            /// <summary>
            /// index
            /// </summary>
            /// <remarks>
            /// Returns items list
            /// </remarks>
            /// <returns>
            /// Data object
            /// </returns>

            public static object index(System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
            {
                if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

                return doGet("api/regions", null, endpointUrlData).Result;
            }

            /// <summary>
            /// get
            /// </summary>
            /// <remarks>
            /// Returns one item
            /// </remarks>
            /// <param name="id">Required parameter 'id' of type int</param>
            /// <returns>
            /// Data object
            /// </returns>

            public static object get(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
            {
                return doGet("api/regions/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
            }

    }
}