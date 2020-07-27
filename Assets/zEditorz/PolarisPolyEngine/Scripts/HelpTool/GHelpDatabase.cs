using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pinwheel.Griffin.HelpTool
{
    //[CreateAssetMenu(fileName ="HelpDatabase", menuName ="Griffin/Help Database")]
    public class GHelpDatabase : ScriptableObject
    {
        private static GHelpDatabase instance;
        public static GHelpDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GHelpDatabase>("HelpDatabase");
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<GHelpDatabase>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private List<GHelpEntry> entries;
        public List<GHelpEntry> Entries
        {
            get
            {
                if (entries==null)
                {
                    entries = new List<GHelpEntry>();
                }
                return entries;
            }
        }
    }
}
