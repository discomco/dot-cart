#! /bin/bash
for f in ./BLD/*.nupkg; do
   dotnet nuget push "$f" -k "$NUGET_API_KEY" -s "$NUGET_URL"
done



