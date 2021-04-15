namespace CodeSwifterStarter.Common.Extensions
{
    public static class PathExtensions
    {
        public static string EnsureEndsWith(this string path, char endChar)
        {
            return path.EndsWith(endChar) ? path : (path + endChar);
        }
    }
}
