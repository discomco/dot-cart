#! /bin/sh
dotnet test --test-adapter-path:. --logger:trx --results-directory ../../TST-RES --verbosity detailed --configuration Debug "$1"
