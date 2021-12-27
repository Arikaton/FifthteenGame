using Newtonsoft.Json;
using UnityEngine;

namespace GameScripts.Game
{
    public class FieldModel
    {
        private static string _FIELD_PREFS_KEY = "FieldPrefsKey";
        public static bool HasSavedInstance => PlayerPrefs.HasKey(_FIELD_PREFS_KEY);

        public int[,] FieldMatrix;

        public FieldModel(Vector2Int size)
        {
            InitializeFieldMatrix(size);
        }

        public FieldModel(int[,] fieldMatrix)
        {
            FieldMatrix = fieldMatrix;
        }

        private void InitializeFieldMatrix(Vector2Int size)
        {
            FieldMatrix = new int[size.x, size.y];
            var counter = 1;
            for (var y = 0; y < size.x; y++)
            {
                for (var x = 0; x < size.y; x++)
                {
                    FieldMatrix[x, y] = counter++;
                }
            }

            FieldMatrix[size.x - 1, size.y - 1] = 0;
        }

        public void SaveToLocalDb()
        {
            var stringMatrix = JsonConvert.SerializeObject(FieldMatrix);
            PlayerPrefs.SetString(_FIELD_PREFS_KEY, stringMatrix);
        }

        public static void ClearSavedInstance() => PlayerPrefs.DeleteKey(_FIELD_PREFS_KEY);

        public static FieldModel LoadFromLocalDb()
        {
            var stringMatrix = PlayerPrefs.GetString(_FIELD_PREFS_KEY);
            var matrix = JsonConvert.DeserializeObject<int[,]>(stringMatrix);
            return new FieldModel(matrix);
        }
    }
}