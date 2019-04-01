using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorEnahnce
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEnhanceInspector : EnhanceInspector
    {
    }


    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class EnhanceInspector : Editor
    {
        private readonly Dictionary<Type, string> _typeDisplayName = new Dictionary<Type, string>
        {
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(int), "int"},
            {typeof(long), "long"},
            {typeof(string), "string"},
            {typeof(bool), "bool"},
            {typeof(Color), "Color"},
            {typeof(Vector3), "Vector3"},
            {typeof(Vector2), "Vector2"},
            {typeof(Quaternion), "Quaternion"},
            {typeof(Vector4), "Vector4"}
        };

        private readonly Dictionary<Type, ParameterDrawer> _typeDrawer = new Dictionary<Type, ParameterDrawer>
        {
            {typeof(float), DrawFloatParameter},
            {typeof(double), DrawDoubleParameter},
            {typeof(int), DrawIntParameter},
            {typeof(long), DrawLongParameter},
            {typeof(string), DrawStringParameter},
            {typeof(bool), DrawBoolParameter},
            {typeof(Color), DrawColorParameter},
            {typeof(Vector3), DrawVector3Parameter},
            {typeof(Vector2), DrawVector2Parameter},
            {typeof(Quaternion), DrawQuaternionParameter},
            {typeof(Vector4), DrawVector4Parameter}
        };

        private readonly List<Type> _supportTypes = new List<Type>
        {
            typeof(float),
            typeof(double),
            typeof(int),
            typeof(long),
            typeof(string),
            typeof(bool),
            typeof(Color),
            typeof(Vector3),
            typeof(Vector2),
            typeof(Quaternion),
            typeof(Vector4),
            typeof(Object)
        };


        private EditorButtonState[] _editorButtonStates;


        private IEnumerable<MemberInfo> _members;
        private IEnumerable<MemberInfo> _methods;

        public virtual void OnEnable()
        {
            var members = target.GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            _members = members.Where(o => Attribute.IsDefined(o, typeof(ShowInInspectorAttribute)));
            _methods = members.Where(o => Attribute.IsDefined(o, typeof(InspectorButtonAttribute)));
        }

        private bool _showAllValue;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
