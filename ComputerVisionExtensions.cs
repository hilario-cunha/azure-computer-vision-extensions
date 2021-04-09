using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Microsoft.Azure.CognitiveServices.Vision.ComputerVision
{
    public static class ComputerVisionExtensions
    {
        public static Task<ReadOperationResult> SendAndGetReadResultFinalStatusInLocalPathAsync(
            this IComputerVisionClient computerVisionClient, 
            string path, 
            int millisecondsDelayForPollingAfterSend,
            int millisecondsDelayForPollingToReadResult,
            string language = null, 
            IList<string> pages = null, 
            CancellationToken cancellationToken = default
        )
        {
            var fileStream = File.OpenRead(path);
            return computerVisionClient.SendAndGetReadResultFinalStatusInStreamAsync(fileStream, millisecondsDelayForPollingAfterSend, millisecondsDelayForPollingToReadResult, language, pages, cancellationToken);
        }

        public static async Task<ReadOperationResult> SendAndGetReadResultFinalStatusInStreamAsync(
            this IComputerVisionClient computerVisionClient, 
            Stream image, 
            int millisecondsDelayForPollingAfterSend,
            int millisecondsDelayForPollingToReadResult,
            string language = null, 
            IList<string> pages = null, 
            CancellationToken cancellationToken = default
        )
        {
            var textHeaders = await computerVisionClient.ReadInStreamAsync(image, language, pages, cancellationToken);
            await Task.Delay(millisecondsDelayForPollingAfterSend);

            // Extract the text
            return await computerVisionClient.GetReadResultFinalStatusAsync(textHeaders, millisecondsDelayForPollingToReadResult, cancellationToken);
        }
        
        public static Task<ReadOperationResult> GetReadResultFinalStatusAsync(
            this IComputerVisionClient computerVisionClient, 
            ReadInStreamHeaders textHeaders,
            int millisecondsDelayForPolling,
            CancellationToken cancellationToken = default
        )
        {
            return computerVisionClient.GetReadResultFinalStatusAsync(textHeaders.GetOperationId(), millisecondsDelayForPolling, cancellationToken);
        }
        
        public static async Task<ReadOperationResult> GetReadResultFinalStatusAsync(
            this IComputerVisionClient computerVisionClient, 
            Guid operationId,
            int millisecondsDelayForPolling,
            CancellationToken cancellationToken = default
        ) 
        {
            ReadOperationResult results;
            do
            {
                results = await computerVisionClient.GetReadResultAsync(operationId, cancellationToken);
                await Task.Delay(millisecondsDelayForPolling, cancellationToken);
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));
            
            return results;
        }      
    }
}