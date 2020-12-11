using System.Linq;

using CMS.DocumentEngine.Types.DancingGoatMvc;
using CMS.Helpers;
using CMS.Search;

using DancingGoat.Repositories;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Models.Search
{
    public class SearchResultAboutUsItemModel : SearchResultPageItemModel
    {
        public SearchResultAboutUsItemModel(SearchResultItem resultItem, AboutUs aboutUs, IAboutUsRepository aboutUsRepository, IPageUrlRetriever pageUrlRetriever)
            : base(resultItem, aboutUs, pageUrlRetriever)
        {
            var sideStories = aboutUsRepository.GetSideStories(aboutUs.NodeAliasPath);
            Content = string.Join(" ", sideStories.Select(story => HTMLHelper.StripTags(story.AboutUsSectionText, false)));
        }
    }
}