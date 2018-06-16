#!/bin/bash

echo 'iniciando aplicação'
migrate=$(head -n 1 '/migrate.txt')
echo `migrate: ${migrate}`

if $migrate == '1'; then 
#update database
	cd '/app/AEFWeb.Data' && 
	dotnet ef -s '/app/AEFWeb.Api' database update &&
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
else 
#just start the app
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
fi

rm '/migrate.txt'

