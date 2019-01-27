#!/bin/sh

if [ ${APPVEYOR_REPO_TAG} == "true" ]
then
    dotnet pack ${APPVEYOR_BUILD_FOLDER}/src/Prometheus.Client.HttpRequestDurations -c Release --include-symbols --no-build -o artifacts/nuget
else
   dotnet pack ${APPVEYOR_BUILD_FOLDER}/src/Prometheus.Client.HttpRequestDurations -c Release --include-symbols --no-build --version-suffix build${APPVEYOR_BUILD_NUMBER} -o artifacts/myget
fi
