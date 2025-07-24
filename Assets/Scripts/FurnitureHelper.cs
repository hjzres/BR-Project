using UnityEngine;
using System.Collections.Generic;
using static Assets.Scripts.Utilities;

namespace Assets.Scripts
{
    public static class FurnitureHelper
    {
        public enum ElementType
        {
            Sitting,
            Hanging,
            Sideways
        }

        public enum WallSide
        {
            LeftX,
            RightX,
            TopZ,
            BottomZ
        }

        public static void AddFurnitureElements(GameObject parent, GameObject[] elements, int amount, float surfacePercentage, Vector3 elementOffset, ElementType elementType, Sorting.SortType sortingType, WallSide side = 0)
        {
            Collider surfaceCollider = parent.GetComponent<Collider>();
            Vector3 surfaceExtents = surfaceCollider.bounds.extents;
            surfacePercentage *= 0.01f;

            List<GameObject> generatedElements = new List<GameObject>();

            for (int i = 0; i < elements.Length; i++)
            {
                Vector3 position;
                Vector2 restrictions;
                Vector3 typeExtents = elements[i].GetComponent<Collider>().bounds.extents;

                float typeOffsetY = typeExtents.y;

                if (elementType == ElementType.Sitting || elementType == ElementType.Hanging)
                {
                    int elementDirection = elementType == ElementType.Hanging ? -1 : 1;
                    restrictions = new Vector2(surfaceExtents.x, surfaceExtents.z) * surfacePercentage;

                    for (int j = 0; j < amount; j++)
                    {
                        GameObject element = GameObject.Instantiate(elements[i], null);
                        position = Sorting.PositionSortingHelper(sortingType, restrictions, amount, j);
                        element.transform.position = new Vector3(position.x, (surfaceExtents.y + typeOffsetY) * elementDirection, position.y) + parent.transform.position + elementOffset;
                        generatedElements.Add(element);
                    }
                }

                else if (elementType == ElementType.Sideways)
                {
                    int sideOffsetMultiplier;

                    if (side == WallSide.LeftX || side == WallSide.RightX)
                    {
                        bool leftSide = side == WallSide.LeftX;
                        sideOffsetMultiplier = leftSide ? -1 : 1;

                        float offsetX = (surfaceExtents.x + typeExtents.x) * sideOffsetMultiplier;
                        
                        restrictions = new Vector2(surfaceExtents.z, surfaceExtents.y);

                        for (int j = 0; j < amount; j++)
                        {
                            GameObject element = GameObject.Instantiate(elements[i], null);
                            position = Sorting.PositionSortingHelper(sortingType, restrictions, amount, j);
                            element.transform.position = new Vector3(offsetX, position.y, position.x * sideOffsetMultiplier) + parent.transform.position + elementOffset;
                            generatedElements.Add(element); 
                        }
                    }

                    else if (side == WallSide.TopZ || side == WallSide.BottomZ)
                    {
                        bool bottomSide = side == WallSide.BottomZ;
                        sideOffsetMultiplier = bottomSide ? -1 : 1;

                        float offsetZ = (surfaceExtents.z + typeExtents.z) * sideOffsetMultiplier;

                        restrictions = new Vector2(surfaceExtents.x, surfaceExtents.y);

                        for (int j = 0; j < amount; j++)
                        {
                            GameObject element = GameObject.Instantiate(elements[i], null);
                            position = Sorting.PositionSortingHelper(sortingType, restrictions, amount, j);
                            element.transform.position = new Vector3(position.x * sideOffsetMultiplier, position.y, offsetZ) + parent.transform.position + elementOffset;
                            generatedElements.Add(element);
                        }
                    }
                }
            }

            // IMPLEMENT SCRIPTABLE OBJECT FEATURES FOR EXTRA FEATURES (ex. Naming).
            for (int i = 0; i < generatedElements.Count; i++)
            {
                Transform parentGameObject = parent.transform;
                generatedElements[i].transform.parent = parentGameObject;
            }
        }
    }
}
