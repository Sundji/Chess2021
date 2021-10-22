using System.IO;
using UnityEngine;

namespace Practice.Chess
{
    public static class DataReaderAndWriter
    {
        private static string _filepath = Path.Combine(Application.persistentDataPath, "MoveLibrary.json");

        public static void SaveMoveLibrary(MoveLibrary moveLibrary)
        {
            try
            {
                string json = JsonUtility.ToJson(moveLibrary);
                File.WriteAllText(_filepath, json);
            }
            catch (IOException exception)
            {
                Debug.LogError("The file did not save.\nError message:\n" + exception.Message);
            }
        }

        public static bool TryLoadMoveLibrary(out MoveLibrary moveLibrary)
        {
            try
            {
                    if (File.Exists(_filepath))
                    {
                        string json = File.ReadAllText(_filepath);
                        moveLibrary = JsonUtility.FromJson<MoveLibrary>(json);
                    }
                    else
                        moveLibrary = new MoveLibrary();
                return true;
            }
            catch (IOException exception)
            {
                Debug.LogError("The file did not load.\nError message:\n" + exception.Message);
                moveLibrary = null;
                return false;
            }
        }
    }
}