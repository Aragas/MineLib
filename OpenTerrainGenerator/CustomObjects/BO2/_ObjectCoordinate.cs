using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTerrainGenerator.CustomObjects
{
    public class ObjectCoordinate
    {
        public int x;
        public int y;
        public int z;
        private int hash;
        public LocalMaterialData material;
        public int BranchDirection;
        public int BranchOdds;


        public ObjectCoordinate(int _x, int _y, int _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
            this.BranchDirection = -1;
            this.BranchOdds = -1;

            hash = x + z << 8 + y << 16;
        }


    public override bool Equals(Object obj)
        {
            //if (obj instanceof ObjectCoordinate)
            //{
            //    ObjectCoordinate object = (ObjectCoordinate)obj;
            //    return object.x == this.x && object.y == this.y && object.z == this.z;
            //}
            return false;
        }

    public override int GetHashCode()
        {
            return hash;
        }

        public ObjectCoordinate Rotate()
        {
            ObjectCoordinate newCoordinate = new ObjectCoordinate(this.z, this.y, (this.x * -1));
            newCoordinate.material = material.rotate();
            newCoordinate.BranchOdds = this.BranchOdds;

            if (this.BranchDirection != -1)
            {
                newCoordinate.BranchDirection = this.BranchDirection + 1;
                if (newCoordinate.BranchDirection > 3)
                    newCoordinate.BranchDirection = 0;
            }

            return newCoordinate;

        }


        public static bool isCoordinateString(String key)
        {
            String[] coordinates = key.Split(',');
            return coordinates.Length == 3;
        }

        public static ObjectCoordinate getCoordinateFromString(String key, String value)
        {
            String[] coordinates = key.Split(new[] { ',' }, 3);
            if (coordinates.Length != 3)
                return null;

            try
            {

                int x = int.Parse(coordinates[0]);
                int z = int.Parse(coordinates[1]);
                int y = int.Parse(coordinates[2]);

                ObjectCoordinate newCoordinate = new ObjectCoordinate(x, y, z);


                String workingDataString = value;
                if (workingDataString.Contains("#"))
                {
                    String[] stringSet = workingDataString.Split('#');
                    workingDataString = stringSet[0];
                    String[] branchData = stringSet[1].Split('@');
                    newCoordinate.BranchDirection = int.Parse(branchData[0]);
                    newCoordinate.BranchOdds = int.Parse(branchData[1]);

                }
                newCoordinate.material = TerrainControl.readMaterial(workingDataString);

                return newCoordinate;

            } catch (FormatException e)
            {
                return null;

            } catch (Exception e)
            {
                return null;

            }


        }


    }
}