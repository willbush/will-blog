using System;
using System.Collections;
using System.Collections.Generic;

namespace will_blog.Infrastructure
{
    public class PagedData<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _currentItems;

        public int TotalCount { get; private set; }
        public int Page { get; }
        public int PostsPerPage { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasNextPage { get; private set; }
        public bool HasPreviousPage { get; private set; }

        public int NextPage
        {
            get
            {
                if (!HasNextPage)
                    throw new InvalidOperationException();

                return Page + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviousPage)
                    throw new InvalidOperationException();

                return Page - 1;
            }
        }

        public PagedData(IEnumerable<T> currentItems, int totalCount, int page, int postsPerPage)
        {
            _currentItems = currentItems;
            TotalCount = totalCount;
            Page = page;
            PostsPerPage = postsPerPage;

            TotalPages = (int) Math.Ceiling((float) TotalCount / postsPerPage);
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _currentItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}