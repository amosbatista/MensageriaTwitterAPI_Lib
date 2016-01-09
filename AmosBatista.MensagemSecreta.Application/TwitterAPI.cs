using System.Collections.Generic;
using AmosBatista.Utilities;
using AmosBatista.MensagemSecreta.Core;
using System.Text;
using System;
using BaseClassNameSpace.Web.BaseServices;
using System.Collections.Specialized;
using System.IO;

// New library
using Twitterizer;

namespace AmosBatista.MensagemSecreta.App
{
    public class TwitterAPI
    {
        private string app_consumer_key = "UElYlAUyfo7S2aNf0aBnsA60c";
        private string app_token = "72423411-T1vi02fZmxzIfEi0RIVwYWbNMwq9fdxzsKHdVQKXB";
        private string app_consumerSecret = "eg8mCxmVGp1RalL6KCAlbuO1XqCDGDJXGirk3w0gNzJHyIIzha";
        private string app_TokenSecret = "ytV6tPoCjXCRbdENObNUNR57SzlHv6JYwVVStlTSlxqaQ";
        private string app_signature_method = "HMAC-SHA1";
        private string app_oauth_version = "1.0";
        private string urlTweeterAPICall = @"https://api.twitter.com/1.1/direct_messages/new.json";

        public void SendTwitterDirectMessage_old(List<string> twitterUserList, string message)
        {
            // APP Constants
            

            // Generating timestamp and 64 Biuts key.
            var nounce_value = Generator.Generate64BitsKey();
            var oauth_timestamp = Generator.GenerateTimeStamp();

            // Creating and setting the Authenticatin object
            var twitterAuthenticator = new TwitterAPIAuthentication(urlTweeterAPICall);

            twitterAuthenticator.AddAuthParameter("include_entities", "true");
            twitterAuthenticator.AddAuthParameter("oauth_consumer_key", app_consumer_key);
            twitterAuthenticator.AddAuthParameter("oauth_nonce", nounce_value);
            twitterAuthenticator.AddAuthParameter("oauth_signature_method", app_signature_method);
            twitterAuthenticator.AddAuthParameter("oauth_timestamp", oauth_timestamp);
            twitterAuthenticator.AddAuthParameter("oauth_token", app_token);
            twitterAuthenticator.AddAuthParameter("oauth_version", app_oauth_version);


            // Now, setting the content of message
            var twitterUserListUnify = new StringBuilder();

            foreach (string userName in twitterUserList)
                twitterUserListUnify.Append(userName);

            twitterAuthenticator.AddAuthParameter("screen_name", twitterUserListUnify.ToString());
            twitterAuthenticator.AddAuthParameter("text", message);


            // Generating the signature method
            twitterAuthenticator.GenerateSignatureParameter("POST", app_consumerSecret, app_TokenSecret);


            // Process the request header
            var authParameters = twitterAuthenticator.GetAllParameters();
            var authParameters_ToHeader = new StringBuilder();
            string [] parameterContent;

            authParameters_ToHeader.Append("OAuth ");
            
            for (int cont = 0; cont < authParameters.Count; cont++){
                parameterContent = authParameters[cont].Split("§".ToCharArray());

                // Encode the parameters to Percent Format
                parameterContent[0] = Uri.EscapeDataString(parameterContent[0]);
                parameterContent[1] = Uri.EscapeDataString(parameterContent[1]);

                authParameters_ToHeader.Append(parameterContent[0]);
                authParameters_ToHeader.Append("=" + '"');
                authParameters_ToHeader.Append(parameterContent[1]);
                authParameters_ToHeader.Append('"');
                if(cont < authParameters.Count-1)
                    authParameters_ToHeader.Append(", ");
            }

            NameValueCollection headerContent = new NameValueCollection();
            //headerContent.Add("Authorization", authParameters_ToHeader.ToString().Replace("\\",""));
            headerContent.Add("Authorization", "OAuth oauth_consumer_key=\"UElYlAUyfo7S2aNf0aBnsA60c\", oauth_nonce=\"cb9a3d3cebb2ab473af86240075b14b8\", oauth_signature=\"Cjn4plMGgH3BvScOgNCLP6sP4H4%3D\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"1451156047\", oauth_token=\"72423411-T1vi02fZmxzIfEi0RIVwYWbNMwq9fdxzsKHdVQKXB\", oauth_version=\"1.0\"");
            
            // Now, start the WebRequest
            var webRequestProcessor = new HttpBaseClass("", "", "", 0, "");

            var httpRequest = webRequestProcessor.CreateWebRequest(urlTweeterAPICall, headerContent, "POST", true);
            var response = httpRequest.GetResponse();
            

        }

        public string SendTwitterDirectMessage(string twitterUser, string message)
        {
            var oauthAuthentication = new OAuthTokens();

            oauthAuthentication.ConsumerKey = app_consumer_key;
            oauthAuthentication.ConsumerSecret = app_consumerSecret;
            oauthAuthentication.AccessToken = app_token;
            oauthAuthentication.AccessTokenSecret = app_TokenSecret;

            // Sending the authentication, with user list and response
            var response = TwitterDirectMessage.Send(oauthAuthentication, twitterUser.ToString(), message);

            if (response.Result == RequestResult.Success)
                return "Sucess";
            else
                return response.ErrorMessage;
            
        }
    }
}

