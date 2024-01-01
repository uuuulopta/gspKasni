#!/bin/bash
# have to execute from its folder because appsetings won't get loaded properly for some reason...
cd /gspAPI
dotnet gspAPI.dll &
P1=$!
node /app/server.js > /app/server.log &
P2=$!
wait $P1 $P2
