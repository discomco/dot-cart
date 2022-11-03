#! /bin/sh
#dotnet test --test-adapter-path:. --logger:trx --results-directory ../../TST-RES --verbosity normal --no-restore --configuration Debug "$1"

dotnet test --verbosity normal --no-restore "$1"
