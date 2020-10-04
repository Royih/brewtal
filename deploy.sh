rm -rf publish
dotnet publish -r linux-arm -o publish

#Robocopy.exe publish/ s:/brewtal -mir

rsync -avzhe ssh --progress --delete --exclude appsettings.Production.json ./publish/ pi@192.168.86.12:/home/pi/brewtal