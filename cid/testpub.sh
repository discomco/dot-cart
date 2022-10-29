#! /bin/bash

cd cid || exit

export api_key="$NUGET_API_KEY"
export source="$NUGET_URL"

export version="$1"
dotnet nuget locals --clear all

dotnet add package -n -s "$source" -v "$version" "DotCart"
dotnet add package -n -s "$source" -v "$version" "DotCart.Behavior"
dotnet add package -n -s "$source" -v "$version" "DotCart.Contract"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Ardalis"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Bogus"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.EventStoreDB"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.InMem"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Microsoft"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Polly"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Serilog"
dotnet add package -n -s "$source" -v "$version" "DotCart.Effects"
dotnet add package -n -s "$source" -v "$version" "DotCart.Schema"
dotnet add package -n -s "$source" -v "$version" "DotCart.TestKit"

dotnet restore --disable-parallel 
#exit 0





