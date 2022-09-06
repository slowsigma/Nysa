
public record FileType(
    String Extension,
    Func<String, Int32> SearchStart,
    Func<String, Int32> SearchEnd,
    Func<String, Boolean> ExcludeLine,
    Func<Char, Boolean> HitPrefixValid,
    Func<Char, Boolean> HitSuffixValid
);
