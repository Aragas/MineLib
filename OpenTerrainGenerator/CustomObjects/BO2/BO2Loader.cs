using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    public class BO2Loader : CustomObjectLoader
    {
        public override CustomObject loadFromFile(String objectName, File file)
        {
            return new BO2(objectName, file);
        }

        public override void onShutdown()
        {
            // Stub method
        }
    }
}