using System.Text.RegularExpressions;

namespace worldStockLab.Web.Utils
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string title)
        {
            // Convert to lower case
            string slug = title.ToLowerInvariant();
            // Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            // Replace multiple spaces with a single space
            slug = Regex.Replace(slug, @"\s+", " ").Trim();
            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s", "-");
            return slug;
        }
    }
}
