using Octokit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitFolks.Utils
{

    public class AuthorComparer : IEqualityComparer<Author>
    {
        public bool Equals(Author x, Author y)
        {
            if (x == null && y == null) return false;

            return x?.Id == y?.Id;
        }

        public int GetHashCode(Author obj)
        {
            return obj.Id;
        }
    }
}
