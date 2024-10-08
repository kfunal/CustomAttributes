# Custom Attributes

In this project, I have developed various custom attributes used in the Unity Inspector. Below are the descriptions of these attributes, how to use them, and the project license information.

## Features

### Horizontal Line
- **Description:** Used to draw a horizontal line in the Inspector window.
- **Customizability:** You can set the color of the line.
- **Usage:**
    ```csharp
    [HorizontalLine(Color.red)]
    public int exampleField;
    ```

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
- **Description:** Currently working on implementing a serializable dictionary for easier data management in the Inspector.
- **Usage:** This feature will allow you to serialize dictionary types for better integration with Unity's serialization system.

## Installation

1. Clone or download the project from GitHub and add it to your Unity project.
2. Define the attributes in your scripts to start using them.

## License

This project is licensed under the **MIT License**.
