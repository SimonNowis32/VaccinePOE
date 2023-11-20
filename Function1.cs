using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

public static class ProcessQueueMessage
{
    [FunctionName("ProcessQueueMessage")]
    public static async Task Run(
        [QueueTrigger("myqueue")] string message,
        ILogger log,
        ExecutionContext context)
    {
        log.LogInformation($"Processing message: {message}");

        var config = new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        string connectionString = config.GetConnectionString("MyConnectionString");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string[] fields = message.Split(':');

            string sql = $"INSERT INTO Vaccinations (Barcode, Date, Center, ID) VALUES ('{fields[0]}', '{fields[1]}', '{fields[2]}', '{fields[3]}')";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                int rowsAffected = await command.ExecuteNonQueryAsync();
                log.LogInformation($"{rowsAffected} rows affected.");
            }
        }
    }
}