using System;
using System.Net;


namespace Q_pop
{
    public class CookieAwareWebClient : System.Net.WebClient
    {
        
        public CookieAwareWebClient()
        {
            CookieContainer = new CookieContainer();
        }

        // An aptly named container to store the Cookie
        public CookieContainer CookieContainer { get; private set; }


        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            // Grabs the base request being made 
            var request = (System.Net.HttpWebRequest)base.GetWebRequest(address);
            // Adds the existing cookie container to the Request
            request.CookieContainer = CookieContainer;
            return request;
        }
    }
}
