using System.Collections.Generic;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.DancingGoatMvc;

using Kentico.Content.Web.Mvc;

namespace DancingGoat.Repositories.Implementation
{
    /// <summary>
    /// Represents a collection of articles.
    /// </summary>
    public class KenticoArticleRepository : IArticleRepository
    {
        private readonly IPageDataContextRetriever pageDataContextRetriever;
        private readonly IPageRetriever pageRetriever;


        /// <summary>
        /// Initializes a new instance of the <see cref="KenticoArticleRepository"/> class that returns articles.
        /// </summary>
        /// <param name="pageRetriever">Retriever for pages based on given parameters.</param>
        /// <param name="pageDataContextRetriever">Retriever for page data context.</param>
        public KenticoArticleRepository(IPageRetriever pageRetriever, IPageDataContextRetriever pageDataContextRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextRetriever = pageDataContextRetriever;
        }


        /// <summary>
        /// Returns an enumerable collection of articles ordered by the date of publication. The most recent articles come first.
        /// </summary>
        /// <param name="nodeAliasPath">The node alias path of the articles section in the content tree.</param>
        /// <param name="count">The number of articles to return. Use 0 as value to return all records.</param>
        public IEnumerable<Article> GetArticles(string nodeAliasPath, int count = 0)
        {
            return pageRetriever.Retrieve<Article>(
                query => query
                    .Path(nodeAliasPath, PathTypeEnum.Children)
                    .TopN(count)
                    .OrderByDescending("DocumentPublishFrom"),
                cache => cache
                    .Key($"{nameof(KenticoArticleRepository)}|{nameof(GetArticles)}|{nodeAliasPath}|{count}")
                    // Include path dependency to flush cache when a new child page is created.
                    .Dependencies((_, builder) => builder.PagePath(nodeAliasPath, PathTypeEnum.Children)));
        }


        /// <summary>
        /// Returns the current article based on the current page data context.
        /// </summary>
        public Article GetCurrent()
        {
            var page = pageDataContextRetriever.Retrieve<Article>().Page;

            return pageRetriever.Retrieve<Article>(
                query => query
                    .WhereEquals("NodeID", page.NodeID),
                cache => cache
                    .Key($"{nameof(KenticoArticleRepository)}|{nameof(GetCurrent)}|{page.NodeID}")
                    .Dependencies((articles, deps) => deps.Pages(articles.First().Fields.RelatedArticles)))
                .First();
        }
    }
}