namespace TestLocalization.Helpers.Routing;

public static class ListExtensions
{
    public static void Remove<T>(this IList<T> list, Type type)
    {
        var items = list.Where(x => x.GetType() == type).ToList();
        items.ForEach(x => list.Remove(x));
    }
}
