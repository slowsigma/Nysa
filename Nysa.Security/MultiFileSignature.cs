using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Security
{

    public record MultiFileSignature
    {
        /// <summary>
        /// The base64 encoded digital signature created from the files located by Files.
        /// </summary>
        public String Value { get; init; }
        /// <summary>
        /// The files used to create the the digital signature. These may be
        /// relative or full paths.
        /// </summary>
        public IReadOnlyList<String> Files { get; init; }

        public MultiFileSignature(String value, IEnumerable<String> files)
        {
            this.Value = value;
            this.Files = files.ToList();
        }
    }

}
