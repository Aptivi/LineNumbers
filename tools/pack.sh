#!/bin/bash

# This script builds and packs the artifacts. Use when you have MSBuild installed.
version=$(cat version)
releaseconf=$1
if [ -z $releaseconf ]; then
	releaseconf=Release
fi

# Check for dependencies
zippath=`which zip`
if [ ! $? == 0 ]; then
	echo zip is not found.
	exit 1
fi

# Pack binary
echo Packing binary...
cd "../LineNumbers.Core/bin/$releaseconf/net8.0/" && "$zippath" -r /tmp/$version-bin.zip . && cd -
cd "../LineNumbers/bin/$releaseconf/net8.0/" && "$zippath" -r /tmp/$version-demo.zip . && cd -
cd "../LineNumbers.Core/bin/$releaseconf/net48/" && "$zippath" -r /tmp/$version-bin48.zip . && cd -
if [ ! $? == 0 ]; then
	echo Packing using zip failed.
	exit 1
fi

# Inform success
mv ~/tmp/$version-bin.zip .
mv ~/tmp/$version-demo.zip .
mv ~/tmp/$version-bin48.zip .
echo Build and pack successful.
exit 0
