#!/bin/bash

echo 'iniciando aplicação'
migrate=$(head -n 1 '/migrate.txt')
echo `migrate: ${migrate}`

if [ -z $migrate ]; then #null
#just start the app
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
else 
#update database
	cd '/app/AEFWeb.Data' && 
	dotnet ef -s '/app/AEFWeb.Api' database update &&
#and start the app
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
fi

rm '/migrate.txt'

