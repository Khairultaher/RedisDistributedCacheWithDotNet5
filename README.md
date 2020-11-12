# Redis Distributed Cache With .Net 5.0
## Redis installation: https://www.c-sharpcorner.com/article/installing-redis-cache-on-windows/
## Add Microsoft.Extensions.Caching.StackExchangeRedis from nuget package
## Add the following line to ConfigureServices method of the Startup.cs 
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost:6379";
                // options.InstanceName = "Inventory";
            });

