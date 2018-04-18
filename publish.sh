cd ./www-src
ng build --prod
cd ..
#dotnet restore
rm publish  -r -f
dotnet publish -r linux-arm -o publish

Robocopy.exe publish/ s:/brewtal -mir  -xf *.db
echo 'Going to kill app if running and then restart it. Type password and Ctrl+C when app is running.'
ssh pi@192.168.1.12 'sudo kill $(ps -A | pgrep -i brewtal) | (cd /share/brewtal/; sudo ./brewtal) &'

#scp -r bin/Debug/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.12:Apps/suvido

#rsync -avh -e ssh bin/Debug/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.12:Apps/suvido
#Remember: chmod u+x,o+x suvido