using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

using TImport = System.String;
using System.IO;

namespace cocos2d.Content.Pipeline.Importers.TMX
{
    [ContentImporter(".TMX", DisplayName = "TMX Importer", DefaultProcessor = "TMX Processor")]
    public class TMXImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            return File.ReadAllText(filename);
            throw new NotImplementedException();
        }
    }
}
