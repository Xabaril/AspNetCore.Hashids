#!/bin/bash

SdkVersion="5.0.403"
./dotnet-install.sh -Version $SdkVersion
export PATH="$PATH:$HOME/.dotnet"
