rm -rf publish
dotnet publish -r linux-arm -o publish
rsync -avzhe ssh --progress --delete --exclude appsettings.Production.json --exclude "Data" ./publish/ pi@192.168.1.12:/home/pi/brewtal
ssh pi@192.168.1.12 sudo systemctl restart brewtal.service