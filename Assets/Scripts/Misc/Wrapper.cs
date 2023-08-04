using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class Wrapper
{
    /// <summary>
    /// Reads data from a json, returning a list of dictionaries
    /// </summary>
    /// <param name="json">The path to the json file</param>
    /// <returns>The data from the json file</returns>
    public static List<Dictionary<string, string>> LoadJsonData(this string json)
    {
        // load the file
        StreamReader stream = new StreamReader(json);

        // read the text
        string text = stream.ReadToEnd();

        // close the stream
        stream.Close();

        // serialize and return the data
        return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(text);
    }


    /// <summary>
    /// Loads in a variable from a dictionary if it exists
    /// </summary>
    /// <typeparam name="T">The type of variable</typeparam>
    /// <param name="data">The dictionary</param>
    /// <param name="variable">The variable</param>
    /// <param name="variableName">The name of the variable in the dictionary</param>
    public static void Load<T>(this Dictionary<string, string> data, ref T variable, string variableName) where T : IConvertible
    {
        // if the variable is in the data
        if (data.ContainsKey(variableName))
        {
            // then set the variable to the value in the dictionary (type cast it)
            variable = (T)Convert.ChangeType(data[variableName], typeof(T));
        }
    }
}
