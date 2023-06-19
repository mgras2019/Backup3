namespace Microsoft.CAP.DistributedCache
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using StackExchange.Redis;
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Redis Cache manager
    /// </summary>
    public static class RedisCacheManager
    {
        /// <summary>
        /// Lazy connection
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            // Use RedisCacheConnection in the localsettings.json during development
            // this will be setup in the Configuration Settings for the function
            string redisCacheConnection = Environment.GetEnvironmentVariable("RedisCacheConnection");

            return ConnectionMultiplexer.Connect(redisCacheConnection);
        });

        /// <summary>
        /// Connection
        /// </summary>
        private static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        /// <summary>
        /// Clear the Cache keys matching the pattern
        /// </summary>
        /// <param name="matchKeys"></param>
        /// <returns>RedisResult object</returns>        
        public static async Task<RedisResult> ClearCacheKeys(string matchKeys)
        {
            var database = Connection.GetDatabase();
            // LUA script to clear the matching keys
            // this will return the number of keys deleted or 0 if no matching keys found
            // result will be Integer
            string clearRedisKeysCommand =
                $"local keyList = redis.call('KEYS', '{matchKeys}')" +
                " if #keyList > 0 then " +
                $" return redis.call('del', unpack(redis.call('keys','{matchKeys}'))) " +
                " else " +
                " return 0" +
               " end";
            return await database.ScriptEvaluateAsync(clearRedisKeysCommand);
        }


        [Function("ClearCacheKeys")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
            FunctionContext executionContext)
        {
            var log = executionContext.GetLogger("ClearCacheKeys");
            log.LogInformation("Distributed cache trigger function processed a request.");
            
            HttpStatusCode httpStatusCode;
            string message = string.Empty;
            
            // check if the right parameters are passed in the http request
            StreamReader reader = new StreamReader(req.Body);
            string responseBody = reader.ReadToEnd();
            try
            {
                CacheApplication cacheApplication = JsonConvert.DeserializeObject<CacheApplication>(responseBody);
                RedisResult result = await ClearCacheKeys(cacheApplication.MatchKeys);
                httpStatusCode = HttpStatusCode.OK;
                // result will be integer since we are returning integer value from the LUA script in ClearCacheKeys function
                message = $"{result} keys cleared from cache for application {cacheApplication.ApplicationName}, key pattern {cacheApplication.MatchKeys}";
            }
            catch (JsonSerializationException serializerException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
                message = $"Bad Request. ApplicationName and MatchKeys are mandatory. Exception: {serializerException.Message}";
                log.LogInformation(message);
            }
            catch (Exception ex)
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                message = $"Error occured while deleting cache keys. Error: {ex.Message}";
                log.LogInformation(message);
            }
            var response = req.CreateResponse(httpStatusCode);
            response.Body = new MemoryStream(Encoding.UTF8.GetBytes(message ?? ""));
            return response;
            
        }
    }
}
