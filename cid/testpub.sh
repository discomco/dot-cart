#! /bin/bash

cd cid || exit

export api_key="$NUGET_API_KEY"
export source="$NUGET_URL"

export version="$1"
dotnet nuget locals --clear all

dotnet add package -n -s "$source" -v "$version" "DotCart.Abstractions"
dotnet add package -n -s "$source" -v "$version" "DotCart.Context"
dotnet add package -n -s "$source" -v "$version" "DotCart.Core"
dotnet add package -n -s "$source" -v "$version" "DotCart.Defaults"
dotnet add package -n -s "$source" -v "$version" "DotCart.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.TestKit"

dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Ardalis"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Bogus"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Console"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.CouchDB"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.CouchDB.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.ElasticSearch"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.EventStoreDB"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.EventStoreDB.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.K8S"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Kafka"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Microsoft"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.MongoDB"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.NATS"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.NATS.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.OTel"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Polly"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.RabbitMQ"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.RabbitMQ.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Redis"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Redis.TestFirst"
dotnet add package -n -s "$source" -v "$version" "DotCart.Drivers.Serilog"

dotnet restore --disable-parallel 
#exit 0





