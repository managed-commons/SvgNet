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
dotnet build -tl:off -v:m --no-restore -c:release && \
echo "----------------" && \
echo "Running tests..." && \
echo "----------------" && \
dotnet test -tl:off -v:m --no-restore --no-build -c:release && \
echo "----------------" && \
echo "Publishing Nuget..." && \
echo "----------------" && \
dotnet build -tl:off -v:m --no-restore -t:NugetOrg -c:release


