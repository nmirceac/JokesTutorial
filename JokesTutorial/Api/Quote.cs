using ApiClientTools;

namespace Api
{

    /// <summary>
    /// The Quote class
    /// Exposing the Mycovers API interfaces
    ///
    /// @apiHash f157b962b1df9489a2bfd25588f2adac
    /// @package Api\Quote
    /// @internalModel App\Quote
    /// </summary>
    public class Quote : ApiClientTools.Client
    {
        protected static string internalModel = "App\\Quote";

        public const string QUOTE_ASSOCIATIONS_PIVOT_TABLE = "Communications.dbo.quote_associations";
        public const int QUOTE_TYPE = 0;
        public const int TYPE_MYCOVER = 1;
        public const int TYPE_HOMEVANTAGE = 2;
        public const int TYPE_LBP = 3;
        public const string MYCOVER_DETAILS_TABLE = "mycover_quote_details";
        public const string HOMEVANTAGE_DETAILS_TABLE = "homevantage_quote_details";
        public const string LBP_DETAILS_TABLE = "lbp_quote_details";


        /// <summary>
        /// getForUser
        /// </summary>
        /// <remarks>
        /// Get by cl user id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getForUser(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getForUser/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getSaved
        /// </summary>
        /// <remarks>
        /// Get saved quotes
        /// </remarks>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getSaved(System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getSaved", null, endpointUrlData).Result;
        }
        /// <summary>
        /// getForIntermediary
        /// </summary>
        /// <remarks>
        /// Get by intermediary id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getForIntermediary(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getForIntermediary/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getForBrokerConsultant
        /// </summary>
        /// <remarks>
        /// Get for broker consultant id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getForBrokerConsultant(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getForBrokerConsultant/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getForFranchise
        /// </summary>
        /// <remarks>
        /// Get for franchise id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getForFranchise(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getForFranchise/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getForFulfilmentOfficer
        /// </summary>
        /// <remarks>
        /// Get for fulfilment officer id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getForFulfilmentOfficer(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getForFulfilmentOfficer/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getLeads
        /// </summary>
        /// <remarks>
        /// Get leads associated to the quotation
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getLeads(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getLeads/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getHolders
        /// </summary>
        /// <remarks>
        /// Get holders associated to the quotation
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getHolders(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getHolders/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// get
        /// </summary>
        /// <remarks>
        /// Returns quotation by id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object get(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getNewVersion
        /// </summary>
        /// <remarks>
        /// Get new version of quotation by id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getNewVersion(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getNewVersion/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getVersions
        /// </summary>
        /// <remarks>
        /// Get all the quote versions by quote id
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getVersions(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null, int page = 1)
        {
            if(endpointUrlData==null) {endpointUrlData = new System.Collections.Generic.Dictionary<string, string> { {"page", page.ToString()} }; }
                else if (!endpointUrlData.ContainsKey("page")) { endpointUrlData.Add("page", page.ToString()); }

            return doGet("api/quote/getVersions/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getByIdentifier
        /// </summary>
        /// <remarks>
        /// Returns quotation by identifier
        /// </remarks>
        /// <param name="identifier">Required parameter 'identifier' of type string</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getByIdentifier(string identifier, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getByIdentifier/{identifier}", new System.Collections.Generic.Dictionary<string, string>() { { "identifier", identifier } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getByReferenceNumber
        /// </summary>
        /// <remarks>
        /// Returns quotation by reference number
        /// </remarks>
        /// <param name="number">Required parameter 'number' of type string</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getByReferenceNumber(string number, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getByReferenceNumber/{number}", new System.Collections.Generic.Dictionary<string, string>() { { "number", number } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getPdfUrl
        /// </summary>
        /// <remarks>
        /// Get Quotation PDF url
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getPdfUrl(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getPdfUrl/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// getClientLandingUrl
        /// </summary>
        /// <remarks>
        /// Get Client Landing Page URL
        /// </remarks>
        /// <param name="id">Required parameter 'id' of type int</param>
        /// <returns>
        /// Data object
        /// </returns>
        public static object getClientLandingUrl(int id, System.Collections.Generic.Dictionary<string, string> endpointUrlData = null)
        {
            return doGet("api/quote/getClientLandingUrl/{id}", new System.Collections.Generic.Dictionary<string, string>() { { "id", id.ToString() } }, endpointUrlData).Result;
        }
        /// <summary>
        /// stepUpdate
        /// </summary>
        /// <remarks>
        /// Update quote step
        /// </remarks>
        /// <returns>
        /// Data object
        /// </returns>

        public static object stepUpdate(System.Dynamic.ExpandoObject endpointData = null)
        {
            return doPost("api/quote/stepUpdate", null, endpointData).Result;
        }
        /// <summary>
        /// saveQuotation
        /// </summary>
        /// <remarks>
        /// Save quote
        /// </remarks>
        /// <returns>
        /// Data object
        /// </returns>

        public static object saveQuotation(System.Dynamic.ExpandoObject endpointData = null)
        {
            return doPost("api/quote/saveQuotation", null, endpointData).Result;
        }
        /// <summary>
        /// setClientName
        /// </summary>
        /// <remarks>
        /// Set quote client_name
        /// </remarks>
        /// <returns>
        /// Data object
        /// </returns>

        public static object setClientName(System.Dynamic.ExpandoObject endpointData = null)
        {
            return doPost("api/quote/setClientName", null, endpointData).Result;
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

            return doGet("api/quote", null, endpointUrlData).Result;
        }

    }
}