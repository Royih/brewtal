rm publish  -r -f
dotnet publish -r linux-arm -o publish

#Robocopy.exe publish/ s:/brewtal -mir

rsync -avzhe ssh --progress --delete ./publish/ pi@192.168.86.12:/home/pi/brewtal