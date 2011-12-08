using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using TImport = System.String;

namespace cocos2d.Content.Pipeline.Importers
{
    [ContentImporter(".*", DisplayName = "Text Importer", DefaultProcessor = "TextImporter")]
    public class TextImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return System.IO.File.ReadAllText(filename);
        }
    }
}
