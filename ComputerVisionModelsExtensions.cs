using System;

namespace Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models
{
    public static class ComputerVisionModelsExtensions
    {
        public static Guid GetOperationId(this ReadInStreamHeaders textHeaders)
        {
            string operationLocation = textHeaders.OperationLocation;
            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            var operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            return Guid.Parse(operationId);
        }
    }
}
