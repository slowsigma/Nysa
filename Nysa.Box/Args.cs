using Nysa.Logics;

public static class args
{
    public static Boolean Flag(this String[] args, String name)
        => args.Any(a => a.Equals($"-{name}", StringComparison.OrdinalIgnoreCase) || a.Equals($"/{name}", StringComparison.OrdinalIgnoreCase));
    public static Option<String> Value(this String[] args, String name)
        => args.SkipWhile(a => !a.Equals($"-{name}", StringComparison.OrdinalIgnoreCase) && !a.Equals($"/{name}", StringComparison.OrdinalIgnoreCase)).Skip(1).FirstOrNone();

}