using System.Collections.Generic;
using GoIoFish.Entities;
using GoIoFish.Helpers;
using GoIoFish.Repositories.Interfaces;

namespace GoIoFish.Repositories.Implementations
{
    public class GooFishRepository : IGooFishRepository
    {
        private readonly DbContext _dbContext;
        
        public GooFishRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public bool AddProduct(GooFishProductEntity product)
        {
            _dbContext.Db.Insertable(product).ExecuteCommand();
            return true;
        }

        public bool AddProducts(List<GooFishProductEntity> products)
        {
            _dbContext.Db.Insertable(products).ExecuteCommand();
            return true;
        }
    }
}