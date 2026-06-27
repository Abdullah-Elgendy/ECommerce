using ECommerce.Domain.Common;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.DataSeeding
{
    internal class CatalogDataSeeder(StoreDbContext dbcontext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                //returns collection of strings that represent migration names
                var PendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync(ct);
                if(PendingMigrations.Any())
                {
                   await dbcontext.Database.MigrateAsync(ct);
                }

                var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json");
                await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json");
                await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json");

                var res = await dbcontext.SaveChangesAsync(ct);

                if (res > 0)
                    logger.LogInformation($"{res} rows added");
                else
                    logger.LogInformation("Database Already Seeded");
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
            }
        }

        private async Task SeedIfEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct = default) where T : BaseEntity<TKey>
        {
            //check if table already has data
            if (await dbcontext.Set<T>().AnyAsync(ct))
            {
                logger.LogInformation("Table Already Has Data");
                return;
            }


            var filePath = Path.Combine(rootPath, fileName);

            //check if file path exists
            if (!File.Exists(filePath))
            {
                logger.LogWarning($"{fileName} Does Not Exist");
                return;
            }

            //setting the jsonserializer options
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            //reading data from file as a stream, use 'using' to close the file once reading is done.
            using var fileStream = File.OpenRead(filePath);

            //deserialize into a c# object (List<T>)
            var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, options, ct);

            //if deserialized items has any items, add them to the database 
            if (items?.Any() ?? false)
            dbcontext.Set<T>().AddRange(items);
            
        }
    }
}
