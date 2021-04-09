namespace Microsoft.Azure.CognitiveServices.Vision.ComputerVision
{
    public static class ComputerVisionClientFactory 
    {
        public static ComputerVisionClient Create(string subscriptionKey, string endpoint)
        {
            if(subscriptionKey == null || subscriptionKey.Length == 0) { new System.ArgumentNullException("subscriptionKey"); }
            if(endpoint == null || endpoint.Length == 0) { new System.ArgumentNullException("endpoint"); }

            var credentials = new ApiKeyServiceClientCredentials(subscriptionKey);
            return new ComputerVisionClient(credentials) { Endpoint = endpoint };
        }
    }
}
