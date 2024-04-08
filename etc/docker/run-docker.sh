#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 62176a94-8eae-4901-8c0d-c4f082c7a368 -t
    fi
    cd ../
fi

docker-compose up -d