//            if (Application.isPlaying)
//            {
//                foreach (var memberInfo in _members)
//                    if (memberInfo != null)
//                    {
//                        switch (memberInfo)
//                        {
//                            case PropertyInfo propertyInfo:
//                                if (_supportTypes.Contains(propertyInfo.PropertyType) ||
//                                    propertyInfo.PropertyType.IsEnum)
//                                {
//                                    if (!_showAllValue)
//                                    {
//                                        var attribute =
//                                            (ShowInInspectorAttribute) Attribute.GetCustomAttribute(propertyInfo,
//                                                typeof(ShowInInspectorAttribute));
//                                        var showName = string.IsNullOrEmpty(attribute.ShowName)
//                                            ? propertyInfo.Name
//                                            : attribute.ShowName;
//                                        var color = attribute.Color;
//
//
//                                        if ((!attribute.Writeable || !propertyInfo.CanWrite))
//                                        {
//                                            try
//                                            {
//                                                PropertyReadOnly(showName, color, propertyInfo.GetValue(target, null));
//                                            }
//                                            catch (Exception)
//                                            {
//                                                // ignored
//                                            }
//                                        }
//
//                                        if (attribute.Writeable && propertyInfo.CanWrite)
//                                            PropertyInspector(showName, color, propertyInfo, target);
//                                    }
//                                    else
//                                    {
//                                        var showName = propertyInfo.Name;
//                                        try
//                                        {
//                                            PropertyReadOnly(showName, Color.red, propertyInfo.GetValue(target, null));
//                                        }
//                                        catch (Exception)
//                                        {
//                                            // ignored
//                                        }
//                                    }
//                                }
//
//                                break;
//                            case FieldInfo fieldInfo:
//                                if (_supportTypes.Contains(fieldInfo.FieldType) || fieldInfo.FieldType.IsEnum)
//                                {
//                                    if (!_showAllValue)
//                                    {
//                                        if (fieldInfo.IsPublic && !fieldInfo.IsStatic) continue;
//                                        var attribute =
//                                            (ShowInInspectorAttribute) Attribute.GetCustomAttribute(fieldInfo,
//                                                typeof(ShowInInspectorAttribute));
//                                        var showName = string.IsNullOrEmpty(attribute.ShowName)
//                                            ? fieldInfo.Name
//                                            : attribute.ShowName;
//                                        var color = attribute.Color;
//                                        if (attribute.Writeable)
//                                        {
//                                            FieldInspector(showName, color, fieldInfo);
//                                        }
//                                        else
//                                        {
//                                            try
//                                            {
//                                                PropertyReadOnly(showName, color,
//                                                    fieldInfo.GetValue(target).ToString());
//                                            }
//                                            catch (Exception)
//                                            {
//                                                // ignored
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        var showName = fieldInfo.Name;
//                                        try
//                                        {
//                                            PropertyReadOnly(showName, Color.red,
//                                                fieldInfo.GetValue(target).ToString());
//                                        }
//                                        catch (Exception)
//                                        {
//                                            // ignored
//                                        }
//                                    }
//                                }
//
//                                break;
//                        }
//                    }
//
//                if (!_showAllValue)
//                {
//                    if (GUILayout.Button("ShowAllValue", GUILayout.ExpandWidth(true)))
//                    {
//                        var members = target.GetType()
//                            .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
//                                        BindingFlags.NonPublic);
//                        _members = members;
//                        _showAllValue = true;
//                    }
//                }
//                else
//                {
//                    if (GUILayout.Button("HideValue", GUILayout.ExpandWidth(true)))
//                    {
//                        var members = target.GetType()
//                            .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
//                                        BindingFlags.NonPublic);
//                        _members = members.Where(o => Attribute.IsDefined(o, typeof(ShowInInspectorAttribute)));
//                        _showAllValue = false;
//                    }
//                }
//            }


            var methodIndex = 0;

            if (_editorButtonStates == null)
                CreateEditorButtonStates(_methods.Select(member => (MethodInfo) member).ToArray());

            foreach (var memberInfo in _methods)
            {
                var method = memberInfo as MethodInfo;
                DrawButtonforMethod(method, GetEditorButtonState(method, methodIndex));
                methodIndex++;
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        private void PropertyReadOnly(string showName, Color color, object value)
        {
            var oc = GUI.color;
            var hasColor = color != Color.clear;

            if (hasColor)
            {
                GUI.color = color;
            }

            var str = value == null ? "null" : value.ToString();
            var height = EditorStyles.label.CalcHeight(new GUIContent(str), EditorGUIUtility.fieldWidth);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(showName);
            EditorGUILayout.SelectableLabel(str, hasColor ? EditorStyles.whiteBoldLabel : EditorStyles.label,
                GUILayout.MinHeight(height));
            EditorGUILayout.EndHorizontal();
            if (hasColor)
            {
                GUI.color = oc;
            }
        }

        private void PropertyInspector(string showName, Color color, PropertyInfo propertyInfo, object targte)
        {
            try
            {
                var oc = GUI.color;
                var hasColor = color != Color.clear;
                if (hasColor)
                {
                    GUI.color = color;
                }

                var t = propertyInfo.PropertyType;
                var value = propertyInfo.GetValue(targte, null);
                if (t == typeof(float))
                {
                    var orignal = (float) value;
                    var newValue = EditorGUILayout.DelayedFloatField(showName, orignal);
                    if (!Mathf.Approximately(newValue, orignal))
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(double))
                {
                    var orignal = (double) value;
                    var newValue = EditorGUILayout.DelayedDoubleField(showName, orignal);
                    if (!Mathf.Approximately((float) newValue, (float) orignal))
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(int))
                {
                    var orignal = (int) value;
                    var newValue = EditorGUILayout.DelayedIntField(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(string))
                {
                    var orignal = (string) value;
                    var newValue = EditorGUILayout.DelayedTextField(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(bool))
                {
                    var orignal = (bool) value;
                    var newValue = EditorGUILayout.Toggle(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(Color))
                {
                    var orignal = (Color) value;
                    var newValue = EditorGUILayout.ColorField(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(Quaternion))
                {
                    var orignal = (Quaternion) value;
                    var vect4 = EditorGUILayout.Vector4Field(showName, new Vector4(orignal.x,orignal.y,orignal.z,orignal.w));
                    var newValue = new Quaternion(vect4.x, vect4.y, vect4.z, vect4.w);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(Vector3))
                {
                    var orignal = (Vector3) value;
                    var newValue = EditorGUILayout.Vector3Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(Vector4))
                {
                    var orignal = (Vector4) value;
                    var newValue = EditorGUILayout.Vector4Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t == typeof(Vector2))
                {
                    var orignal = (Vector2) value;
                    var newValue = EditorGUILayout.Vector2Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (t.IsEnum)
                {
                    var orignal = (Enum) value;
                    var newValue = t.IsDefined(typeof(FlagsAttribute), true)
                        ? EditorGUILayout.EnumFlagsField(showName, orignal)
                        : EditorGUILayout.EnumPopup(showName, orignal);
                    if (!Equals(orignal, newValue))
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }
                else if (typeof(Object).IsAssignableFrom(t))
                {
                    var orignal = (Object) value;
                    var newValue = EditorGUILayout.ObjectField(showName, orignal, t, true);
                    if (newValue != orignal)
                    {
                        propertyInfo.SetValue(target, newValue, null);
                    }
                }

                if (hasColor)
                {
                    GUI.color = oc;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void FieldInspector(string showName, Color color, FieldInfo fileInfo)
        {
            try
            {
                var oc = GUI.color;
                var hasColor = color != Color.clear;
                if (hasColor)
                {
                    GUI.color = color;
                }

                var t = fileInfo.FieldType;
                var value = fileInfo.GetValue(target);
                if (t == typeof(float))
                {
                    var orignal = (float) value;
                    var newValue = EditorGUILayout.DelayedFloatField(showName, orignal);
                    if (!Mathf.Approximately(newValue, orignal))
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(double))
                {
                    var orignal = (double) value;
                    var newValue = EditorGUILayout.DelayedDoubleField(showName, orignal);
                    if (!Mathf.Approximately((float) newValue, (float) orignal))
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(int))
                {
                    var orignal = (int) value;
                    var newValue = EditorGUILayout.DelayedIntField(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(string))
                {
                    var orignal = (string) value;
                    var newValue = EditorGUILayout.DelayedTextField(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(bool))
                {
                    var orignal = (bool) value;
                    var newValue = EditorGUILayout.Toggle(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(Color))
                {
                    var orignal = (Color) value;
                    var newValue = EditorGUILayout.ColorField(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(Quaternion))
                {
                    var orignal = (Quaternion) value;
                    var vect4 = EditorGUILayout.Vector4Field(showName, new Vector4(orignal.x,orignal.y,orignal.z,orignal.w));
                    var newValue = new Quaternion(vect4.x, vect4.y, vect4.z, vect4.w);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(Vector3))
                {
                    var orignal = (Vector3) value;
                    var newValue = EditorGUILayout.Vector3Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(Vector2))
                {
                    var orignal = (Vector2) value;
                    var newValue = EditorGUILayout.Vector2Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t == typeof(Vector4))
                {
                    var orignal = (Vector4) value;
                    var newValue = EditorGUILayout.Vector4Field(showName, orignal);
                    if (orignal != newValue)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (t.IsEnum)
                {
                    var orignal = (Enum) value;
                    var newValue = t.IsDefined(typeof(FlagsAttribute), true)
                        ? EditorGUILayout.EnumFlagsField(showName, orignal)
                        : EditorGUILayout.EnumPopup(showName, orignal);
                    if (!Equals(orignal, newValue))
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }
                else if (typeof(Object).IsAssignableFrom(t))
                {
                    var orignal = (Object) value;
                    var newValue = EditorGUILayout.ObjectField(showName, orignal, t, true);
                    if (newValue != orignal)
                    {
                        fileInfo.SetValue(target, newValue);
                    }
                }

                if (hasColor)
                {
                    GUI.color = oc;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }


        private void CreateEditorButtonStates(MethodInfo[] methods)
        {
            _editorButtonStates = new EditorButtonState[methods.Length];
            var methodIndex = 0;
            foreach (var methodInfo in methods)
            {
                _editorButtonStates[methodIndex] = new EditorButtonState(methodInfo.GetParameters().Length);
                methodIndex++;
            }
        }

        private EditorButtonState GetEditorButtonState(MethodInfo method, int methodIndex)
        {
            return _editorButtonStates[methodIndex];
        }


        private void DrawButtonforMethod(MethodInfo methodInfo, EditorButtonState state)
        {
            EditorGUILayout.BeginHorizontal();
            var ba = (InspectorButtonAttribute) Attribute.GetCustomAttribute(methodInfo,
                typeof(InspectorButtonAttribute));
            GUI.enabled = !EditorApplication.isCompiling &&
                          (ba.Mode == InspectorDiplayMode.AlwaysEnabled || (EditorApplication.isPlaying
                               ? ba.Mode == InspectorDiplayMode.EnabledInPlayMode
                               : ba.Mode == InspectorDiplayMode.DisabledInPlayMode));

            var paramsNum = methodInfo.GetParameters().Length;
            if (paramsNum > 0)
            {
                var foldoutRect = EditorGUILayout.GetControlRect(GUILayout.Width(10.0f));
                state.Opened = EditorGUI.Foldout(foldoutRect, state.Opened, "");
            }
            else
            {
                state.Opened = false;
            }

            var showName = string.IsNullOrEmpty(ba.MethodName) ? MethodDisplayName(methodInfo) : ba.MethodName;
            var clicked = GUILayout.Button(showName, GUILayout.ExpandWidth(true));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            if (state.Opened)
            {
                EditorGUI.indentLevel++;
                var paramIndex = 0;
                foreach (var parameterInfo in methodInfo.GetParameters())
                {
                    var currentVal = state.Parameters[paramIndex];
                    state.Parameters[paramIndex] = DrawParameterInfo(parameterInfo, currentVal);
                    paramIndex++;
                }

                EditorGUI.indentLevel--;
            }

            if (clicked)
            {
                var returnVal = methodInfo.Invoke(target, state.Parameters);

                if (returnVal is IEnumerator enumerator)
                {
                    var mono = target as MonoBehaviour;
                    if (mono != null)
                    {
                        mono.StartCoroutine(enumerator);
                    }
                }
                else if (returnVal != null)
                    Debug.Log("Method call result -> " + returnVal);
            }
        }


        private object GetDefaultValue(ParameterInfo parameter)
        {
            var hasDefaultValue = !DBNull.Value.Equals(parameter.DefaultValue);

            if (hasDefaultValue) return parameter.DefaultValue;

            var parameterType = parameter.ParameterType;
            if (parameterType.IsValueType) return Activator.CreateInstance(parameterType);

            return null;
        }


        private object DrawParameterInfo(ParameterInfo parameterInfo, object currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(parameterInfo.Name);

            var drawer = GetParameterDrawer(parameterInfo);
            if (currentValue == null) currentValue = GetDefaultValue(parameterInfo);
            var paramValue = drawer.Invoke(parameterInfo, currentValue);

            EditorGUILayout.EndHorizontal();

            return paramValue;
        }

        private ParameterDrawer GetParameterDrawer(ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;

            if (typeof(Object).IsAssignableFrom(parameterType)) return DrawUnityEngineObjectParameter;

            return _typeDrawer.TryGetValue(parameterType, out var drawer) ? drawer : null;
        }


        private static object DrawFloatParameter(ParameterInfo parameterInfo, object val)
        {
            //Since it is legal to define a float param with an integer default value (e.g void method(float p = 5);)
            //we must use Convert.ToSingle to prevent forbidden casts
            //because you can't cast an "int" object to float 
            //See for http://stackoverflow.com/questions/17516882/double-casting-required-to-convert-from-int-as-object-to-float more info

            return EditorGUILayout.FloatField(Convert.ToSingle(val));
        }


        private static object DrawDoubleParameter(ParameterInfo parameterInfo, object val)
        {
            //Since it is legal to define a float param with an integer default value (e.g void method(float p = 5);)
            //we must use Convert.ToSingle to prevent forbidden casts
            //because you can't cast an "int" object to float 
            //See for http://stackoverflow.com/questions/17516882/double-casting-required-to-convert-from-int-as-object-to-float more info

            return EditorGUILayout.DoubleField(Convert.ToDouble(val));
        }


        private static object DrawIntParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.IntField((int) val);
        }


        private static object DrawLongParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.LongField((long) val);
        }

        private static object DrawBoolParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Toggle((bool) val);
        }

        private static object DrawStringParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.TextField((string) val);
        }

        private static object DrawColorParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.ColorField((Color) val);
        }

        private static object DrawUnityEngineObjectParameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.ObjectField((Object) val, parameterInfo.ParameterType, true);
        }

        private static object DrawVector2Parameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Vector2Field("", (Vector2) val);
        }

        private static object DrawVector3Parameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Vector3Field("", (Vector3) val);
        }

        private static object DrawQuaternionParameter(ParameterInfo parameterInfo, object val)
        {
            return Quaternion.Euler(EditorGUILayout.Vector3Field("", ((Quaternion) val).eulerAngles));
        }

        private static object DrawVector4Parameter(ParameterInfo parameterInfo, object val)
        {
            return EditorGUILayout.Vector4Field("", (Vector4) val);
        }

        private string MethodDisplayName(MethodInfo method)
        {
            var sb = new StringBuilder();
            sb.Append(method.Name + "(");
            var methodParams = method.GetParameters();
            foreach (var parameter in methodParams)
            {
                sb.Append(MethodParameterDisplayName(parameter));
                sb.Append(",");
            }

            if (methodParams.Length > 0) sb.Remove(sb.Length - 1, 1);

            sb.Append(")");
            return sb.ToString();
        }

        private string MethodParameterDisplayName(ParameterInfo parameterInfo)
        {
            if (!_typeDisplayName.TryGetValue(parameterInfo.ParameterType, out var parameterTypeDisplayName))
                parameterTypeDisplayName = parameterInfo.ParameterType.ToString();

            return parameterTypeDisplayName + " " + parameterInfo.Name;
        }

//        private string MethodUID(MethodInfo method)
//        {
//            var sb = new StringBuilder();
//            sb.Append(method.Name + "_");
//            foreach (var parameter in method.GetParameters())
//            {
//                sb.Append(parameter.ParameterType);
//                sb.Append("_");
//                sb.Append(parameter.Name);
//            }
//
//            sb.Append(")");
//            return sb.ToString();
//        }

        private class EditorButtonState
        {
            public readonly object[] Parameters;
            public bool Opened;

            public EditorButtonState(int numberOfParameters)
            {
                Parameters = new object[numberOfParameters];
            }
        }

        private delegate object ParameterDrawer(ParameterInfo parameter, object val);
    }
}