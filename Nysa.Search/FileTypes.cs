using Nysa.Logics;

public static class FileTypes
{
    public static Boolean LineStartsWith(String search, String sequence)
        => search.Trim().StartsWith(sequence);

    public static readonly Func<String, Int32> StartZero = s => 0;
    public static readonly Func<String, Int32> SearchAll = s => s.Length;
    public static readonly Func<String, Boolean> ExcludeFalse = s => false;
    public static readonly Func<Char, Boolean> CharGood = c => true;

    public static Func<String, Int32> StartZeroWhen(Func<String, Boolean> when)
        => s => when(s) ? 0 : s.Length;

    public static Func<String, Int32> SearchUntil(String sequence)
        => s => s.IndexOf(sequence).Make(e => e < 0 ? s.Length : e);

    public static Func<String, Boolean> ExcludeLinesStartingWith(params String[] lineStarts)
        => s => s.Trim().Make(t => lineStarts.Any(l => t.StartsWith(l)));

    private static Boolean PrefixValidCStyle(Char character)
        =>    !Char.IsLetter(character)
           && !character.Equals('.')
           && !character.Equals('{');

    private static Boolean SuffixValidCStyle(Char character)
        => character == '.' || character == '\\' || character == ']' || character == '"';

    private static Boolean PrefixValidSqlStyle(Char character)
        => character == ' ' || character == '[' || character == '"';

    private static Boolean SuffixValidSqlStyle(Char character)
        => character == '.' || character == ']' || character == '"';

    private static Boolean PrefixValidPsStyle(Char character)
        =>    !Char.IsLetter(character)
           && !character.Equals('.');

    private static Boolean SuffixValidPsStyle(Char character)
        => character == '.' || character == '\'' || character == ']' || character == '"';

    private static IReadOnlyList<FileType> All()
        => new FileType[]
        {
            new FileType(".cs",
                         StartZeroWhen(s => !LineStartsWith(s, "//")),
                         SearchAll,
                         ExcludeLinesStartingWith("[System.Xml.Serialization.XmlTypeAttribute",
                                                  "[System.Xml.Serialization.XmlRootAttribute",
                                                  "[Description(\"",
                                                  "[MessageHandler(\"",
                                                  "throw new "),
                        PrefixValidCStyle,
                        SuffixValidCStyle),
            new FileType(".js", StartZeroWhen(s => !LineStartsWith(s, "//")), SearchAll, ExcludeFalse, PrefixValidCStyle, SuffixValidCStyle),
            new FileType(".resx", StartZero, SearchAll, ExcludeFalse, CharGood, CharGood),
            new FileType(".xls", StartZero, SearchAll, ExcludeFalse, CharGood, CharGood),
            new FileType("xslt", StartZero, SearchAll, ExcludeFalse, CharGood, CharGood),
            new FileType(".cls", StartZeroWhen(s => !LineStartsWith(s, "'")), SearchAll, ExcludeFalse, CharGood, CharGood),
            new FileType(".bas", StartZeroWhen(s => !LineStartsWith(s, "'")), SearchAll, ExcludeFalse, CharGood, CharGood),
            new FileType(".ps1", StartZeroWhen(s => !LineStartsWith(s, "#")), SearchAll, ExcludeFalse, PrefixValidPsStyle, SuffixValidPsStyle),
            new FileType(".sql", StartZeroWhen(s => !LineStartsWith(s, "--")), SearchAll, ExcludeFalse, PrefixValidSqlStyle, SuffixValidSqlStyle) 
        };

    public static IReadOnlyDictionary<String, FileType> Index()
        => All().ToDictionary(k => k.Extension, v => v, StringComparer.OrdinalIgnoreCase);

}