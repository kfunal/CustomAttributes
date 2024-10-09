# Custom Attributes

In this project, I have developed various custom attributes used in the Unity Inspector. Below are the descriptions of these attributes, how to use them, and the project license information.

## Features

### Horizontal Line
- **Description:** Used to draw a horizontal line in the Inspector window.
- **Customizability:** You can set the color, thickness, and padding of the line.
- **Usage:**
    ```csharp
    [HorizontalLine(Color.red, Thickness: 2, Padding: 5)]
    public int exampleField;
    ```
- **Default Values:** If not specified, the line will use default values for **thickness**, **padding**, and **color**.

### Note
- **Description:** Used as an information note in the Inspector. It is displayed using a `HelpBox` and automatically selects the appropriate box type based on the message type.
- **Message Types:** `None`, `Info`, `Warning`, `Error`
- **Usage:**
    ```csharp
    [Note("This is an info note.", MessageType.Info)]
    public string exampleField;
    ```

### Expandable
- **Description:** Allows the given field to be shown in a `Foldout` in the Inspector. You can open and close this field to manage its contents.
- **Usage:**
    ```csharp
    [Expandable]
    public MyCustomClass exampleField;
    ```

### Serializable Dictionary
- **Description:** Implemented a serializable dictionary for easier data management in the Inspector. This allows for the serialization of dictionary types directly within the Unity Inspector.
- **Key Types:** The key type must be a non-nullable type. It cannot be a class type or `Object`, as these types can be `null`. Valid key types include `string`, `int`, `Color`, `bool`, `float`, `Vector2`, `Vector3`, and `Enum`.

- **Invalid Key Types:** The key type cannot be `SerializedPropertyType.Generic`. This type encompasses various scenarios where serialization is not possible. Specifically, it includes:
  - **Custom Classes/Structs:** If a class or struct is not marked with `[System.Serializable]` or does not have a default constructor, these types fall under `SerializedPropertyType.Generic`. This results from Unity's inability to serialize such types.
  - **Complex Types:** Unity cannot directly serialize complex object structures or data (e.g., a `Dictionary<K,V>`). Therefore, it is invalid to use such types as key types.
  - **Unity's Serialization System:** Unity can only serialize certain types under specific conditions. Thus, key types must be restricted to the predefined simple types (such as string, int, Color, etc.).

  Therefore, when using a serializable dictionary, the key types must be limited to those specified above. Using any invalid type as a key can lead to serialization errors.

- **Future Work:** I will continue to explore and expand the list of valid key types to enhance the flexibility of the Serializable Dictionary feature.

- **Usage:**
    ```csharp
    public SerializableDictionary<string, int> exampleDictionary;
    ```

## Installation

1. Clone or download the project from GitHub and add it to your Unity project.
2. Define the attributes in your scripts to start using them.

## License

This project is licensed under the **MIT License**.
