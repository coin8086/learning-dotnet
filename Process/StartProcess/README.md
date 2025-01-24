To publish a self contained and "trimed" executable for Linux x64:

1. Have the following in the .csproj file

  ```xml
    <PropertyGroup>
      <RuntimeIdentifiers>linux-x64</RuntimeIdentifiers>
      <PublishTrimmed>true</PublishTrimmed>
      <TrimMode>link</TrimMode>
    </PropertyGroup>
  ```

2. Publish it with the following command

  ```cmd
  dotnet.exe publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
  ```
