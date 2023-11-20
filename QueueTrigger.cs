using System;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

class Program
{
    static async Task Main(string[] args)
    {
        // Retrieve the storage account from the connection string.
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("<DefaultEndpointsProtocol=https;AccountName=vaccinedata;AccountKey=6MO/XJbCtelsDrqPP9Et50+yV+5v942W1NkIPO+/ArfamA45W3I3KvlJz6fOAZX0DOwyRiJEKgxA+AStSkssmA==;EndpointSuffix=core.windows.net>");

        // Create a queue client for interacting with the queue service.
        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

        // Retrieve a reference to a queue.
        CloudQueue queue = queueClient.GetQueueReference("myqueue");

        // Create the queue if it doesn't already exist.
        await queue.CreateIfNotExistsAsync();

        // Add messages to the queue.
        await queue.AddMessageAsync(new CloudQueueMessage("vaccineBarcode:vaccinationDate:vaccinationCenter:0662708959"));
        await queue.AddMessageAsync(new CloudQueueMessage("vaccineBarcode:vaccinationDate:vaccinationCenter:peloClinic"));
        await queue.AddMessageAsync(new CloudQueueMessage("vaccineBarcode:vaccinationDate:vaccinationCenter:0762741848"));
        await queue.AddMessageAsync(new CloudQueueMessage("vaccineBarcode:vaccinationDate:vaccinationCenter:peloClinic2"));
        await queue.AddMessageAsync(new CloudQueueMessage("vaccineBarcode:vaccinationDate:vaccinationCenter:peloClinic3"));


        Console.WriteLine("Messages added to queue.");
    }
}