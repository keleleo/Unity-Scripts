using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;



public class ObjDataEditorWindow : EditorWindow
{
    [MenuItem("Window/UIElements/ObjectDataEditorWindow")]
    public static void ShowWindow()
    {
        var window = GetWindow<ObjDataEditorWindow>();
        window.titleContent = new GUIContent("ObjectData Editor");
        window.minSize = new Vector2(800, 600);
    }

    private void OnEnable()
    {
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ObjectWindow/EditorWindow.uxml");
        TemplateContainer treeAsset = original.CloneTree();
        rootVisualElement.Add(treeAsset);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ObjectWindow/EditorStyle.uss");
        rootVisualElement.styleSheets.Add(styleSheet); CreateListView();
    }

    private void CreateListView()
    {
        FindAllObjects(out ObjectData[] objects);

        ListView objectList = rootVisualElement.Query<ListView>("object-list").First();
        objectList.makeItem = () => new Label();
        objectList.bindItem = (element, i) => (element as Label).text = objects[i].name;

        objectList.itemsSource = objects;
        objectList.itemHeight = 16;
        objectList.selectionType = SelectionType.Single;

        objectList.onSelectionChanged += (enumerable) =>
        {
            foreach (Object it in enumerable)
            {
                Box objectInfoBox = rootVisualElement.Query<Box>("object-info").First();
                objectInfoBox.Clear();

                ObjectData obj = it as ObjectData;

                SerializedObject serializedObject = new SerializedObject(obj);
                SerializedProperty objectProperty = serializedObject.GetIterator();
                objectProperty.Next(true);

                while (objectProperty.NextVisible(false))
                {
                    PropertyField prop = new PropertyField(objectProperty);

                    prop.SetEnabled(objectProperty.name != "m_Script");
                    prop.Bind(serializedObject);
                    objectInfoBox.Add(prop);

                    if (objectProperty.name == "image")
                    {
                        prop.RegisterCallback<ChangeEvent<UnityEngine.Object>>((changeEvt) => LoadObjectImage(obj.image != null ? obj.image.texture : null));
                    }
                }

                LoadObjectImage(obj.image != null ? obj.image.texture : null);
            }
        };

        objectList.Refresh();
    }

    private void FindAllObjects(out ObjectData[] objects)
    {
        var guids = AssetDatabase.FindAssets("t:ObjectData");

        objects = new ObjectData[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(guids[i]);
            objects[i] = AssetDatabase.LoadAssetAtPath<ObjectData>(path);
        }
    }

    private void LoadObjectImage(Texture texture)
    {
        var objectPreviewImage = rootVisualElement.Query<Image>("preview").First();
        objectPreviewImage.image = texture;
    }

}
