#! /bin/bash

cd cid || exit

export api_key="$NUGET_API_KEY"
export source="$NUGET_URL"

export version="$1"
dotnet nuget locals --clear all

dotnet add package -n -s "$source" -v "$version" "DotCart"
dotnet restore --disable-parallel 
#exit 0





