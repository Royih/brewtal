Raspberry PI Setup:
* Enable SSH & VNC
* Enabled SPI & I2C
* Installed Samba and configured /share with a folder "brewtal" where new veresion are Robo-copied in
* Installed MongoDB
* 
Dotnet core-app is hosted behind nginx with auto-restart feature:
    https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4


sudo systemctl start brewtal.service
sudo systemctl status brewtal.service
    