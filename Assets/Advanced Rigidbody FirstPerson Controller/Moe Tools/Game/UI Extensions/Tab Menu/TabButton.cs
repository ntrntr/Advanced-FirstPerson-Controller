using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Moe.Tools;

namespace Moe.Tools
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        [SerializeField]
        TabMenu menu;
        public TabMenu Menu { get { return menu; } set { menu = value; } }

        [SerializeField]
        int index;
        public int Index { get { return index; } set { index = value; } }

        void Awake()
        {
            menu.InitButton(GetComponent<Button>(), index);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(TabButton))]
        public class Inspector : InspectorBase<TabButton>
        {
            InspectorCustomGUI gui;
            SerializedProperty menu;
            public TabMenu MenuObject { get { return menu.objectReferenceValue as TabMenu; } }

            ListPopup<TabMenu.Tab> index;

            protected override void OnEnable()
            {
                base.OnEnable();

                menu = serializedObject.FindProperty("menu");

                gui = new InspectorCustomGUI(serializedObject);
                gui.Overrides.Add(menu.displayName, DrawMenu);
                gui.Overrides.Add("index", DrawIndex);

                InitIndex();
            }

            protected virtual void InitIndex()
            {
                if (menu.objectReferenceValue)
                    index = new ListPopup<TabMenu.Tab>(serializedObject.FindProperty("index"), MenuObject.Tabs, delegate (TabMenu.Tab tab) { return tab.Name; });
                else
                    index = null;
            }

            public override void OnInspectorGUI()
            {
                EditorGUILayout.Space();

                gui.Draw();

                serializedObject.ApplyModifiedProperties();
            }

            protected virtual void DrawMenu()
            {
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUILayout.PropertyField(menu);
                }
                if (EditorGUI.EndChangeCheck())
                    InitIndex();
            }

            protected virtual void DrawIndex()
            {
                if (index != null)
                    index.Draw();
            }
        }
#endif
    }
}