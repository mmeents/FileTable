# FileTable 
## Library of routines implementing a tables stored in text files.
### [install from NuGet link](https://www.nuget.org/packages/FileTables)
### Latest is 1.0.5 adds SettingsFile a file based name value pair object designed to hold settins for an app. 
### Version 1.0.4 Adds try blocks around load and save, empty value handeling.
### Version 1.0.2 updates row to long.
* ### By table I mean three properties.
  * ### FileName - to save the location of the file.
  * ### Columns - concurrent dictionary of columns
  * ### Rows - Concurrent dictionary of rows where
    * #### Each Row is a Dictionary of Fields that match the Columns.
* ### By stored in text files with MessagePack means
  * #### Creates a wirepack object that has 2 properties a list of column and list of row.
    * #### On Save the code copies the Dictionary to List in wirepack
      * #### uses MessagPack to translate the wirepack into Byte[] in which I convert to string via Base64 encoding the buffer.
      * #### uses a stream to write the encoding to file.
    * #### On Load it's reverse, Load the encoded string from file.
      * #### Decode Base 64 to byte[] then use MessagePack to deserialize back to wirepack object.
        * #### from wirepack object copy the list of columns and rows to active Model.   
   
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
  

