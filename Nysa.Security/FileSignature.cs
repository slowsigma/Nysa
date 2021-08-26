using System;
using System.Collections.Generic;
using System.Linq;

namespace Nysa.Security
{

    public record FileSignature(
        /// <summary>
        /// The base64 encoded digital signature created from the file located by FilePath.
        /// </summary>
        String Value,
        /// <summary>
        /// The file path of the file used to create the the digital signature.
        /// </summary>
        String FilePath)
    {
    }

}
