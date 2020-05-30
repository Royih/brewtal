rm publish  -r -f
dotnet publish -r linux-arm -o publish

Robocopy.exe publish/ s:/brewtal -mir