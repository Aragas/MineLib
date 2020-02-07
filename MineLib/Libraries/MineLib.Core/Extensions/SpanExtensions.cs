namespace System
{
    public static class SpanExtensions
    {
        public static bool All<T>(this Span<T> span, Func<T, bool> func)
        {
            for(var i = 0; i < span.Length; i++)
                if (!func(span[i]))
                    return false;
            return true;
        }
    }
}