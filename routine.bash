#!/bin/bash
dotnet test -c:release && dotnet build -t:NugetOrg -c:release
