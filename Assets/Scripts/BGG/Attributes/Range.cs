using System;
using UnityEditor;
using UnityEngine;

namespace BV
{
    [System.Serializable]
    public struct Range : IEquatable<Range>
    {
        public float min;
        public float max;

        public float length { get { return (max - min); } }

        public Range(float v0, float v1)
        {
            min = v0;
            max = v1;
        }

        public float Clamp(float v)
        {
            return Mathf.Clamp(v, min, max);
        }
        public float Random()
        {
            return UnityEngine.Random.Range(min, max);
        }
        public float Evaluate(float v)
        {
            return MathX.Map(v, 0f, 1f, min, max);
        }
        public float MapFrom(float v, float inMin, float inMax)
        {
            return MathX.Map(v, inMin, inMax, min, max);
        }
        public float MapTo(float v, float outMin, float outMax)
        {
            return MathX.Map(v, min, max, outMin, outMax);
        }

        // Virtuals
        public override string ToString()
        {
            return "(" + StringX.FormatFloat(min, 3) + "," + StringX.FormatFloat(max, 3) + ")";
        }

        // IEquatable
        public bool Equals(Range other)
        {
            //			if(other == null) return false;
            return (other.min == min && other.max == max);
        }
        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;
            Range other = (Range)obj;
            return (other.min == min && other.max == max);
        }
        public static bool operator ==(Range r0, Range r1)
        {
            //			if (r0 == null && r1==null)
            //				return true;
            //			if (r0 == null || r1 == null)
            //				return false;
            return r0.Equals(r1);
        }
        public static bool operator !=(Range r0, Range r1)
        {
            return !(r0 == r1);
        }
        public override int GetHashCode()
        {
            return new Vector2(min, max).GetHashCode();
        }
        public static Range operator *(Range r, float m)
        {
            return new Range(r.min * m, r.max * m);
        }
    }

    [CustomPropertyDrawer(typeof(BV.Range))]
    public class RangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect fullRect, SerializedProperty property, GUIContent label)
        {
            Rect r = EditorGUI.PrefixLabel(fullRect, label);

            EditorGUI.BeginChangeCheck();

            SerializedProperty minProp = property.FindPropertyRelative("min");
            SerializedProperty maxProp = property.FindPropertyRelative("max");
            float min = minProp.floatValue;
            float max = maxProp.floatValue;


            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            DrawRange(r, ref min, ref max);
            EditorGUI.indentLevel = indent;

            if (EditorGUI.EndChangeCheck())
            {
                minProp.floatValue = min;
                maxProp.floatValue = max;
            }
        }

        protected virtual void DrawRange(Rect r, ref float min, ref float max)
        {
            float valueSpacer = 20f;
            float valueWidth = (r.width - valueSpacer) / 2f;

            Rect r0 = new Rect(r.x, r.y, valueWidth, r.height);
            Rect r1 = new Rect(r.x + valueWidth, r.y, valueSpacer, r.height);
            Rect r2 = new Rect(r.x + r.width - valueWidth, r.y, valueWidth, r.height);

            min = EditorGUI.FloatField(r0, min);
            EditorGUI.LabelField(r1, "to", BV.Editor.centeredLabelStyle);
            max = EditorGUI.FloatField(r2, max);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return BV.Editor.lineHeight;
        }
    }
}