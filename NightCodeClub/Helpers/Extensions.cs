public static class Extensions {
    public static bool find<T>(this T[] array, T target) {
        return Array.Exists(array, x => x.Equals(target));
    }
}