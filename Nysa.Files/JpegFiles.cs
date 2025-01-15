using System.Drawing;

using Nysa.Logics;

namespace Nysa.Files;

public static class JpegFiles
{
    public static Option<Image> GetImage(this String filePath)
        => Return.Try(() => { return Image.FromFile(filePath); })
                 .Match(i => i.Some(), e => Option.None);

                 
}
