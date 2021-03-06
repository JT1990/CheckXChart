/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Settings), true)]
    public class BasePropertyDrawer : PropertyDrawer
    {
        protected int m_Index;
        protected int m_DataSize;
        protected float m_DefaultWidth;
        protected string m_DisplayName;
        protected string m_KeyName;
        protected Rect m_DrawRect;
        protected Dictionary<string, float> m_Heights = new Dictionary<string, float>();
        protected Dictionary<string, bool> m_PropToggles = new Dictionary<string, bool>();
        protected Dictionary<string, bool> m_DataToggles = new Dictionary<string, bool>();

        public virtual string ClassName { get { return ""; } }
        public virtual List<string> IngorePropertys { get { return new List<string> { }; } }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            m_DrawRect = pos;
            m_DrawRect.height = EditorGUIUtility.singleLineHeight;
            m_DefaultWidth = pos.width;
            var list = prop.displayName.Split(' ');
            if (list.Length > 0)
            {
                if (!int.TryParse(list[list.Length - 1], out m_Index))
                {
                    m_Index = 0;
                    m_DisplayName = prop.displayName;
                    m_KeyName = prop.propertyPath + "_" + m_Index;
                }
                else
                {
                    m_DisplayName = ClassName + " " + m_Index;
                    m_KeyName = prop.propertyPath + "_" + m_Index;
                }
            }
            else
            {
                m_DisplayName = prop.displayName;
            }
            if (!m_PropToggles.ContainsKey(m_KeyName))
            {
                m_PropToggles.Add(m_KeyName, false);
            }
            if (!m_DataToggles.ContainsKey(m_KeyName))
            {
                m_DataToggles.Add(m_KeyName, false);
            }
            if (!m_Heights.ContainsKey(m_KeyName))
            {
                m_Heights.Add(m_KeyName, 0);
            }
            else
            {
                m_Heights[m_KeyName] = 0;
            }
        }

        private string GetKeyName(SerializedProperty prop)
        {
            var index = 0;
            var list = prop.displayName.Split(' ');
            if (list.Length > 0)
            {
                int.TryParse(list[list.Length - 1], out index);
            }
            return prop.propertyPath + "_" + index;
        }

        protected void AddSingleLineHeight()
        {
            m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            m_DrawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected void PropertyField(SerializedProperty prop, string relativePropName)
        {
            if (IngorePropertys.Contains(relativePropName)) return;
            if (!ChartEditorHelper.PropertyField(ref m_DrawRect, m_Heights, m_KeyName, prop, relativePropName))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativePropName);
            }
        }

        protected void PropertyField(SerializedProperty prop, SerializedProperty relativeProp)
        {
            if (!ChartEditorHelper.PropertyField(ref m_DrawRect, m_Heights, m_KeyName, relativeProp))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativeProp);
            }
        }

        protected void PropertyTwoFiled(SerializedProperty prop, string relativeListProp, string labelName = null)
        {
            PropertyTwoFiled(prop, prop.FindPropertyRelative(relativeListProp), labelName);
        }
        protected void PropertyTwoFiled(SerializedProperty prop, SerializedProperty relativeListProp, string labelName = null)
        {
            if (string.IsNullOrEmpty(labelName))
            {
                labelName = relativeListProp.displayName;
            }
            ChartEditorHelper.MakeTwoField(ref m_DrawRect, m_DefaultWidth, relativeListProp, labelName);
            m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected bool MakeFoldout(SerializedProperty prop, string relativePropName)
        {
            if (string.IsNullOrEmpty(relativePropName))
            {
                return ChartEditorHelper.MakeFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName, m_DisplayName, null);
            }
            else
            {
                var relativeProp = prop.FindPropertyRelative(relativePropName);
                return ChartEditorHelper.MakeFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName, m_DisplayName, relativeProp);
            }
        }

        protected virtual void DrawExtendeds(SerializedProperty prop)
        {
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            var key = GetKeyName(prop);
            if (m_Heights.ContainsKey(key)) return m_Heights[key] + GetExtendedHeight();
            else return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected virtual float GetExtendedHeight()
        {
            return 0;
        }
    }
}