#!/bin/bash
if [ "$(type -t droutine)" = "alias" ]; then
    droutine && pub_release
else
    dotnet test -c:release && dotnet build -t:NugetOrg -c:release
fi

