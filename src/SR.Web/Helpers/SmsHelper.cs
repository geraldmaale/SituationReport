using System.Net;
using GreatIdeas.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using SR.Shared;
using SR.Shared.DTOs.Messaging;
using SR.Shared.Params;

namespace SR.Web.Helpers;

public static class SmsHelper
{
    public static ApiResult SendMessage(MessageParams messageParams, SmsApiDto smsApiModel)
    {
        try
        {
            //hubtel send a text message.
            string url = "https://smsc.hubtel.com/v1/messages/send";

            var queryStringParam = new Dictionary<string, string>
            {
	            ["from"] = smsApiModel.SenderId,
	            ["to"]= messageParams.To,
	            ["content"]= messageParams.Content,
	            ["clientid"] = smsApiModel.ClientId,
	            ["clientsecret"] = smsApiModel.ClientSecret
            };
            
            WebClient client = new ();

            client.DownloadStringAsync(new Uri(url));

            return new ApiResult()
            {
                IsSuccessful = true,
                Message = $"Message sent to {messageParams.To} successfully"
            };
        }
        catch (Exception)
        {
            return new ApiResult(){
                Message = $"Message failed to send to {messageParams.To}",
            };
        }
    }
    
    public static async ValueTask<ApiResult> SendMessage(MessageParams messageParams, 
        SmsApiDto smsApiModel, CancellationToken cancellationToken)
    {
        try
        {
            //hubtel send a text message.
            HttpClient httpClient = new HttpClient();
                
            string url = "https://smsc.hubtel.com/v1/messages/send?" +
                         "from=" + smsApiModel.SenderId +
                         "&to=" + messageParams.To +
                         "&content=" + messageParams.Content +
                         "&clientid=" + smsApiModel.ClientId +
                         "&clientsecret=" + smsApiModel.ClientSecret;
            
            /*string url = "https://smsc.hubtel.com/v1/messages/send";

            var queryString = new QueryBuilder();
            queryString.Add("from", messageParams.From);
            queryString.Add("to", messageParams.To);
            queryString.Add("content", messageParams.Content);
            queryString.Add("clientId", smsApiModel.ClientId);
            queryString.Add("clientsecret", smsApiModel.ClientSecret);
            
            HttpClient client = new ();
            var response = await client.PostAsync(url, new FormUrlEncodedContent(queryString));
            */

            var response = await httpClient.GetAsync(url, cancellationToken: cancellationToken);
                
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
                
            if (!response.IsSuccessStatusCode)
            {
                return new ApiResult<dynamic>()
                {
                    Message = $"Message failed to send to {messageParams.To}",
                };
            }
            
            return new ApiResult<dynamic>()
            {
                IsSuccessful = true,
                Message = $"Message sent successfully to {messageParams.To}",
            };

        }
        catch (Exception)
        {
            return new ApiResult(){Message = StatusLabels.UnprocessableError};
        }
    }
}