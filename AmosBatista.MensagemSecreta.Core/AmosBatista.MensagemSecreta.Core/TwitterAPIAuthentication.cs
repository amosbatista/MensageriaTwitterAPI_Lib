using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;

namespace AmosBatista.MensagemSecreta.Core
{
    public class TwitterAPIAuthentication
    {
        // Parameters list (name of atribute, value)
        private List<string> _AuthParameters; 

        // Common parameters
        private string _UrlTweeterAPICall;

        // Function to add a new parameter to the parameter list
        public void AddAuthParameter(string parameterName, string value)
        {
            _AuthParameters.Add(parameterName + "§" + value);
        }

        // Function that return the parameters
        public List<string> GetAllParameters()
        {
            return _AuthParameters;
        }

        // Constructor
        public TwitterAPIAuthentication(string UrlTweeterAPICall)
        {
            _AuthParameters = new List<string>();
            _UrlTweeterAPICall = UrlTweeterAPICall;
        }

        /*
         * Function that generate the parameter 'oauth_signature'. 
         * It receives the app consumer secret and token secret key, given by Twitter API manager.
         * It also receives the kind of method to the HTTP request, as 'POST' or 'GET'
         */
        public void GenerateSignatureParameter(string methodType, string consumerSecret, string tokenSecret)
        {
            var signatureContent = new StringBuilder();
            var signatureContentPlusKeyURL = new StringBuilder();
            var singningKey = new StringBuilder();
            var signingContentKey = new StringBuilder();
            string[] parameterContent;
            Byte[] signatureValueHashCode;

            // Sorting the list
            _AuthParameters.Sort();

            // Looping all list, to append its content in a single String
            for (int contKeyList = 0; contKeyList < _AuthParameters.Count; contKeyList++)
            {
                // Split the content of the list
                parameterContent = _AuthParameters[contKeyList].Split("§".ToCharArray());

                // Encode the parameters to Percent Format
                parameterContent[0] = Uri.EscapeDataString(parameterContent[0]);
                parameterContent[1] = Uri.EscapeDataString(parameterContent[1]);

                //Append this parameter to string
                signatureContent.Append(parameterContent[0]);
                signatureContent.Append("=");
                signatureContent.Append(parameterContent[1]);

                // Append a '&' char if it's not the last parameter
                if (contKeyList < _AuthParameters.Count - 1)
                    signatureContent.Append("&");
                
            }

            // After all parameters, start a new appending, formating all signature content to percent format
            signatureContentPlusKeyURL.Append(Uri.EscapeDataString(signatureContent.ToString()));

            // Adding other parameters to the string
            signatureContentPlusKeyURL.Insert(0, Uri.EscapeDataString(_UrlTweeterAPICall) + "&");
            signatureContentPlusKeyURL.Insert(0, methodType.ToUpper() + "&");

            // Creating the signing content
            singningKey.Append(Uri.EscapeDataString(consumerSecret));
            singningKey.Append("&");
            singningKey.Append(Uri.EscapeDataString(tokenSecret));

            // So, with the 2 keys, generate the result of SHA-1 algorithm calcule.
            signingContentKey.Append(signatureContentPlusKeyURL.ToString());
            signingContentKey.Append(singningKey.ToString());

            SHA1Managed sha1SignGenerator = new SHA1Managed();
            signatureValueHashCode = sha1SignGenerator.ComputeHash(System.Text.Encoding.Unicode.GetBytes(signingContentKey.ToString()));

            // At the end, append a new parameter, with the signature value
            AddAuthParameter("oauth_signature", Convert.ToBase64String(signatureValueHashCode));
        }
        
    }
}
