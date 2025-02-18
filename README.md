# FileTable 

## Overview
FileTable is a library of routines implementing tables stored in text files using MessagePack for serialization. It provides a simple way to manage tabular data with columns and rows, and save/load the data to/from text files.

### Installation
Install the library from NuGet:
[FileTables on NuGet](https://www.nuget.org/packages/FileTables)

### Latest Version
- **1.0.6**: Adds Unit Tests for code coverage, copilot rework and optimization found within.
- **1.0.5**: Adds `SettingsFile`, a file-based name-value pair object designed to hold settings for an app.
- **1.0.4**: Adds try blocks around load and save, and handles empty values.
- **1.0.2**: Updates row to long.

## Features
- **FileName**: Specifies the location of the file.
- **Columns**: Concurrent dictionary of columns.
- **Rows**: Concurrent dictionary of rows where each row is a dictionary of fields that match the columns.

## Serialization with MessagePack
- **Save**: 
  - Creates a wirepack object with a list of columns and rows.
  - Uses MessagePack to translate the wirepack into a byte array, which is then converted to a Base64 encoded string.
  - Writes the encoded string to a file.
- **Load**:
  - Reads the encoded string from the file.
  - Decodes the Base64 string to a byte array and deserializes it back to a wirepack object using MessagePack.
  - Copies the list of columns and rows from the wirepack object to the active model.

## Usage
Each `FileTable` can be created from scratch. Setting `Active = true` loads from the file if one exists. If the table does not exist, the developer needs to add the columns before adding rows.
Row Usage: 
```csharp table.Rows[12]["ID"].Value = "12";``` where first index is the row and second is the column name. 
### Example
Checkout AppSmith and Apiary they both use `FileTable` for data ID.
## FileTableViewer
An app to inspect and update `FileTable` via a standard C# GridView control. The full source is included. It is a work in progress and still needs new row, delete, and filtering functions.

### Features
- Grid loading in virtual mode.
- Grid sorting in virtual mode.
- Shows how to handle modifications and save or cancel write to disk.

## Main Objects

### FileTable
Represents a table stored in a text file.

#### Properties
- **Active**: Gets or sets the active state of the table.
- **FileName**: Gets or sets the file name.
- **Package**: Gets or sets the table wire package.
- **Columns**: Gets or sets the columns.
- **Rows**: Gets or sets the rows.

#### Methods
- **FileTable(string fileName)**: Initializes a new instance of the `FileTable` class.
- **GetActive()**: Gets the active state.
- **SetActive(bool value)**: Sets the active state.
- **Load()**: Loads the table from the file.
- **LoadAsync()**: Asynchronously loads the table from the file.
- **Save()**: Saves the table to the file.
- **SaveAsync()**: Asynchronously saves the table to the file.
- **AddColumn(string columnName, ColumnType columnType)**: Adds a column to the table.
- **AddRow()**: Adds a row to the table.

### SettingsFile
A file-based name-value pair object designed to hold settings for an app.

#### Properties
- **FileName**: Gets or sets the file name.
- **Settings**: Gets or sets the settings dictionary.

#### Methods
- **SettingsFile(string fileName)**: Initializes a new instance of the `SettingsFile` class.
- **Load()**: Loads the settings from the file.
- **LoadAsync()**: Asynchronously loads the settings from the file.
- **Save()**: Saves the settings to the file.
- **SaveAsync()**: Asynchronously saves the settings to the file.
- **Get(string key)**: Gets the value associated with the specified key.
- **Set(string key, string value)**: Sets the value for the specified key.

## Each FileTable can be created from scratch. 
  * Setting Active = true; loads from file if one exists. 
  * if your table didn't exist then the developer needs to add the columns before adding rows.
  * below is an example of how one could wrap the FileTable.
  * see sample output from [AppSmith App](https://github.com/mmeents/AppSmith) 

## FileTableViewer 
  * App to inspect and update FileTables via standard C# GridView Control.
  * Full source included. 
  * Work in progress, still needs new row, delete and filtering functions.
  * Show Grid loading in virtual mode.
  * Show Grid Sorting in virtual mode.
  * Show how one could show modification and Save or Cancel for write to disk.
  

