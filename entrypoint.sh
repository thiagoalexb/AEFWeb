#!/bin/bash

echo 'Iniciando web service'

if $migrate; then
#update database
	echo 'Migrating...';
	cd '/app/AEFWeb.Data' && 
	dotnet ef -s '/app/AEFWeb.Api' database update &&
#and start the app
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
else 
#just start the app
	echo 'Migration off';
	cd '/app/AEFWeb.Api' &&
	dotnet '/app/AEFWeb.Api/bin/Debug/netcoreapp2.0/AEFWeb.Api.dll';
fi

