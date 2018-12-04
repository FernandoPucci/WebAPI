FROM microsoft/iis:latest

RUN powershell -NoProfile -Command Remove-Item -Recurse C:\inetpub\wwwroot\*

WORKDIR /inetpub/wwwroot

COPY ${source:-obj/Docker/publish} .
