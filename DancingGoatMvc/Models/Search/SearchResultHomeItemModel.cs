using System.Linq;

using CMS.Helpers;
using CMS.Search;

using DancingGoat.Repositories;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models.Search
{
    public class SearchResultHomeItemModel : SearchResultPageItemModel
    {
        public SearchResultHomeItemModel(SearchResultItem resultItem, CMS.DocumentEngine.Types.DancingGoatMvc.Home home, IHomeRepository homeRepository, IPageUrlRetriever pageUrlRetriever)
            : base(resultItem, home, pageUrlRetriever)
        {
            var homeSections = homeRepository.GetHomeSections(home.NodeAliasPath);
            Content = string.Join(" ", homeSections.Select(section => HTMLHelper.StripTags(section.HomeSectionText, false)));
        }
    }
}