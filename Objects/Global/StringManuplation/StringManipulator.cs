﻿using Objects.Global.StringManuplation.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.Global.StringManuplation
{
    public class StringManipulator : IStringManipulator
    {
        public string Manipulate(List<KeyValuePair<string, string>> replacementKeyValuePair, string stringToBeManipulated)
        {
            string updatedMessage = stringToBeManipulated;
            foreach (KeyValuePair<string, string> kvp in replacementKeyValuePair)
            {
                updatedMessage = updatedMessage.Replace(kvp.Key, kvp.Value);
            }

            return updatedMessage;
        }

        public string UpdateTargetPerformer(string performer, string target, string message)
        {
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
            keyValuePairs.Add(new KeyValuePair<string, string>("{performer}", performer));
            keyValuePairs.Add(new KeyValuePair<string, string>("{target}", target));
            string updatedMessage = GlobalReference.GlobalValues.StringManipulator.Manipulate(keyValuePairs, message);

            return updatedMessage;
        }
    }
}
