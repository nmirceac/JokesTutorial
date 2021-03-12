using ApiClientTools;

namespace Api
{
    public class Region : ApiClientTools.Client
    {
        public static object getResource()
        {
            return doGet("api/regions/getResource").Result;
        }
    }
}
