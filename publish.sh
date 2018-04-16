cd ./www-src
ng build --prod
cd ..
dotnet restore
dotnet publish -r linux-arm

scp -r bin/Debug/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.12:Apps/suvido

#rsync -avh -e ssh bin/Debug/netcoreapp2.0/linux-arm/publish/* pi@192.168.1.12:Apps/suvido

#Remember: chmod u+x,o+x suvido