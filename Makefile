.PHONY: test
test:
	dotnet test DotCart.root.sln --no-restore --verbosity minimal --logger trx --results-directory ./TST-RES
 
.PHONY: run
run:
	dotnet run --project examples/car/Engine.Api.Cmd 
 
