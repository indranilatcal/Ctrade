using CTrade.Client.Core;
using CTrade.Client.DataAccess;
using CTrade.Client.DataAccess.Requests;
using CTrade.Client.DataAccess.Responses;
using CTrade.Client.Services.Entities;
using CTrade.Client.Services.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrade.Client.Services
{
    public interface ICategoryService
    {
        Task<ICategoryResponse> GetCategoriesAsync(int maxNnumberOfCategories = 0);
        Task<ICategoryResponse> GetCategoriesForSiteAsync(string siteId);
        Task<IServiceResponse> UpdateSubCategoriesAsync(string siteId, string[] subCategories);
        Task<IServiceResponse> OverrideAsync(string siteId, Category[] itemsToOverride);
    }

    public class CategoryService : ServiceBase, ICategoryService
    {
        public CategoryService(IDbRepository repository, ILogger logger) : base(repository, logger) { }

        public async Task<ICategoryResponse> GetCategoriesAsync(int maxNnumberOfCategories = 0)
        {
            ICategoryResponse categoryResponse = null;

            try
            {
                var searchRequest = SearchRequestFactory.Create(Index.DesignDoc, Index.Categories);

                if (maxNnumberOfCategories > 0)
                    searchRequest.Configure(q => q.Limit(maxNnumberOfCategories));

                searchRequest.Configure(q => q.IncludeDocs(true)
                    .Expression(string.Format(Index.DocTypeExpressionFormat, DocType.Category))
                    );

                var response = await Repository.SearchAsync(searchRequest);
                categoryResponse = new CategoryResponse(response);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryResponse;
        }

        public async Task<ICategoryResponse> GetCategoriesForSiteAsync(string siteId)
        {
            if (string.IsNullOrWhiteSpace(siteId))
                return new CategoryResponse(ErrorMessage.SiteIdIsMandatory);
            ICategoryResponse categoryResponse = null;
            try
            {
                var categoriesThatIncludeSite = await GetCategoriesThatIncludeSiteAsync(siteId);
                if (categoriesThatIncludeSite.Any())
                {
                    var categoriesIncludingSite = categoriesThatIncludeSite.Select(c => new Category
                        {
                            Id = c.Id,
                            Label = c.Label,
                            Rev = c.Rev,
                            SubCategories = c.SubCategories.Where(s => s.Sites != null &&
                                s.Sites.Any(st => st.Id == siteId)).ToArray()
                        });
                    categoryResponse = new CategoryResponse(categoriesIncludingSite);
                }
                else
                    categoryResponse = new CategoryResponse(Enumerable.Empty<Category>());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return categoryResponse;
        }

        public async Task<IServiceResponse> UpdateSubCategoriesAsync(string siteId, string[] subCategories)
        {
            if (string.IsNullOrWhiteSpace(siteId))
                return new CategoryHeaderResponse(ErrorMessage.SiteIdIsMandatory);
            subCategories = subCategories ?? new string[] { };

            IServiceResponse response = null;

            try
            {
                var categoriesResponse = await GetCategoriesAsync();
                if (!categoriesResponse.IsEmpty)
                {
                    var potentialCategoriesToUpdate = new List<Category>();
                    foreach (var c in categoriesResponse.Categories)
                    {
                        if(ProcessAdd(c, siteId, subCategories))
                            potentialCategoriesToUpdate.Add(c);
                        if(ProcessRemoval(c, siteId, subCategories) && !potentialCategoriesToUpdate.Contains(c))
                            potentialCategoriesToUpdate.Add(c);
                    }

                    if (potentialCategoriesToUpdate != null && potentialCategoriesToUpdate.Any())
                    {
                        var bulkResponse = await PersistCategories(potentialCategoriesToUpdate);
                        response = new CategoryHeaderResponse(bulkResponse.HttpHeaderInfo.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return response ?? new CategoryHeaderResponse();
        }
        
        public async Task<IServiceResponse> OverrideAsync(string siteId, Category[] itemsToOverride)
        {
            if (string.IsNullOrWhiteSpace(siteId))
                return new CategoryHeaderResponse(ErrorMessage.SiteIdIsMandatory);
            if(itemsToOverride == null || !itemsToOverride.Any())
                return new CategoryHeaderResponse(ErrorMessage.ItemsToOverrideShouldNotBeEmpty);
            IServiceResponse response = null;
            try
            {
                var categoriesThatIncludeSite = await GetCategoriesThatIncludeSiteAsync(siteId);
                if (categoriesThatIncludeSite.Any())
                {
                    var potentialCategoriesToUpdate = new List<Category>();
                    foreach (var overridenCategory in itemsToOverride)
                    {
                        var originalCategory = categoriesThatIncludeSite.FirstOrDefault(ct => ct.Id == overridenCategory.Id);
                        if (originalCategory != null && ProcessOverride(siteId, overridenCategory, originalCategory))
                            potentialCategoriesToUpdate.Add(originalCategory);
                    }

                    if (potentialCategoriesToUpdate != null && potentialCategoriesToUpdate.Any())
                    {
                        var bulkResponse = await PersistCategories(potentialCategoriesToUpdate);
                        response = new CategoryHeaderResponse(bulkResponse.HttpHeaderInfo.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return response ?? new CategoryHeaderResponse();
        }

        #region Private Helpers
        private bool ProcessOverride(string siteId, Category overridenCategory, Category originalCategory)
        {
            bool isDirty = false;
            var originalSubCategories = originalCategory.SubCategories
                .Where(s => s.Sites != null && s.Sites.Any(st => st.Id == siteId));
            var overridenSubCategories = overridenCategory.SubCategories
                .Where(s => s.Sites != null && s.Sites.Any(st => st.Id == siteId));
            var potentialUpdatePairs = originalSubCategories.Join(overridenSubCategories, o => o.Id,
                i => i.Id, (o, i) => new { Original = o, Overriden = i });
            if (potentialUpdatePairs != null && potentialUpdatePairs.Any())
            {
                foreach(var pair in potentialUpdatePairs)
                {
                    var potentialSitesToUpdate = pair.Original.Sites.Where(st => st.Id == siteId)
                        .Join(pair.Overriden.Sites, o => o.Id, i => i.Id, (o, i) => new { Original = o, Overriden = i });
                    if (potentialSitesToUpdate != null && potentialSitesToUpdate.Any())
                    {
                        foreach(var sitePair in potentialSitesToUpdate)
                        {
                            if(sitePair.Original.CategoryLabelOverride != sitePair.Overriden.CategoryLabelOverride ||
                                sitePair.Original.SubCategoryLabelOverride != sitePair.Overriden.SubCategoryLabelOverride)
                            {
                                isDirty = true;
                                sitePair.Original.CategoryLabelOverride = sitePair.Overriden.CategoryLabelOverride;
                                sitePair.Original.SubCategoryLabelOverride = sitePair.Overriden.SubCategoryLabelOverride;
                            }
                        }
                    }
                }
            }

            return isDirty;
        }
        private async Task<IEnumerable<Category>> GetCategoriesThatIncludeSiteAsync(string siteId)
        {
            IEnumerable<Category> categoriesThatIncludeSite = null;
            var allCategoriesResponse = await GetCategoriesAsync();
            if (!allCategoriesResponse.IsEmpty)
                categoriesThatIncludeSite = allCategoriesResponse.Categories
                    .Where(c => c.SubCategories.Any(s => s.Sites != null && s.Sites.Any(st => st.Id == siteId)));

            return categoriesThatIncludeSite ?? Enumerable.Empty<Category>();
        }
        private async Task<BulkResponse> PersistCategories(IEnumerable<Category> categoriesToUpdate)
        {
            var bulkRequest = BulkRequestFactory.Create();
            bulkRequest.Include(categoriesToUpdate.Select(c => c.AsJObject()).ToArray());
            var bulkResponse = await Repository.BulkAsync(bulkRequest);
            return bulkResponse;
        }

        private bool ProcessAdd(Category c, string siteId, string[] subCategories)
        {
            bool isDirty = false;
            var subCategoriesToAddSitesTo = 
                c.SubCategories.Where(s => (s.Sites == null || !s.Sites.Any(st => st.Id == siteId)) && subCategories.Contains(s.Id));

            if (subCategoriesToAddSitesTo != null && subCategoriesToAddSitesTo.Any())
            {
                foreach (var s in subCategoriesToAddSitesTo)
                {
                    var sites = s.Sites == null ? new List<Site>() : s.Sites.ToList();
                    sites.Add(new Site { Id = siteId });
                    s.Sites = sites.ToArray();
                    isDirty = true;
                }
            }

            return isDirty;
        }

        private bool ProcessRemoval(Category c, string siteId, string[] subCategories)
        {
            bool isDirty = false;
            var subCategoriesToRemoveSitesFrom =
                c.SubCategories.Where(s => (s.Sites != null && s.Sites.Any(st => st.Id == siteId)) && !subCategories.Contains(s.Id));

            if (subCategoriesToRemoveSitesFrom != null && subCategoriesToRemoveSitesFrom.Any())
            {
                foreach (var s in subCategoriesToRemoveSitesFrom)
                {
                    var sites = s.Sites.ToList();
                    sites.Remove(s.Sites.Single(st => st.Id == siteId));
                    s.Sites = sites.ToArray();
                    isDirty = true;
                }
            }

            return isDirty;
        }
        #endregion
    }
}
