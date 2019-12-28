using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{

    public interface CustomObjectLoader
    {
        /**
         * Returns a CustomObject with the given name and file. The object shouldn't yet be initialisized.
         *
         * @param objectName Name of the object.
         * @param file       File of the object.
         * @return The object.
         */
        public CustomObject loadFromFile(String objectName, File file);

        /**
         * Called whenever Open Terrain Generator is being shut down / reloaded.
         */
        public void onShutdown();
    }
}