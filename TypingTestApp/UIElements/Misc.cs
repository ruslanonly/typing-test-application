using System;
using System.Collections.Generic;
using System.Windows;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;

namespace TypingTestApp
{
    public class Misc
    {
        static public string[][] keyMap = {
            new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" },
            new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L" },
            new string[] { "Z", "X", "C", "V", "B", "N", "M" },
        };
        static public Dictionary<string, string> TranslatedKeys = new Dictionary<string, string>
        {
            { "q", "й" }, { "w", "ц" }, { "e", "у" }, { "r", "к" }, { "t", "е" }, { "y", "н" }, { "u", "г" }, { "i", "ш" }, { "o", "щ" }, { "p", "з" },
            { "a", "ф" }, { "s", "ы" }, { "d", "в" }, { "f", "а" }, { "g", "п" }, { "h", "р" }, { "j", "о" }, { "k", "л" }, { "l", "д" },
            { "z", "я" }, { "x", "ч" }, { "c", "с" }, { "v", "м" }, { "b", "и" }, { "n", "т" }, { "m", "ь" }, { ",", "б" },
        };
        static public Word[] UIElementCollectionToWordArray(UIElementCollection collection)
        {
            Word[] array = new Word[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                Word word = (Word)collection[i];
                word.Default();
                array[i] = word;

            }
            return array;
        }
        static public async ValueTask<string[]> LoadWordsGroup(WordGroup wordGroup)
        {
            try
            {
                using (StreamReader sr = new StreamReader("../../../Assets/" + wordGroup.ToString() + ".json"))
                {
                    return await JsonSerializer.DeserializeAsync<string[]>(sr.BaseStream);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                using (StreamReader sr = new StreamReader("../../../Assets/English.json"))
                {
                    return await JsonSerializer.DeserializeAsync<string[]>(sr.BaseStream);
                }
            }
        }   
    }
}
