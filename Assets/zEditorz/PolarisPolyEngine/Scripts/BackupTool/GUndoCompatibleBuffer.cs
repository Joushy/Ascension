using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Pinwheel.Griffin.BackupTool
{
    //[CreateAssetMenu(fileName = "UndoCompatibleBuffer", menuName = "Griffin/Undo Compatible Buffer")]
    public class GUndoCompatibleBuffer : ScriptableObject
    {
        private static GUndoCompatibleBuffer instance;
        public static GUndoCompatibleBuffer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GUndoCompatibleBuffer>("UndoCompatibleBuffer");
                    if (instance == null)
                    {
                        instance = ScriptableObject.CreateInstance<GUndoCompatibleBuffer>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private string currentBackupName;
        public string CurrentBackupName
        {
            get
            {
                return currentBackupName;
            }
            set
            {
                currentBackupName = value;
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            Undo.willFlushUndoRecord += OnWillUndoFlush;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            Undo.willFlushUndoRecord -= OnWillUndoFlush;
#endif
        }

        public void RecordUndo()
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(this, "Terrain Editing");
            Undo.IncrementCurrentGroup();
#endif
        }

        private void OnWillUndoFlush()
        {
            CurrentBackupName = null;
        }
    }
}
