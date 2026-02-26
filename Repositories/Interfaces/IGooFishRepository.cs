using System.Collections.Generic;
using GoIoFish.Entities;

namespace GoIoFish.Repositories.Interfaces
{
    public interface IGooFishRepository
    {
        bool AddProduct(GooFishProductEntity product);
        bool AddProducts(List<GooFishProductEntity> products);
    }
}