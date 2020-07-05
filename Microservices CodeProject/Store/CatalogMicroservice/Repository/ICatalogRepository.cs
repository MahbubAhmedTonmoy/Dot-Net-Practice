using CatalogMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogMicroservice.Repository
{
    public interface ICatalogRepository
    {
        List<CatalogItem> GetCatalogItems();
        CatalogItem GetCatalogItem(Guid catalogItemId);
        void InsertCatalogItem(CatalogItem catalogItem);
        void UpdateCatalogItem(CatalogItem catalogItem);
        void DeleteCatalogItem(Guid catalogItemId);
    }
}
