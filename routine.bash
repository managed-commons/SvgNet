#!/bin/bash
echo "-------------" && \
echo "Formatting..." && \
echo "-------------" && \
dotnet format -v:m && \
echo "------------" && \
echo "Restoring..." && \
echo "------------" && \
dotnet restore -tl:off -v:m && \
echo "-----------" && \
echo "Building..." && \
echo "-----------" && \
dotnet build -tl:off -v:m --no-restore && \
echo "----------------" && \
echo "Running tests..." && \
echo "----------------" && \
dotnet test -c:release && \
echo "----------------" && \
echo "Publishing Nuget..." && \
echo "----------------" && \
dotnet build -t:NugetOrg -c:release


